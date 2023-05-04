using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Debug Spawn")]
    [SerializeField] string spawnThisAtStart;

    [Header("References")]
    [SerializeField] GameController gameController;
    [SerializeField] GameObject impactExplosion;
    [SerializeField] Transform player;
    [SerializeField] GameObject enemiesBar;

    [Header("Prefabs")]
    [SerializeField] List<GameObject> enemiesPrefabs;

    void Start()
    {
        player = Camera.main.transform;
        
        if (spawnThisAtStart != null) StartCoroutine(SpawnEnemies(spawnThisAtStart, 2f));
        //StartCoroutine(SpawnEnemies( 10 , 3, 5f));
    }

    IEnumerator SpawnEnemies(int enemiesToSpawn, int enemyType, float timeBetween)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject newEnemy = Instantiate(enemiesPrefabs[enemyType-1], transform.position, transform.rotation);
            newEnemy.GetComponent<Enemy>().gameController = gameController;
            newEnemy.GetComponent<Enemy>().impactExplosion = impactExplosion;
            newEnemy.GetComponent<LookAtCamera>().target = player;
            yield return new WaitForSeconds(timeBetween);
        }
    }
    IEnumerator SpawnEnemies(string enemiesToSpawn, float timeBetween)
    {
        for (int i = 0; i < enemiesToSpawn.Length; i++)
        {
            if (enemiesToSpawn[i] - '0' >= 1 && enemiesToSpawn[i] - '0' <= 4)
            {
                GameObject newEnemy = Instantiate(enemiesPrefabs[enemiesToSpawn[i] - '0' - 1], transform.position, transform.rotation);
                newEnemy.name = newEnemy.name + i;
                newEnemy.GetComponent<Enemy>().gameController = gameController;
                newEnemy.GetComponent<Enemy>().impactExplosion = impactExplosion;
                newEnemy.GetComponent<LookAtCamera>().target = player;
                yield return new WaitForSeconds(7);
            }
        }
    }
}
