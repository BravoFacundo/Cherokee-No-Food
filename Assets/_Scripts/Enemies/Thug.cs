using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thug : Enemy
{

    public override void Start() //esto tiene que estar mal. Fijarse si no poner nada es lo mismo
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
        if (col.name == "Trigger_EnemyReachPlayer")
        {
            StartCoroutine(gameController.DealDamage(1, "Thug", gameObject));
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
        
    }

}
