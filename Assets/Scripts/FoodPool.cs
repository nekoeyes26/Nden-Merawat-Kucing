using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FoodPool : MonoBehaviour
{
    [SerializeField] private FoodObject[] foodPrefabs;
    public float foodSpawnTime = 2f;
    private float timeUntilFoodSpawn;
    private int lastSpawnedIndex = -1;
    private int secondLastSpawnedIndex = -1;

    private FoodObject tempFood;
    [SerializeField] private List<FoodObject> activeFoodList = new List<FoodObject>();
    [SerializeField] private List<FoodObject> inactiveFoodList = new List<FoodObject>();
    [SerializeField] private Transform poolParent;
    public float maxLeftPosition = -15f;
    public Vector3 spawnPosition;

    private void Start()
    {

    }
    private void Update()
    {
        CheckFoodObjectPositions();
        timeUntilFoodSpawn += Time.deltaTime;
        if (activeFoodList.Count <= 0 && inactiveFoodList.Count <= 0)
        {
            SpawnFood(GetFoodToSpawn());
        }
        if (timeUntilFoodSpawn >= foodSpawnTime)
        {
            SpawnFood(GetFoodToSpawn());
            timeUntilFoodSpawn = 0;
        }
    }

    public FoodObject GetFoodToSpawn()
    {
        if (foodPrefabs.Length > 0)
        {
            int prefabIndex;
            do
            {
                prefabIndex = Random.Range(0, foodPrefabs.Length);
            } while (prefabIndex == lastSpawnedIndex && prefabIndex == secondLastSpawnedIndex);

            secondLastSpawnedIndex = lastSpawnedIndex;
            lastSpawnedIndex = prefabIndex;
            FoodObject foodToSpawn = foodPrefabs[prefabIndex];
            return foodToSpawn;
        }
        else
        {
            Debug.LogError("No food prefabs assigned");
            return null;
        }
    }

    private void SpawnFood(FoodObject foodObject)
    {
        FoodObject foundFood = null;
        foreach (FoodObject inactiveFood in inactiveFoodList)
        {
            if (inactiveFood.type == foodObject.type)
            {
                foundFood = inactiveFood;
                break;
            }
        }
        if (foundFood != null)
        {
            // If found, remove it from the inactive list and reuse it
            inactiveFoodList.Remove(foundFood);
            tempFood = foundFood;
        }
        else
        {
            // If not found, instantiate a new object
            tempFood = Instantiate(foodObject);
        }

        tempFood.transform.SetParent(poolParent);
        tempFood.transform.localPosition = spawnPosition;
        tempFood.InitializeObject();
        tempFood.gameObject.SetActive(true);
        activeFoodList.Add(tempFood);
    }

    public void RemoveFood(FoodObject target)
    {
        target.gameObject.SetActive(false);
        activeFoodList.Remove(target);
        inactiveFoodList.Add(target);
    }

    private void CheckFoodObjectPositions()
    {
        for (int i = activeFoodList.Count - 1; i >= 0; i--)
        {
            FoodObject food = activeFoodList[i];
            if (food.transform.localPosition.x < maxLeftPosition)
            {
                RemoveFood(food);
            }
        }
    }

    public void RestartFoods()
    {
        // Deactivate all active foods and move them back to the inactive queue
        foreach (FoodObject food in activeFoodList)
        {
            food.transform.position = transform.position;
            food.gameObject.SetActive(false);
            inactiveFoodList.Add(food);
        }

        // Clear the list of active foods
        activeFoodList.Clear();

        // Optionally reset other variables or states
        lastSpawnedIndex = -1;
        secondLastSpawnedIndex = -1;
        timeUntilFoodSpawn = 0f;
    }
}
