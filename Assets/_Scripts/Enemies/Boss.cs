using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Header("Prefabs")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject knifePrefab;

    public override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Arrow"))
        {
            if (isColliding) return;
            isColliding = true;
            print(hp + " - " + 2 + " = " + (hp - 2));
            hp -= 2;
            if (hp <= 0) StartCoroutine(EnemyDeath(col.gameObject));
            else StartCoroutine(EnemyHit(col.gameObject));
        }
        else
        if (col.name == "Trigger_EnemyCenterToAttack")
        {
            StartCoroutine(gameManager.EnemyAttack(1, "Boss", gameObject));
        }

        if (col.name == "Trigger_EnemyStartAI")
        {
            StartCoroutine(nameof(BossDodge));
        }
        else
        if (col.name == "Trigger_EnemyCenterToDrop")
        {
            StopCoroutine(nameof(BossDodge));
            moveLeft = false; moveRight = false; center = true;
        }

    }

    private IEnumerator BossDodge()
    {
        while (!center)
        {

            string[] actions = { "MoveForward", "MoveRight", "MoveLeft", "Jump", "JumpAttack" };
            int random = Random.Range(0, actions.Length);
            float moveTime = Random.Range(3f, 6f);
            yield return new WaitForSeconds(moveTime);

            switch (actions[random])
            {
                case "MoveForward":
                    moveRight = false;
                    moveLeft = false;
                    break;
                case "MoveRight":
                    moveRight = true;
                    moveLeft = false;
                    break;
                case "MoveLeft":
                    moveRight = false;
                    moveLeft = true;
                    break;
                case "Jump":
                    moveRight = false;
                    moveLeft = false;
                    if (canMove && isGrounded)
                    {
                        moveForward = false;
                        rb.velocity = Vector3.zero;
                        rb.AddForce(10f * jumpForce * Vector3.up, ForceMode.Impulse);
                        yield return new WaitForSeconds(3f);
                    }
                    break;
                case "JumpAttack":
                    moveRight = false;
                    moveLeft = false;
                    if (canMove && isGrounded)
                    {
                        moveForward = false;
                        rb.velocity = Vector3.zero;
                        rb.AddForce(10f * jumpForce * Vector3.up, ForceMode.Impulse);
                        yield return new WaitForSeconds(3f);
                    }
                    break;

            }
        }

    }
}
