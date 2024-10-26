using UnityEngine;

public class CameraTouchControl : MonoBehaviour
{
    public float speed = 0.1f; // Speed at which the camera moves

    private Vector2 touchStartPos;
    private bool isTouching = false;

    void Update()
    {
        // Check for mouse button down
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            isTouching = true;
        }

        // Check for mouse movement
        if (Input.GetMouseButton(0) && isTouching)
        {
            Vector2 mouseDelta = (Vector2)Input.mousePosition - touchStartPos;
            transform.Translate(0, -mouseDelta.y * speed * Time.deltaTime, 0);
            touchStartPos = Input.mousePosition;
        }

        // Check for mouse button up
        if (Input.GetMouseButtonUp(0))
        {
            isTouching = false;
        }
    }
}