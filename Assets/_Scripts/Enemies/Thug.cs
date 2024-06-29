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
            StartCoroutine(nameof(EnemyDodge));
        }
        else
        if (col.name == "Trigger_EnemyCenterToAttack")
        {
            StopCoroutine(nameof(EnemyDodge));
            moveLeft = false; moveRight = false; center = true;
        }
        
        //if (col.name == "Trigger_Player") StartCoroutine(gameManager.EnemyAttack(1, "Thug", gameObject));
    }

}
