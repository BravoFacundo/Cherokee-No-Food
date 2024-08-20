using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : Enemy
{
    [Header("Configuration")]
    [SerializeField] private float jumpMaxHeight;
    [SerializeField] private float landRecoveryTime;

    [Header("Prefabs")]
    [SerializeField] private GameObject trunkPrefab;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject shurikenPrefab;

    public override void Update()
    {
        base.Update();
        if (isGrounded)
        {
            //moveForward = true;
            //moveSpeed = ninjaSpeed;
        }
        else
        {
            //moveForward = false;
            //moveSpeed = ninjaSpeed * 2f;
        }
    }

    protected override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);

        if (col.name == "Trigger_05Meters") ChangeState(EnemyState.EvaluateNextAction);
        else if (col.name == "Trigger_42Meters") ChangeState(EnemyState.MovingCenter);
        else if (col.name == "Trigger_45Meters") ChangeState(EnemyState.Attack);
    }

    public override void EnemyAttack()
    {
        StartCoroutine(nameof(EnemyJumpAnimation));
        ChangeState(EnemyState.AwaitNextAction);
    }
    private IEnumerator EnemyJumpAnimation()
    {
        GameObject newBomb = Instantiate(bombPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
        yield return new WaitForSeconds(2f);
    }
    public override void EnemyJumpAttack()
    {
        StartCoroutine(nameof(EnemyJumpAttackAnimation));
        ChangeState(EnemyState.AwaitNextAction);
    }
    private IEnumerator EnemyJumpAttackAnimation()
    {
        yield return new WaitForSeconds(2f);
    }

    public override void EnemyHit()
    {
        StartCoroutine(nameof(EnemyHitAnimation));
        ChangeState(EnemyState.AwaitNextAction);
    }
    private IEnumerator EnemyHitAnimation(GameObject arrow)
    {
        //canMove = false;
        particleManager.ImpactExplosion(arrow.transform.position, transform.rotation);
        GameObject newTrunk = Instantiate(trunkPrefab, arrow.transform.position, transform.rotation);
        arrow.GetComponent<Rigidbody>().isKinematic = true;
        arrow.transform.parent = newTrunk.transform;
        arrow.transform.position = newTrunk.transform.position;
        arrow.transform.localScale *= 2f;
        Destroy(arrow.transform.GetChild(0).GetChild(0).gameObject);
        Destroy(arrow.transform.GetChild(0).GetChild(1).gameObject);
        Destroy(arrow.transform.GetChild(1).gameObject);
        //arrow.GetComponent<MeshRenderer>().enabled = true;
        Destroy(gameObject); //No se debe matar al ninja, reposicionarlo y reusarlo. Solo que ya sin la vida extra.
        yield return new WaitForSeconds(2f);
        Destroy(newTrunk);
        //canMove = true;
    }
}