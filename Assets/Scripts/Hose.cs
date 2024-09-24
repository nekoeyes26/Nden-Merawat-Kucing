using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hose : MonoBehaviour
{
    private Transform[] children;
    [SerializeField] private Vector3[] originalPositions;
    private Quaternion[] originalRotations;
    public float moveSpeed;

    void Start()
    {
        children = new Transform[transform.childCount];
        originalPositions = new Vector3[transform.childCount];
        originalRotations = new Quaternion[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
            originalPositions[i] = children[i].localPosition;
            originalRotations[i] = children[i].localRotation;
        }
    }

    public void ResetChildrenSmoothly()
    {
        for (int i = 0; i < children.Length; i++)
        {
            StartCoroutine(SmoothMove(children[i].position, originalPositions[i], moveSpeed));
            StartCoroutine(SmoothRotate(children[i].rotation, originalRotations[i], moveSpeed));
        }
    }

    private IEnumerator SmoothMove(Vector3 startPos, Vector3 endPos, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
    }

    private IEnumerator SmoothRotate(Quaternion startRot, Quaternion endRot, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Lerp(startRot, endRot, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endRot; // Ensure the object reaches its final rotation
    }
}
