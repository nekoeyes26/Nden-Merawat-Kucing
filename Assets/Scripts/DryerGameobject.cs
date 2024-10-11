using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DryerGameobject : MonoBehaviour
{
    private Vector3 originalPosition;
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    private Animator dryerAnimator;
    public BathController bathController;

    private float holdTime = 2.0f;
    private float holdTimer = 0f;

    private bool isOverPet = false;
    public float moveSpeed = 0.2f;
    public float limitXMinPos = -10.5f;
    public float limitXMaxPos = 2.14f;
    public float limitYMinPos = -3.62f;
    public float limitYMaxPos = 3.6f;

    private void Start()
    {
        mainCamera = Camera.main;
        originalPosition = transform.position;
        dryerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
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
            newPosition.x = Mathf.Clamp(newPosition.x, limitXMinPos, limitXMaxPos);
            newPosition.y = Mathf.Clamp(newPosition.y, limitYMinPos, limitYMaxPos);
            transform.position = newPosition;
            dryerAnimator.SetBool("isDrying", true);
        }

        // Stop dragging when the mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            ReturnToOriginalPosition();
            dryerAnimator.SetBool("isDrying", false);
        }

        if (isOverPet && isDragging)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdTime)
            {
                bathController.IsDried = true;
                holdTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pet") && bathController.IsShowered && !bathController.IsDried)
        {
            isOverPet = true;
            // Debug.Log("Colliding with Pet, isOverPet: " + isOverPet);
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
}
