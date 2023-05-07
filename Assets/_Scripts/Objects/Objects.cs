using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{


    [SerializeField] private float startRotation;
    private Rigidbody rb;

    [Header("Ground Check")]
    public bool isGrounded;
    private GameObject groundCheck;
    public LayerMask groundMask;

    private void Start()
    {
        var target = Camera.main.gameObject.transform;
        transform.LookAt(target);
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }
    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, .5f, groundMask);

        if (startRotation > 0)
        {
            rb.AddTorque(0,0, .6f, ForceMode.Acceleration);
            startRotation--;
        }
        else
        {
            GetComponent<Rigidbody>().useGravity = true;
            rb.angularVelocity = Vector3.one;
        }

        if (isGrounded) rb.angularVelocity = Vector3.zero;
    }
}
