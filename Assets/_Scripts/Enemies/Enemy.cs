using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    AwaitNextAction,
    EvaluateNextAction,
    
    Idle,
    MovingForward,
    MovingLeft,
    MovingRight,
    MovingCenter,
    Jump,
    
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

    [Header ("Stats")]
    public float hp = 2;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Vector2 evadeSpeed;

    [Header("Move Variables")]
    [HideInInspector] public bool canMove = true;
    private bool? moveForward, moveLeft;
    private bool moveCenter, levitate;

    [Header("Move States")]
    [HideInInspector] public bool isGrounded;
    private bool wasGrounded = false;
    private LayerMask groundMask;
    [HideInInspector] public bool isJumping = false;
    [HideInInspector] public bool isLevitating = false;

    [Header("References")]
    [HideInInspector] public ParticleManager particleManager;
    [HideInInspector] public Collider currentCol; 

    [Header("Local References")]
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
        SetMovement(true);
        SetParticleEmission(false);
        ChangeState(EnemyState.MovingForward);
    }

    public virtual void Update()
    {
        GroundDetection();
        CheckParticleEmision();

        EnemyStateMachine();
    }

    public void ChangeState(EnemyState newState) => currentState = newState;
    private void EnemyStateMachine()
    {
        switch (currentState)
        {
            case EnemyState.AwaitNextAction:
                break;
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
        ChangeState(enemyEvadeActions[Random.Range(0, enemyEvadeActions.Count)]);
        float recallState = Random.Range(evadeSpeed.x, evadeSpeed.y);
        StartCoroutine(RecallEvaluateNextActionState(recallState));
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
        moveForward = null;
        animator.SetBool("EnemyIdle", true);
        ChangeState(EnemyState.AwaitNextAction);
    }
    public void EnemyMoveForward() => SetMovement(true, null);
    public void EnemyMoveLeft() => SetMovement(true, true);
    public void EnemyMoveRight() => SetMovement(true, false);

    public void EnemyMoveCenter()
    {
        moveForward = true;
        if (transform.position.x > 0.0f)
        {
            moveLeft = false;
        }
        if (transform.position.x < 0.0f)
        {
            moveLeft = true;
        }
        if (Mathf.Abs(transform.position.x) < 0.01f)
        {
            rb.velocity = new(0, rb.velocity.y, rb.velocity.z);
            moveLeft = null;
            ChangeState(EnemyState.AwaitNextAction);
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

    public virtual void EnemyHit() => StartCoroutine(nameof(EnemyHitAnimation));
    public IEnumerator EnemyHitAnimation()
    {
        canMove = false;
        yield return new WaitForSeconds(1f);
        canMove = true;
    }

    public virtual void EnemyDeath() => StartCoroutine(nameof(EnemyDeathAnimation));
    private IEnumerator EnemyDeathAnimation()
    {
        canMove = false;
        animator.SetTrigger("EnemyDeath");
        yield return new WaitForSeconds(Utilities.GetAnimationClipDurationByAction(animator, "Death"));
        EnemyDelete();
    }
    public void EnemyDelete()
    {
        Destroy(gameObject);
        //Delete enemy from enemy pool
    }

    //----------------------------------------------------------------------------------------------------------------------------

    protected virtual void OnTriggerEnter(Collider col)
    {
        currentCol = col;
        if (col.CompareTag("Arrow"))
        {
            EvaluateDamage(col.GetComponent<ArrowData>().arrowDamage);
            HandleDamageAnimation(col);
        }
    }
    protected virtual void OnTriggerExit(Collider col)
    {
        if (col == currentCol) currentCol = null;
    }

    //----------------------------------------------------------------------------------------------------------------------------

    public virtual void FixedUpdate()
    {
        HandleMovement();
        HandleMovementSpeed();
        HandleLevitate();
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
    }
    private void MoveForward(int direction) => ApplyMovement(-Vector3.forward * direction, moveSpeed);
    private void MoveHorizontal(int direction) => ApplyMovement(Vector3.right * direction, moveSpeed);
    public void ApplyMovement(Vector3 direction, float speed)
    {
        rb.AddForce(50f * speed * direction, ForceMode.Force);
    }
    private void HandleMovementSpeed()
    {
        Vector3 flatVel = new(rb.velocity.x, 0f, rb.velocity.z);
        float clampedSpeed = Mathf.Clamp(flatVel.magnitude, 0, moveSpeed);
        rb.velocity = new Vector3(flatVel.normalized.x * clampedSpeed, rb.velocity.y, flatVel.normalized.z * clampedSpeed);
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

    private void SetMovement(bool? forward, bool? left = null)
    {
        canMove = true;
        moveForward = forward;
        moveLeft = left;
        animator.SetBool("EnemyIdle", false);
        ChangeState(EnemyState.AwaitNextAction);
    }
    public void SetActive(bool b)
    {
        canMove = b;
        animator.enabled = b;
        rb.isKinematic = !b;

        if (b)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            ChangeState(EnemyState.AwaitNextAction);
        }
    }

}
