using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    MovingForward,
    MovingLeft,
    MovingRight,
    MovingCenter,
    Jump,
    
    EvaluateNextAction,
    
    Attack,
    JumpAttack,
    
    Hit,
    Death
}

public class Enemy : MonoBehaviour
{
    [Header("States")]
    public EnemyState currentState;
    public List<EnemyState> enemyEvadeActions;
    [HideInInspector] public bool isStateFirstEntry;

    [Header ("Stats")]
    public float hp = 2;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Vector2 evadeSpeed;

    [Header("Move Variables")]
    private bool canMove;
    private bool? moveForward, moveLeft;
    private bool moveCenter, levitate;
    [HideInInspector] public bool isLevitating = false;

    [Header("Ground Check")]    
    public bool isGrounded;
    public LayerMask groundMask;
    private bool wasGrounded = false;
    private GameObject groundCheck;

    [Header("References")]
    [HideInInspector] public PlayerController playerController; //Se usa?
    [HideInInspector] public ParticleManager particleManager;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public GameObject particles;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        particles = transform.Find("Particles").gameObject;
        groundMask = LayerMask.GetMask("Ground");
    }

    public virtual void Start()
    {
        canMove = true;
        moveLeft = null;
        ChangeState(EnemyState.MovingForward);
        SetParticleEmission(false);
    }

    public virtual void Update()
    {
        GroundDetection();
        CheckParticleEmision();

        EnemyStateMachine();
    }

    public void ChangeState(EnemyState newState)
    {
        isStateFirstEntry = true;
        currentState = newState;
    }
    private void EnemyStateMachine()
    {
        switch (currentState)
        {
            case EnemyState.EvaluateNextAction:
                EnemyEvaluateNextAction();
                break;

            case EnemyState.Idle:
                EnemyIdle();
                break;
            case EnemyState.MovingForward:
                EnemyMoveForward();
                break;
            case EnemyState.MovingLeft:
                EnemyMoveLeft();
                break;
            case EnemyState.MovingRight:
                EnemyMoveRight();
                break;
            case EnemyState.MovingCenter:
                EnemyMoveCenter();
                break;
            case EnemyState.Jump:
                EnemyJump();
                break;            

            case EnemyState.Attack:
                EnemyAttack();
                break;
            case EnemyState.JumpAttack:
                EnemyJumpAttack();
                break;

            case EnemyState.Hit:
                EnemyHit();
                break;
            case EnemyState.Death:
                EnemyDeath();
                break;
        }
    }

    public void EnemyEvaluateNextAction()
    {
        if (isStateFirstEntry)
        {
            isStateFirstEntry = false;
            ChangeState(enemyEvadeActions[Random.Range(0, enemyEvadeActions.Count)]);
            float recallState = Random.Range(evadeSpeed.x, evadeSpeed.y);
            StartCoroutine(RecallEvaluateNextActionState(recallState));
        }
    }
    private IEnumerator RecallEvaluateNextActionState(float recallState)
    {
        yield return new WaitForSeconds(recallState);
        if (currentState != EnemyState.MovingCenter ||
            currentState != EnemyState.Jump ||
            currentState != EnemyState.JumpAttack)
            ChangeState(EnemyState.EvaluateNextAction);
    }

    public void EnemyIdle()
    {
        if (isStateFirstEntry)
        {
            isStateFirstEntry = false;

            moveForward = null;            
            animator.SetBool("EnemyIdle", true);
        }
    }
    public void EnemyMoveForward()
    {
        if (isStateFirstEntry)
        {
            isStateFirstEntry = false;
            
            moveForward = true;            
            animator.SetBool("EnemyIdle", false);
        }
    }
    public void EnemyMoveLeft()
    {
        if (isStateFirstEntry)
        {
            isStateFirstEntry = false;
            
            moveForward = true; 
            moveLeft = true;
            animator.SetBool("EnemyIdle", false);
        }
    }
    public void EnemyMoveRight()
    {
        if (isStateFirstEntry)
        {
            isStateFirstEntry = false;
            
            moveForward = true; 
            moveLeft = false;
            animator.SetBool("EnemyIdle", false);
        }
    }
    
    public void EnemyMoveCenter()
    {
        if (isStateFirstEntry)
        {
            isStateFirstEntry = false;            
            moveForward = true;
        }

        if (transform.position.x > 0.0f)
        {
            moveLeft = false;
        }
        if (transform.position.x < 0.0f)
        {
            moveLeft = false;
        }
        if (Mathf.Abs(transform.position.x) < 0.01f)
        {
            moveLeft = null;
        }
    }
    
    public virtual void EnemyJump() { }
    public virtual void EnemyAttack() { }
    public virtual void EnemyJumpAttack() { }

    public void EvaluateDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0) ChangeState(EnemyState.Death);
        else ChangeState(EnemyState.Hit);
    }
    public void HandleDamageAnimation(Collider col)
    {
        particleManager.ImpactExplosion(col.transform.position + new Vector3(0, 0, -.1f), transform.rotation);
        Destroy(col);
    }

    public virtual void EnemyHit()
    {
        if (isStateFirstEntry)
        {
            isStateFirstEntry = false;
            StartCoroutine(nameof(EnemyHitAnimation));
        }
    }
    public IEnumerator EnemyHitAnimation()
    {
        moveForward = null;
        yield return new WaitForSeconds(1f);
        moveForward = true;
    }

    public virtual void EnemyDeath()
    {
        if (isStateFirstEntry)
        {
            isStateFirstEntry = false;
            StartCoroutine(nameof(EnemyDeathAnimation));
        }
    }
    private IEnumerator EnemyDeathAnimation()
    {
        moveForward = null;
        animator.SetTrigger("EnemyDeath");
        yield return new WaitForSeconds(Utilities.GetAnimationClipDurationByAction(animator, "Death"));
        Destroy(gameObject);
    }

    //----------------------------------------------------------------------------------------------------------------------------

    protected virtual void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Arrow"))
        {
            EvaluateDamage(col.GetComponent<ArrowData>().arrowDamage);
            HandleDamageAnimation(col);
        }
    }

    //----------------------------------------------------------------------------------------------------------------------------

    public virtual void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (canMove)
        {
            if (moveForward == true) MoveForward(1);
            else if (moveForward == false) MoveForward(-1);
            if (moveLeft == true) MoveHorizontal(1);
            else if (moveLeft == false) MoveHorizontal(-1);
        }
        HandleLevitate();
        HandleSpeedLimit();
    }

    private void MoveForward(int direction) => 
        rb.AddForce(50f * moveSpeed * (-Vector3.forward * direction), ForceMode.Force);
    private void MoveHorizontal(int direction) => 
        rb.AddForce(50f * moveSpeed * (Vector3.right * direction), ForceMode.Force);

    private void HandleSpeedLimit()
    {
        Vector3 flatVel = new(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    private void HandleLevitate()
    {
        if (levitate && !isLevitating)
        {
            isLevitating = true;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }
        else if (!levitate && isLevitating)
        {
            isLevitating = false;
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    //----------------------------------------------------------------------------------------------------------------------------

    public void GroundDetection() => isGrounded = Physics.CheckSphere(transform.position, .1f, groundMask);
    public void CheckParticleEmision()
    {
        if (isGrounded && !wasGrounded) SetParticleEmission(true);
        else if (!isGrounded && wasGrounded) SetParticleEmission(false);
    }
    public void SetParticleEmission(bool b)
    {
        wasGrounded = !wasGrounded;
        
        ParticleSystem[] particleSystems = particles.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            ParticleSystem.EmissionModule em = particleSystem.GetComponent<ParticleSystem>().emission;
            em.enabled = b;
        }
    }
 
}
