using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thug : Enemy
{

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
            StartCoroutine(nameof(ThugDodge));
        }
        else
        if (col.name == "Trigger_EnemyCenterToAttack")
        {
            StopCoroutine(nameof(ThugDodge));
            EnemyCenter();
        }
        
        //if (col.name == "Trigger_Player") StartCoroutine(gameManager.EnemyAttack(1, "Thug", gameObject));
    }

    private IEnumerator ThugDodge()
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

}
