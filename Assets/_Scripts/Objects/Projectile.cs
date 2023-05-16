using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Move Variables")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 1f;
    private Rigidbody rb;
    private Transform childTransform;

    private Vector3 camPosition;

    public virtual void Start()
    {
        camPosition = Camera.main.transform.position + new Vector3(0, .25f, 0);
        rb = GetComponent<Rigidbody>();
        childTransform = transform.GetChild(0);
    }

    public virtual void FixedUpdate()
    {
        // Calculate direction to the camera
        Vector3 targetDirection = camPosition - transform.position;
        targetDirection.Normalize();

        // Calcula la aceleración y aplica una fuerza en esa dirección
        Vector3 force = targetDirection * moveSpeed;
        rb.AddForce(force);

        // Limita la velocidad a la velocidad máxima
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, moveSpeed);

        // Realiza la rotación del objeto hijo en su eje X local
        float rotationAmount = rotationSpeed * Time.deltaTime * 100;
        childTransform.Rotate(0, rotationAmount, 0, Space.Self);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.name == "Trigger_EnemyReachPlayer")
        {
            //StartCoroutine(gameManager.EnemyAttack(1, "Thug", gameObject));
        }

    }

}
