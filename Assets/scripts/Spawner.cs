using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> prefabsToSpawn; // List to store prefabs
    public float spawnInterval = 3f; // Base interval of 3 seconds
    public float nextSpawnTime;
    public List<List<GameObject>> waves;
    public int currentEnemyIndex = 0; // Track the index of the current enemy to spawn in the wave
    public int enemiesAlive = 0; // Track the number of enemies currently alive


    void Start()
    {
        nextSpawnTime = 0;
        GameObject batman = Resources.Load<GameObject>("Batman");
        GameObject zombie = Resources.Load<GameObject>("Zombie");
        GameObject ghost = Resources.Load<GameObject>("Ghost");

        waves = new List<List<GameObject>> {
            new List<GameObject> { batman, batman, batman },
            new List<GameObject> { ghost, batman, batman, ghost },
            new List<GameObject> { zombie, zombie, zombie, ghost, zombie },
        };

        GameManager.Instance.totalWaves = waves.Count; // Ensure the GameManager knows how many waves there are
    }

    void Update()
    {
        if (GameManager.Instance.isRoundActive && Time.time >= nextSpawnTime && currentEnemyIndex < waves[GameManager.Instance.currentWave - 1].Count)
        {
            SpawnEnemy(GameManager.Instance.currentWave - 1);
            nextSpawnTime = Time.time + spawnInterval + Random.Range(0f, 3f);
        }
    }

    void SpawnEnemy(int waveIndex)
    {
        GameObject enemyPrefab = waves[waveIndex][currentEnemyIndex];
        GameObject spawnedEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        Animator animator = spawnedEnemy.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("walking", true);
        }
        else
        {
            Debug.LogError("Spawned GameObject does not have an Animator component.");
        }
        currentEnemyIndex++;
        enemiesAlive++; // Increment count of enemies alive
        // spawnedEnemy.GetComponent<Batman>().OnDeath += OnEnemyDeath; // Subscribe to the death event of the enemy
    }

    public void OnEnemyDeath()
    {
        enemiesAlive--;
        Debug.Log(  "enemiesAlive: " + enemiesAlive + "\n" +
                    "currentEnemyIndex: " + currentEnemyIndex + "\n" +
                    "waves[GameManager.Instance.currentWave - 1].Count: " + waves[GameManager.Instance.currentWave - 1].Count + "\n");
        if (enemiesAlive <= 0 && currentEnemyIndex >= waves[GameManager.Instance.currentWave - 1].Count)
        {
            Debug.Log("EndRound");
            GameManager.Instance.EndRound();
        }
    }
}
