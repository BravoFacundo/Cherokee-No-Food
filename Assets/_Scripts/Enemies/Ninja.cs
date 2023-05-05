using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : Enemy
{

    public override void Start()
    {
        base.Start();
    }
    public override void Update()
    {
        base.Update();

        if (isGrounded)
        {
            moveForward = true;
            //moveSpeed = ninjaSpeed;
        }        
        else
        {
            moveForward = false;
            //moveSpeed = ninjaSpeed * 2f;
        } 
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
            StartCoroutine(gameManager.EnemyAttack(1, "Ninja", gameObject));
        }

        if (col.name == "Trigger_EnemyStartAI")
        {
            StartCoroutine(nameof(NinjaDodge));
        }
        else
        if (col.name == "Trigger_EnemyCenterToDrop")
        {
            StopCoroutine(nameof(NinjaDodge));
            moveLeft = false; moveRight = false; center = true;
        }

    }

    private IEnumerator NinjaDodge()
    {
        while (!center)
        {

            string[] actions = { "MoveForward", "MoveRight", "MoveLeft", "JumpRight" };
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
                case "JumpRight":                    
                    moveRight = true ;
                    moveLeft  = false;
                    if (canMove && isGrounded)
                    {
                        moveForward = false;
                        rb.velocity = Vector3.zero;
                        rb.AddForce(10f * jumpForce * Vector3.up, ForceMode.Impulse);
                        yield return new WaitForSeconds(3f);
                    }
                    break;
                case "JumpLeft":
                    moveRight = false;
                    moveLeft  = true ;
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
