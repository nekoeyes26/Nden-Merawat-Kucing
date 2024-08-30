using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    public float obstacleSpawnTime = 2f;
    private float timeUntilObstacleSpawn;

    // private Queue<GameObject> obstaclePool = new Queue<GameObject>();
    // private int maxPoolSize = 7;

    private int lastSpawnedIndex = -1;
    private int secondLastSpawnedIndex = -1;

    public GroundnCoinMove groundnCoinMove;
    private float originalGroundSpeed;
    private List<GameObject> activeObstacleList = new List<GameObject>();
    private Queue<GameObject> inactiveObstacleQueue = new Queue<GameObject>();

    private void Start()
    {
        originalGroundSpeed = groundnCoinMove.speed;
    }
    private void Update()
    {
        timeUntilObstacleSpawn += Time.deltaTime;
        if (timeUntilObstacleSpawn >= obstacleSpawnTime)
        {
            if (activeObstacleList.Count > 0)
            {
                GameObject obstacle = activeObstacleList[0];
                GroundnCoinMove groundnCoinMoves = obstacle.GetComponent<GroundnCoinMove>();
                // Debug.Log("obstacle 0 speed: " + groundnCoinMoves.speed);
                if (groundnCoinMoves.speed == originalGroundSpeed)
                {
                    if (activeObstacleList.Count() >= 7)
                    {
                        GameObject outframeObstacle = activeObstacleList[Random.Range(0, 4)];
                        RemoveObstacle(outframeObstacle);
                    }
                    else
                    {
                        SpawnObstacle();
                    }
                    timeUntilObstacleSpawn = 0;
                }
            }
            else
            {
                SpawnObstacle();
                timeUntilObstacleSpawn = 0;
            }
        }
    }

    // private void SpawnLoop()
    // {
    //     timeUntilObstacleSpawn += Time.deltaTime;
    //     if (timeUntilObstacleSpawn >= obstacleSpawnTime)
    //     {
    //         GroundnCoinMove[] groundnCoinObjects = FindObjectsOfType<GroundnCoinMove>();
    //         if (groundnCoinObjects.Length > 0)
    //         {
    //             // Debug.Log("index 0 " + groundnCoinObjects[0].speed);
    //             if (groundnCoinObjects[0].speed == originalGroundSpeed)
    //             {
    //                 Spawn();
    //                 timeUntilObstacleSpawn = 0;
    //             }
    //         }
    //         else
    //         {
    //             Spawn();
    //             timeUntilObstacleSpawn = 0;
    //         }
    //     }
    // }

    // private void Spawn()
    // {
    //     int prefabIndex;
    //     do
    //     {
    //         prefabIndex = Random.Range(0, obstaclePrefabs.Length);
    //     } while (prefabIndex == lastSpawnedIndex && prefabIndex == secondLastSpawnedIndex);

    //     secondLastSpawnedIndex = lastSpawnedIndex;
    //     lastSpawnedIndex = prefabIndex;
    //     GameObject obstacleToSpawn = obstaclePrefabs[prefabIndex];
    //     GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);
    //     obstaclePool.Enqueue(spawnedObstacle);

    //     if (obstaclePool.Count > maxPoolSize)
    //     {
    //         GameObject oldestObstacle = obstaclePool.Dequeue();
    //         Destroy(oldestObstacle);
    //     }
    // }

    // public void Restart()
    // {
    //     while (obstaclePool.Count > 0)
    //     {
    //         GameObject obstacleToDestroy = obstaclePool.Dequeue();
    //         Destroy(obstacleToDestroy);
    //     }
    //     lastSpawnedIndex = -1;
    //     secondLastSpawnedIndex = -1;
    //     timeUntilObstacleSpawn = 0f;
    // }

    private void SpawnObstacle()
    {
        GameObject tempObstacle;
        if (inactiveObstacleQueue.Count > 0)
        {
            tempObstacle = inactiveObstacleQueue.Dequeue();
        }
        else
        {
            int prefabIndex;
            do
            {
                prefabIndex = Random.Range(0, obstaclePrefabs.Length);
            } while (prefabIndex == lastSpawnedIndex && prefabIndex == secondLastSpawnedIndex);

            secondLastSpawnedIndex = lastSpawnedIndex;
            lastSpawnedIndex = prefabIndex;
            GameObject obstacleToSpawn = obstaclePrefabs[prefabIndex];
            tempObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);
        }

        tempObstacle.transform.position = transform.position;
        foreach (Transform child in tempObstacle.transform)
        {
            child.gameObject.SetActive(true);
        }
        tempObstacle.SetActive(true);
        activeObstacleList.Add(tempObstacle);
    }

    private void RemoveObstacle(GameObject target)
    {
        target.SetActive(false);
        activeObstacleList.Remove(target);
        inactiveObstacleQueue.Enqueue(target);
    }

    public void RestartObstacles()
    {
        // Deactivate all active obstacles and move them back to the inactive queue
        foreach (GameObject obstacle in activeObstacleList)
        {
            obstacle.transform.position = transform.position;
            GroundnCoinMove groundnCoinMoves = obstacle.GetComponent<GroundnCoinMove>();
            groundnCoinMoves.speed = 5;
            obstacle.SetActive(false);
            inactiveObstacleQueue.Enqueue(obstacle);
        }

        // Clear the list of active obstacles
        activeObstacleList.Clear();

        // Optionally reset other variables or states
        lastSpawnedIndex = -1;
        secondLastSpawnedIndex = -1;
        timeUntilObstacleSpawn = 0f;
    }
}
