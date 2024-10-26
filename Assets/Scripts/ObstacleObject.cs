using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    Normal,
    Flying,
    Enemy
}
public class ObstacleObject : MonoBehaviour
{
    public float speed = 5f;
    public float speedRestoreDuration = 3f;
    public ObstacleType type;
    public float originalSpeed;
    // Start is called before the first frame update
    void Start()
    {
        originalSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.left;
    }

    public void SpeedReduction()
    {
        StartCoroutine(ReduceAndRestoreSpeed());
    }

    private IEnumerator ReduceAndRestoreSpeed()
    {
        speed = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < speedRestoreDuration)
        {
            speed = Mathf.Lerp(0f, originalSpeed, elapsedTime / speedRestoreDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the speed is fully restored to the original value
        speed = originalSpeed;
    }

    public void InitializeObject()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
