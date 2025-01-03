using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Soap : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject foamPrefab;
    private Vector3 originalPosition;
    private RectTransform rectTransform;
    private Canvas canvas;

    public float foamSpawnRate = 0.1f;
    private float foamSpawnTimer;

    private Queue<GameObject> foamQueue = new Queue<GameObject>();
    public int maxFoamCount = 10;

    public BathController bathController;
    public Image gloveImage;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalPosition = rectTransform.anchoredPosition;
        foamSpawnTimer = foamSpawnRate;
        if (gloveImage != null)
        {
            gloveImage.gameObject.SetActive(false);
            gloveImage.rectTransform.anchoredPosition = rectTransform.anchoredPosition;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (gloveImage != null)
        {
            gloveImage.gameObject.SetActive(true);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (gloveImage != null)
        {
            Vector2 newPosition = gloveImage.rectTransform.anchoredPosition + eventData.delta / canvas.scaleFactor;
            gloveImage.rectTransform.anchoredPosition = newPosition;
        }

        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        foamSpawnTimer -= Time.deltaTime;
        if (foamSpawnTimer <= 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            if (bathController.IsWet && !bathController.IsShowered)
            {
                if (hit.collider != null && hit.collider.CompareTag("Pet"))
                {
                    Vector3 foamPosition = new Vector3(worldPosition.x, worldPosition.y, 0);

                    GameObject spawnedFoam = Instantiate(foamPrefab, foamPosition, Quaternion.identity, hit.collider.transform);
                    foamQueue.Enqueue(spawnedFoam);

                    if (foamQueue.Count > maxFoamCount)
                    {
                        bathController.IsSoapy = true;
                        GameObject oldestFoam = foamQueue.Dequeue();
                        Destroy(oldestFoam);
                    }
                }
            }

            foamSpawnTimer = foamSpawnRate;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (gloveImage != null)
        {
            gloveImage.rectTransform.anchoredPosition = rectTransform.anchoredPosition; // Reset position to the soap's original position
            gloveImage.gameObject.SetActive(false); // Disable the glove
        }

        rectTransform.anchoredPosition = originalPosition; // Reset soap position (if needed)
    }
}
