using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header ("Stats")]
    public int hp = 1;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Move Variables")]
    [SerializeField] public bool canMove;
    [SerializeField] public bool moveForward;
    [SerializeField] public bool moveRight;
    [SerializeField] public bool moveLeft;
    [SerializeField] public bool center;

    private Vector3 moveDirection;

    public bool isColliding;
    
    [Header("Ground Check")]    
    public bool isGrounded;
    private GameObject groundCheck;
    public LayerMask groundMask;
    private bool isGroundedLock = false;

    [Header("References")]
    public GameObject impactExplosion;
    [HideInInspector] public GameController gameController;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public GameObject particles;
    private Camera cam;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        animator = GetComponentInChildren<Animator>();
        groundMask = LayerMask.GetMask("Ground");
        particles = transform.Find("Particles").gameObject;
        moveForward = true;
    }

    public virtual void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, .5f , groundMask);

        if (isGrounded && !isGroundedLock) SetParticleEmition(true);
        else if (!isGrounded && isGroundedLock) SetParticleEmition(false);
    }

    public virtual void FixedUpdate()
    {
        if (canMove)
        {
            if (center) 
            {
                if (transform.position.x > 0.0f) moveLeft = true;
                if (transform.position.x < 0.0f) moveRight = true;
                if (transform.position.x > -0.01f && transform.position.x < 0.01f)
                {
                    center = false; moveLeft = false; moveRight = false;
                    rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
                }
            }
            //Cuando se esta siendo atacado, centrar al enemigo a -0.9 o 0.9 dependiendo del lado del que venga
            //Esto es para que el jugador sepa que se acerca otro enemigo. Y tenga feedback de su game over

            //Base Movement
            if (moveForward) rb.AddForce(50f * moveSpeed * -Vector3.forward, ForceMode.Force);
            if (moveRight) rb.AddForce(50f * moveSpeed * Vector3.right, ForceMode.Force);
            if (moveLeft) rb.AddForce(50f * moveSpeed * -Vector3.right, ForceMode.Force);

            //Speed Limit
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.name == "EnemyMoveStartArea")
        {
            StartCoroutine(nameof(EnemyDodge));
        }
        else
        if (col.name == "EnemyMoveEndArea")
        {
            StopCoroutine(nameof(EnemyDodge));
            moveLeft = false; moveRight = false; center = true;
        }        

    }

    public IEnumerator EnemyHit(GameObject arrow)
    {
        canMove = false;
        //Damage Animation
        GameObject newImpactExplosion = Instantiate(impactExplosion, arrow.transform.position, transform.rotation);
        newImpactExplosion.transform.position = Vector3.MoveTowards(newImpactExplosion.transform.position, Camera.main.transform.position, 1.4f);
        newImpactExplosion.GetComponent<LookAtCamera>().target = cam.transform;
        Destroy(arrow);
        yield return new WaitForSeconds(1f);
        canMove = true;
        isColliding = false;
        yield return new WaitForSeconds(.5f);
        Destroy(newImpactExplosion);
    }
    public IEnumerator EnemyDeath(GameObject arrow)
    {
        canMove = false;

        GameObject newImpactExplosion = Instantiate(impactExplosion, arrow.transform.position + new Vector3(0, 0, -1f), transform.rotation);
        newImpactExplosion.GetComponent<LookAtCamera>().target = cam.transform;
        Destroy(arrow);
        animator.SetTrigger("EnemyDeath");
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        Destroy(newImpactExplosion);
    }

    public IEnumerator EnemyDodge() 
    {
        while (!center)
        {
            string[] actions = { "MoveForward", "MoveRight", "MoveLeft" };
            int random = Random.Range(0, actions.Length);
            float moveTime = Random.Range(3f, 6f);
            yield return new WaitForSeconds(moveTime);

            switch (actions[random])
            {
                case "MoveForward":                    
                    moveLeft = false;
                    moveRight = false;
                    break;
                case "MoveRight":
                    moveRight = true;
                    moveLeft = false;
                    break;
                case "MoveLeft":
                    moveLeft = true;
                    moveRight = false;
                    break;
            }
        }
    }

    public void SetParticleEmition(bool b)
    {
        isGroundedLock = !isGroundedLock;
        
        ParticleSystem[] particleSystems = particles.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            ParticleSystem.EmissionModule em = particleSystem.GetComponent<ParticleSystem>().emission;
            em.enabled = b;
        }
    }
 
}
