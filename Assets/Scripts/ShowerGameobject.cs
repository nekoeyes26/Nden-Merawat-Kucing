using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShowerGameobject : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 originalPosition;
    private Camera mainCamera;
    public bool isOverPet = false;
    public float moveSpeed = 0.2f;
    private Animator showerAnimator;
    public BathController bathController;
    private float holdTime = 2.0f;
    private float holdTimer = 0f;
    public Hose hose;

    private void Start()
    {
        mainCamera = Camera.main;
        originalPosition = transform.position;
        showerAnimator = GetComponent<Animator>();
        hose.moveSpeed = moveSpeed;
    }

    void Update()
    {
        // Detect mouse down on the object to start dragging
        if (Input.GetMouseButtonDown(0))
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
            showerAnimator.SetBool("isShowering", true);
        }

        // Stop dragging when the mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            ReturnToOriginalPosition();
            showerAnimator.SetBool("isShowering", false);
        }

        if (isOverPet)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdTime)
            {
                bathController.IsWet = true;
                // Debug.Log(bathController.IsWet);
                holdTimer = 0f;
            }
        }

        if (bathController.IsSoapy && !bathController.IsShowered && CountFoamObjects() == 0)
        {
            bathController.IsShowered = true;
        }
    }

    // Detect collision with "Pet" objects using 2D colliders
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pet") && !bathController.IsWet)
        {
            isOverPet = true;
            // Debug.Log("Colliding with Pet, isOverPet: " + isOverPet);
        }

        if (collision.gameObject.CompareTag("Foam"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pet"))
        {
            isOverPet = false;
            // Debug.Log("Stopped colliding with Pet, isOverPet: " + isOverPet);
        }
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

    private void ReturnToOriginalPosition()
    {
        StartCoroutine(SmoothMove(transform.position, originalPosition, moveSpeed));
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
        transform.position = endPos; // Ensure the object reaches its final position
    }

    public int CountFoamObjects()
    {
        int foamCount = 0;
        GameObject[] foams = GameObject.FindGameObjectsWithTag("Foam");
        foamCount = foams.Length;
        Debug.Log(foamCount);
        return foamCount;
    }
}
