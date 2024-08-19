using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnHandler : MonoBehaviour
{
    [Header("Debug Spawn")]
    [SerializeField] string spawnThisAtStart;
    [SerializeField] float startDelay;

    [Header("Spawn Points")]
    [SerializeField] Vector3 doorSpawPoint;
    [SerializeField] Vector3 windowSpawPoint;

    [Header("References")]
    [SerializeField] GameManager gameManager;
    [SerializeField] PlayerController playerController;
    [SerializeField] ParticleManager particleController;
    private Transform player;

    [Header("Prefabs")]
    [SerializeField] List<GameObject> enemiesPrefabs;

    private IEnumerator Start()
    {
        player = Camera.main.transform;

        yield return new WaitForSeconds(startDelay);
        if (spawnThisAtStart != null) StartCoroutine(SpawnEnemi());        
    }

    private IEnumerator SpawnEnemi()
    {
        yield return new WaitForSeconds(4f);
        SpawnEnemy(1, doorSpawPoint);
    }

    public void SpawnEnemies(string enemiesToSpawn) => SpawnEnemies(enemiesToSpawn, 0);
    public IEnumerator SpawnEnemies(string enemiesToSpawn, float timeBetween)
    {
        for (int i = 0; i < enemiesToSpawn.Length; i++)
        {
            SpawnEnemy(int.Parse(enemiesToSpawn[i].ToString()), doorSpawPoint);
            yield return new WaitForSeconds(timeBetween);
        }
    }
    public void SpawnEnemy(int enemyToSpawn, Vector3 startPosition)
    {
        Mathf.Clamp(enemyToSpawn, 1, 4);
        GameObject newEnemy = Instantiate(enemiesPrefabs[enemyToSpawn-1], startPosition, Quaternion.identity);
        UpdateEnemyReferences(newEnemy, enemyToSpawn);
    }

    private void UpdateEnemyReferences(GameObject newEnemy, int enemyToSpawn)
    {
        var newEnemyScript = newEnemy.GetComponent<Enemy>();
        newEnemyScript.playerController = playerController;
        newEnemyScript.particleManager = particleController;        
        newEnemy.GetComponent<LookAtCamera>().target = player;

        gameManager.AddNewEnemy(newEnemy, enemyToSpawn);
    }

}
