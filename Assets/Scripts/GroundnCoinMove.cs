using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundnCoinMove : MonoBehaviour
{
    public float speed = 5f;
    public float speedRestoreDuration = 2f;
    // Start is called before the first frame update
    void Start()
    {

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
        float originalSpeed = speed;
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
}
