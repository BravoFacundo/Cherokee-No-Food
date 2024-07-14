using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Debug Spawn")]
    [SerializeField] string spawnThisAtStart;
    [SerializeField] float delayedStart;

    [Header("Spawn Points")]
    [SerializeField] Transform doorSpawPoint;
    [SerializeField] Transform windowSpawPoint;

    [Header("References")]
    [SerializeField] PlayerController playerController;
    [SerializeField] GameManager gameManager;
    [SerializeField] ParticleManager particleController;
    [SerializeField] GameObject impactExplosion;
    private Transform player;
    [SerializeField] GameObject enemiesBar;


    [Header("Prefabs")]
    [SerializeField] List<GameObject> enemiesPrefabs;

    void Start()
    {
        player = Camera.main.transform;
        
        if (spawnThisAtStart != null) StartCoroutine(DelayedStart(delayedStart));
        //StartCoroutine(SpawnEnemies( 10 , 3, 5f));
    }

    private IEnumerator DelayedStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(SpawnEnemies(spawnThisAtStart, 2f));
    }

    IEnumerator SpawnEnemies(int enemiesToSpawn, int enemyType, float timeBetween)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject newEnemy = Instantiate(enemiesPrefabs[enemyType-1], doorSpawPoint.position, doorSpawPoint.rotation);
            newEnemy.GetComponent<Enemy>().playerController = playerController;
            newEnemy.GetComponent<Enemy>().particleController = particleController;
            //newEnemy.GetComponent<Enemy>().impactExplosion = impactExplosion;
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
                GameObject newEnemy = Instantiate(enemiesPrefabs[enemiesToSpawn[i] - '0' - 1], doorSpawPoint.position, doorSpawPoint.rotation);
                newEnemy.name = newEnemy.name + i;
                newEnemy.GetComponent<Enemy>().playerController = playerController;
                newEnemy.GetComponent<Enemy>().particleController = particleController;
                //newEnemy.GetComponent<Enemy>().impactExplosion = impactExplosion;
                newEnemy.GetComponent<LookAtCamera>().target = player;
                
                //Estoy tomando la primera. Esto se rompe si tiene mas de 5 enemigos osea mas de 1 barra
                enemiesBar.transform.GetChild(0).GetChild(i).GetComponent<Animator>().SetInteger("EnemyReveal", enemiesToSpawn[i] - '0');

                yield return new WaitForSeconds(3);
            }
        }
    }
}
