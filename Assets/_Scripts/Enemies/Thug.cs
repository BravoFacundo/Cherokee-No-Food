using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thug : Enemy
{
    protected override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);

        if (col.name == "Trigger_05Meters") ChangeState(EnemyState.EvaluateNextAction);
        else if (col.name == "Trigger_45Meters") ChangeState(EnemyState.MovingCenter);
    }
}

