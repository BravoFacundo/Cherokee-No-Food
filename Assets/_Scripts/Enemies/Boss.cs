using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Header("States")]
    [SerializeField] private List<EnemyState> bossStates;

    [Header("Configuration")]
    [SerializeField] private float jumpMaxHeight;
    [SerializeField] private float landRecoveryTime;

    [Header("Prefabs")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private GameObject landSmokePrefab;

    private bool _isGroundedLock = false;

    public override void Start()
    {
        base.Start();
        StartCoroutine(nameof(EnemySpawn));
    }

    public override void Update()
    {
        base.Update();
        if (isGrounded && !_isGroundedLock) StartCoroutine(nameof(EnemyLand));
        else if (!isGrounded) _isGroundedLock = false;
    }

    protected override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);

        if (col.name == "Trigger_05Meters") ChangeState(EnemyState.EvaluateNextAction);
        else if (col.name == "Trigger_42Meters") ChangeState(EnemyState.MovingCenter);
        else if (col.name == "Trigger_45Meters") ChangeState(EnemyState.Attack);
    }

    private IEnumerator EnemySpawn()
    {
        rb.AddForce(2f * jumpForce * Vector3.up, ForceMode.Impulse);
        yield return new WaitForSeconds(.3f);
        //levitate = true;
        yield return new WaitForSeconds(2f);
        //canMove = false;
        //levitate = false;
    }
    private IEnumerator EnemyLand()
    {
        _isGroundedLock = true;
        animator.SetTrigger("isLanding");
        var ar = new Vector3(transform.position.x, 0, transform.position.z);
        Instantiate(landSmokePrefab, ar, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        //canMove = true;
        animator.SetTrigger("isRunning");
        //StartCoroutine(nameof(BossDodge));
    }
}