using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    [SerializeField] private ObstacleObject[] obstaclePrefabs;
    public float obstacleSpawnTime = 2f;
    private float timeUntilObstacleSpawn;

    // private Queue<ObstacleObject> obstaclePool = new Queue<ObstacleObject>();
    // private int maxPoolSize = 7;

    private int lastSpawnedIndex = -1;
    private int secondLastSpawnedIndex = -1;

    [SerializeField] private ObstacleObject tempObstacle;
    private float originalObstacleSpeed;
    private List<ObstacleObject> activeObstacleList = new List<ObstacleObject>();
    private List<ObstacleObject> inactiveObstacleList = new List<ObstacleObject>();
    [SerializeField] private Transform poolParent;
    public float maxLeftPosition = -15f;

    private void Start()
    {
        originalObstacleSpeed = tempObstacle.speed;
    }
    private void Update()
    {
        CheckObstacleObjectPositions();
        timeUntilObstacleSpawn += Time.deltaTime;
        if (timeUntilObstacleSpawn >= obstacleSpawnTime)
        {
            if (activeObstacleList.Count > 0)
            {
                ObstacleObject obstacleObject = activeObstacleList[0];
                if (obstacleObject.speed == originalObstacleSpeed)
                {
                    SpawnObstacle(GetObstacleToSpawn());
                    timeUntilObstacleSpawn = 0;
                }
            }
            else
            {
                SpawnObstacle(GetObstacleToSpawn());
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
    //             if (groundnCoinObjects[0].speed == originalObstacleSpeed)
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
    //     ObstacleObject obstacleToSpawn = obstaclePrefabs[prefabIndex];
    //     ObstacleObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);
    //     obstaclePool.Enqueue(spawnedObstacle);

    //     if (obstaclePool.Count > maxPoolSize)
    //     {
    //         ObstacleObject oldestObstacle = obstaclePool.Dequeue();
    //         Destroy(oldestObstacle);
    //     }
    // }

    // public void Restart()
    // {
    //     while (obstaclePool.Count > 0)
    //     {
    //         ObstacleObject obstacleToDestroy = obstaclePool.Dequeue();
    //         Destroy(obstacleToDestroy);
    //     }
    //     lastSpawnedIndex = -1;
    //     secondLastSpawnedIndex = -1;
    //     timeUntilObstacleSpawn = 0f;
    // }

    public ObstacleObject GetObstacleToSpawn()
    {
        if (obstaclePrefabs.Length > 0)
        {
            int prefabIndex;
            do
            {
                prefabIndex = Random.Range(0, obstaclePrefabs.Length);
            } while (prefabIndex == lastSpawnedIndex && prefabIndex == secondLastSpawnedIndex);

            secondLastSpawnedIndex = lastSpawnedIndex;
            lastSpawnedIndex = prefabIndex;
            ObstacleObject obstacleToSpawn = obstaclePrefabs[prefabIndex];
            Debug.Log("Now: " + obstacleToSpawn.type);
            return obstacleToSpawn;
        }
        else
        {
            Debug.LogError("No obstacle prefabs assigned");
            return null;
        }
    }

    private void SpawnObstacle(ObstacleObject obstacleObject)
    {
        ObstacleObject foundObstacle = null;
        foreach (ObstacleObject inactiveObs in inactiveObstacleList)
        {
            if (inactiveObs.type == obstacleObject.type)
            {
                foundObstacle = inactiveObs;
                break;
            }
        }
        if (foundObstacle != null)
        {
            // If found, remove it from the inactive list and reuse it
            inactiveObstacleList.Remove(foundObstacle);
            tempObstacle = foundObstacle;
        }
        else
        {
            // If not found, instantiate a new object
            tempObstacle = Instantiate(obstacleObject);
        }

        tempObstacle.transform.SetParent(poolParent);
        tempObstacle.transform.position = transform.position;
        tempObstacle.InitializeObject();
        tempObstacle.gameObject.SetActive(true);
        activeObstacleList.Add(tempObstacle);
    }

    private void RemoveObstacle(ObstacleObject target)
    {
        target.gameObject.SetActive(false);
        target.speed = originalObstacleSpeed;
        activeObstacleList.Remove(target);
        inactiveObstacleList.Add(target);
    }

    private void CheckObstacleObjectPositions()
    {
        for (int i = activeObstacleList.Count - 1; i >= 0; i--)
        {
            ObstacleObject obstacle = activeObstacleList[i];
            if (obstacle.transform.position.x < maxLeftPosition)
            {
                RemoveObstacle(obstacle);
            }
        }
    }

    public void RestartObstacles()
    {
        // Deactivate all active obstacles and move them back to the inactive queue
        foreach (ObstacleObject obstacle in activeObstacleList)
        {
            obstacle.transform.position = transform.position;
            obstacle.speed = 5;
            obstacle.gameObject.SetActive(false);
            inactiveObstacleList.Add(obstacle);
        }

        // Clear the list of active obstacles
        activeObstacleList.Clear();

        // Optionally reset other variables or states
        lastSpawnedIndex = -1;
        secondLastSpawnedIndex = -1;
        timeUntilObstacleSpawn = 0f;
    }
}
