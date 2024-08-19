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
    }

    public override void EnemyJump()
    {
        StartCoroutine(nameof(SumoJump));
        ChangeState(EnemyState.AwaitNextAction);
    }
    private IEnumerator SumoJump()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(10f * jumpForce * Vector3.up, ForceMode.Impulse);
        yield return new WaitForSeconds(3f);
        ChangeState(EnemyState.EvaluateNextAction);
    }

}
