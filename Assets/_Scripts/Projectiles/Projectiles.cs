using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    [Header("Stats")]
    [HideInInspector] public float moveSpeed = 5f;

    [Header("Move Variables")]
    [SerializeField] public bool canMove;
    private Vector3 moveDirection;

    [Header("References")]
    public GameObject impactExplosion;
    [HideInInspector] public GameManager gameController;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public GameObject particles;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        particles = transform.Find("Particles").gameObject;
    }

    public virtual void FixedUpdate()
    {
        if (canMove)
        {
            //Base Movement
            rb.AddForce(50f * moveSpeed * -Vector3.forward, ForceMode.Force);

            //Speed Limit
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    public IEnumerator ProjectileDeath(GameObject arrow)
    {
        canMove = false;

        GameObject newImpactExplosion = Instantiate(impactExplosion, arrow.transform.position + new Vector3(0, 0, -1f), transform.rotation);
        newImpactExplosion.GetComponent<LookAtCamera>().target = Camera.main.transform;
        Destroy(arrow);
        animator.SetTrigger("ProjectileDeath");
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        Destroy(newImpactExplosion);
    }

}
