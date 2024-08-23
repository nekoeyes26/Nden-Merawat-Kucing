using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    public float obstacleSpawnTime = 2f;
    private float timeUntilObstacleSpawn;

    private Queue<GameObject> obstaclePool = new Queue<GameObject>();
    private int maxPoolSize = 7;

    private int lastSpawnedIndex = -1;
    private int secondLastSpawnedIndex = -1;

    public GroundnCoinMove groundnCoinMove;
    private float originalGroundSpeed;

    private void Start()
    {
        originalGroundSpeed = groundnCoinMove.speed;
    }
    private void Update()
    {
        SpawnLoop();
    }

    private void SpawnLoop()
    {
        timeUntilObstacleSpawn += Time.deltaTime;
        if (timeUntilObstacleSpawn >= obstacleSpawnTime)
        {
            GroundnCoinMove[] groundnCoinObjects = FindObjectsOfType<GroundnCoinMove>();
            if (groundnCoinObjects.Length > 0)
            {
                // Debug.Log("index 0 " + groundnCoinObjects[0].speed);
                if (groundnCoinObjects[0].speed == originalGroundSpeed)
                {
                    Spawn();
                    timeUntilObstacleSpawn = 0;
                }
            }
            else
            {
                Spawn();
                timeUntilObstacleSpawn = 0;
            }
        }
    }

    private void Spawn()
    {
        int prefabIndex;
        do
        {
            prefabIndex = Random.Range(0, obstaclePrefabs.Length);
        } while (prefabIndex == lastSpawnedIndex && prefabIndex == secondLastSpawnedIndex);

        secondLastSpawnedIndex = lastSpawnedIndex;
        lastSpawnedIndex = prefabIndex;
        GameObject obstacleToSpawn = obstaclePrefabs[prefabIndex];
        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);
        obstaclePool.Enqueue(spawnedObstacle);

        if (obstaclePool.Count > maxPoolSize)
        {
            GameObject oldestObstacle = obstaclePool.Dequeue();
            Destroy(oldestObstacle);
        }
    }

    public void Restart()
    {
        while (obstaclePool.Count > 0)
        {
            GameObject obstacleToDestroy = obstaclePool.Dequeue();
            Destroy(obstacleToDestroy);
        }
        lastSpawnedIndex = -1;
        secondLastSpawnedIndex = -1;
        timeUntilObstacleSpawn = 0f;
    }
}
