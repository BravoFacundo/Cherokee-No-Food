using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sumo : Enemy
{
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (isGrounded) animator.speed = 1f;
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

        if (col.name == "Trigger_EnemyStartAI")
        {
            StartCoroutine(nameof(SumoDodge));
        }
        else
        if (col.name == "Trigger_EnemyCenterToAttack")
        {
            StopCoroutine(nameof(SumoDodge));
            EnemyCenter();
        }
                
        //if (col.name == "Trigger_Player") StartCoroutine(gameManager.EnemyAttack(1, "Sumo", gameObject));
    }

    private IEnumerator SumoDodge()
    {
        while (!center)
        {            

            string[] actions = { "MoveForward", "MoveRight", "MoveLeft", "Jump" };
            int random = Random.Range(0, actions.Length);
            float moveTime = Random.Range(3f, 6f);
            yield return new WaitForSeconds(moveTime);

            switch (actions[random])
            {
                case "MoveForward":
                    moveRight = false; 
                    moveLeft  = false;
                    break;
                case "MoveRight":
                    moveRight = true ;  
                    moveLeft  = false;
                    break;
                case "MoveLeft":
                    moveRight = false; 
                    moveLeft  = true ;
                    break;
                case "Jump":
                    moveRight = false; 
                    moveLeft  = false;
                    if (canMove && isGrounded)
                    {
                        rb.velocity = Vector3.zero;
                        rb.AddForce(10f * jumpForce * Vector3.up, ForceMode.Impulse);
                        yield return new WaitForSeconds(3f);
                    }                    
                    break;                
            }            
        }
    }

}
