using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sumo : Enemy
{
    public override void Update()
    {
        base.Update();

        if (isGrounded) animator.speed = 1f;
    }    

    protected override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);

        if (col.name == "Trigger_05Meters") ChangeState(EnemyState.EvaluateNextAction);
        else if (col.name == "Trigger_45Meters") ChangeState(EnemyState.MovingCenter);
        else if (col.name == "Player_Collision") ChangeState(EnemyState.Attack);
    }

    public override void EnemyJump() { if (!isJumping) StartCoroutine(nameof(EnemyJumpAnimation)); }
    private IEnumerator EnemyJumpAnimation()
    {
        isJumping = true;
        rb.velocity = Vector3.zero;
        rb.AddForce(10f * jumpForce * Vector3.up, ForceMode.Impulse);
        yield return new WaitForSeconds(3f);
        isJumping = false;
        ChangeState(EnemyState.EvaluateNextAction);
    }

    public override void EnemyAttack()
    {
        PlayerController playerController = currentCol.GetComponent<PlayerCollision>().playerController;
        StartCoroutine(playerController.EnemyAttack(1, this.GetType().ToString(), gameObject));
        Utilities.RepositionToObjectPool(gameObject);
        this.SetActive(false);
    }

}
