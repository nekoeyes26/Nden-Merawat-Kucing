using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Soap : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject foamPrefab; // The foam prefab to instantiate on the animal
    private Vector3 originalPosition; // The original position of the soap
    private RectTransform rectTransform;
    private Canvas canvas;

    public float foamSpawnRate = 0.1f; // Time interval between spawning foam
    private float foamSpawnTimer; // Timer to control foam spawn rate

    private Queue<GameObject> foamQueue = new Queue<GameObject>(); // Queue to manage foam objects
    public int maxFoamCount = 10; // Maximum number of foam objects allowed

    public BathController bathController;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalPosition = rectTransform.anchoredPosition;
        foamSpawnTimer = foamSpawnRate; // Initialize the timer
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Optional: Do something when drag starts (e.g., play a sound)
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the soap with the mouse/touch position
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        // Convert the screen position to a world position
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Control the spawn rate using a timer
        foamSpawnTimer -= Time.deltaTime;
        if (foamSpawnTimer <= 0)
        {
            // Perform a raycast to detect if the soap is over an animal
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            if (bathController.IsWet && !bathController.IsShowered)
            {
                if (hit.collider != null && hit.collider.CompareTag("Pet"))
                {
                    // Adjust the world position to set the Z position to -1
                    Vector3 foamPosition = new Vector3(worldPosition.x, worldPosition.y, -1);

                    // Spawn foam at the adjusted position
                    GameObject spawnedFoam = Instantiate(foamPrefab, foamPosition, Quaternion.identity, hit.collider.transform);
                    foamQueue.Enqueue(spawnedFoam);

                    // If the foam count exceeds the maximum, destroy the oldest foam
                    if (foamQueue.Count > maxFoamCount)
                    {
                        bathController.IsSoapy = true;
                        GameObject oldestFoam = foamQueue.Dequeue();
                        Destroy(oldestFoam);
                    }
                }
            }

            // Reset the spawn timer
            foamSpawnTimer = foamSpawnRate;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = originalPosition;
    }
}
