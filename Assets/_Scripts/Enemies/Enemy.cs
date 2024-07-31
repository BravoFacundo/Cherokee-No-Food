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
    public bool canMove;
    public bool moveForward, moveRight, moveLeft, moveCenter;
    public bool levitate;
    [HideInInspector] public bool levitateLock = false;

    [Header("Debug Variables")]
    public bool isColliding;
    
    [Header("Ground Check")]    
    public bool isGrounded;
    private GameObject groundCheck;
    public LayerMask groundMask;
    private bool isGroundedLock = false;

    [Header("References")]
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public ParticleManager particleController;
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
        isGrounded = Physics.CheckSphere(transform.position, .1f , groundMask);

        if (isGrounded && !isGroundedLock) SetParticleEmition(true);
        else if (!isGrounded && isGroundedLock) SetParticleEmition(false);

        if (canMove) animator.SetBool("EnemyIdle", false);
        else animator.SetBool("EnemyIdle", true);
    }

    public virtual void FixedUpdate()
    {
        if (canMove)
        {
            if (moveCenter) 
            {
                if (transform.position.x > 0.0f) moveLeft = true;
                if (transform.position.x < 0.0f) moveRight = true;
                if (transform.position.x > -0.01f && transform.position.x < 0.01f)
                {
                    moveCenter = false; moveLeft = false; moveRight = false;
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
        if (levitate && !levitateLock)
        {
            levitateLock = true;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }
        else if (!levitate && levitateLock)
        {
            levitateLock = false;
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    public void EnemyMoveCenter()
    {
        moveLeft = false; moveRight = false;
        moveCenter = true;
    }

    public IEnumerator EnemyHit(GameObject arrow)
    {
        canMove = false;
        particleController.ImpactExplosion(arrow.transform.position, transform.rotation);
        Destroy(arrow);
        yield return new WaitForSeconds(1f);
        canMove = true;
        isColliding = false;
    }
    public IEnumerator EnemyDeath(GameObject arrow)
    {
        canMove = false;
        particleController.ImpactExplosion(arrow.transform.position, transform.rotation);
        Destroy(arrow);
        animator.SetTrigger("EnemyDeath");
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
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
