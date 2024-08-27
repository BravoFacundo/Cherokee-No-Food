using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeEnemy : Enemy
{
    [Header("Configuration")]
    public float jumpMaxHeight;
    public float landRecoveryTime;

    [Header("Prefabs")]
    public GameObject bombPrefab;
    public GameObject projectilePrefab;
    public GameObject landSmokePrefab;
}
