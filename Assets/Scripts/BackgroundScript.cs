using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    public float animationSpeed = 1f;
    private MeshRenderer meshRenderer;
    public float speedRestoreDuration = 2f;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        meshRenderer.material.mainTextureOffset += new Vector2(animationSpeed * Time.deltaTime, 0);
    }

    public void SpeedReduction()
    {
        StartCoroutine(ReduceAndRestoreSpeed());
    }

    private IEnumerator ReduceAndRestoreSpeed()
    {
        float originalSpeed = animationSpeed;
        animationSpeed = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < speedRestoreDuration)
        {
            animationSpeed = Mathf.Lerp(0f, originalSpeed, elapsedTime / speedRestoreDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the speed is fully restored to the original value
        animationSpeed = originalSpeed;
    }
}
