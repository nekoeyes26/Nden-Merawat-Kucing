using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType
{
    Ayam,
    Daging,
    Ikan,
    Kol,
    Susu,
    Wortel
}
public class FoodObject : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 originalPosition;
    private Camera mainCamera;
    private float moveSpeed = 0.2f;
    public float speed = 2f;

    private bool isOverPet = false;

    [SerializeField] private FeedController feedController;
    [SerializeField] private FoodPool foodPool;
    public FoodType type;
    public bool interactable = true;

    private void Start()
    {
        mainCamera = Camera.main;
        originalPosition = transform.position;
        feedController = FindObjectOfType<FeedController>();
        foodPool = FindObjectOfType<FoodPool>();
    }

    void Update()
    {
        originalPosition += speed * Time.deltaTime * Vector3.left;
        if (feedController.activityComplete)
        {
            interactable = false;
        }
        if (!isDragging)
        {
            transform.position = originalPosition;
        }
        // Detect mouse down on the object to start dragging
        if (Input.GetMouseButtonDown(0) && interactable)
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            if (IsMouseOverObject(mousePosition))
            {
                isDragging = true;
                offset = transform.position - mousePosition;
            }
        }

        // While dragging, move the object to follow the mouse
        if (isDragging)
        {
            Vector3 newPosition = GetMouseWorldPosition() + offset;
            transform.position = newPosition;
            feedController.CatSteady();
            GameEvents.DraggingFood(true);
        }

        // Stop dragging and check if the drop is successful or not
        if (Input.GetMouseButtonUp(0))
        {
            if (isOverPet && isDragging)
            {
                // Drop successful on a "Pet" object, return instantly to original position
                // Debug.Log("Dropped on Pet");
                // ReturnToOriginalPositionInstantly();
                // transform.gameObject.SetActive(false);
                feedController.AddPoint(transform.gameObject);
                foodPool.RemoveFood(this);
            }
            else
            {
                // Drop failed, smoothly return to original position
                // Debug.Log("Drop failed, returning to original position.");
                // ReturnToOriginalPositionSmoothly();
                ReturnToOriginalPositionInstantly();
            }
            isDragging = false;
            feedController.CatBackIdle();
            GameEvents.DraggingFood(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pet"))
        {
            isOverPet = true;
            Debug.Log("Object is over Pet");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Pet"))
        {
            isOverPet = false;
            // Debug.Log("Object is no longer over Pet");
        }
    }

    // Function to return the object to its original position instantly
    private void ReturnToOriginalPositionInstantly()
    {
        transform.position = originalPosition;
    }

    // Function to smoothly return the object to its original position
    private void ReturnToOriginalPositionSmoothly()
    {
        StartCoroutine(SmoothMove(transform.position, originalPosition, moveSpeed));
    }

    // Coroutine to smoothly move the object back to its original position
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

    // Helper method to check if the mouse is over the object
    private bool IsMouseOverObject(Vector3 mousePosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        return hit.collider != null && hit.collider.gameObject == gameObject;
    }

    // Get mouse position in world space
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = mainCamera.WorldToScreenPoint(transform.position).z; // Maintain z position
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }

    public void InitializeObject()
    {
        originalPosition = transform.position;
    }
}
