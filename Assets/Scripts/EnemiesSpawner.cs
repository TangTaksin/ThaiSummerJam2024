using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int poolSize = 10;
    public float spawnRate = 2f;
    public Vector2 spawnArea; // Define the area where enemies can spawn

    private List<GameObject> pooledEnemies = new List<GameObject>();
    private float nextSpawnTime;

    void Start()
    {
        // Initialize the object pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            pooledEnemies.Add(enemy);
        }
    }

    void Update()
    {
        // Spawn enemies at a regular interval
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnEnemy()
    {
        // Find a disabled enemy object from the pool and activate it
        for (int i = 0; i < pooledEnemies.Count; i++)
        {
            if (!pooledEnemies[i].activeInHierarchy)
            {
                // Randomize spawn position within the spawn area
                float spawnX = Random.Range(-spawnArea.x / 2, spawnArea.x / 2);
                float spawnY = Random.Range(-spawnArea.y / 2, spawnArea.y / 2);
                Vector2 spawnPosition = new Vector2(spawnX, spawnY);

                // Set the enemy's position and activate it
                pooledEnemies[i].transform.position = spawnPosition;
                pooledEnemies[i].SetActive(true);
                return;
            }
        }
    }
}
