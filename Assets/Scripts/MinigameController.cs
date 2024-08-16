using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinigameController : MonoBehaviour
{
    public float jumpForce = 500;

    [SerializeField]
    private bool isGrounded = false;

    Rigidbody2D rb;
    public MinigameManager manager;

    public float enemyHitCooldown = 3.0f;
    private bool isEnemyHitCooldown = false;

    private BoxCollider2D boxCollider;
    // private Vector2 groundedSize = new Vector2(0.75f, 0.875f);
    // private Vector2 groundedOffset = new Vector2(0.375f, 0f);
    // private Vector2 notGroundedSize = new Vector2(1f, 0.875f);
    // private Vector2 notGroundedOffset = new Vector2(0.5f, 0f);
    // private float transitionDuration = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (isGrounded)
        // {
        //     StartCoroutine(TransitionToGroundedState());
        // }
        // else
        // {
        //     StartCoroutine(TransitionToNotGroundedState());
        // }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded == true && !isEnemyHitCooldown)
            {
                rb.AddForce(Vector2.up * jumpForce);
                isGrounded = false;
            }
        }
    }

    public void Jump()
    {
        if (isGrounded == true && !isEnemyHitCooldown)
        {
            rb.AddForce(Vector2.up * jumpForce);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (isGrounded == false)
            {
                isGrounded = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (isGrounded == true)
            {
                isGrounded = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Score"))
        {
            manager.AddScore();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Enemy") && !isEnemyHitCooldown)
        {
            manager.HittingEnemy();
            isEnemyHitCooldown = true;
            Invoke(nameof(ResetEnemyHitCooldown), enemyHitCooldown);
        }
    }

    private void ResetEnemyHitCooldown()
    {
        isEnemyHitCooldown = false;
    }

    // IEnumerator TransitionToGroundedState()
    // {
    //     float elapsedTime = 0;
    //     Vector2 initialSize = boxCollider.size;
    //     Vector2 initialOffset = boxCollider.offset;

    //     while (elapsedTime < transitionDuration)
    //     {
    //         float t = elapsedTime / transitionDuration;
    //         boxCollider.size = Vector2.Lerp(initialSize, groundedSize, t);
    //         boxCollider.offset = Vector2.Lerp(initialOffset, groundedOffset, t);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }

    //     boxCollider.size = groundedSize;
    //     boxCollider.offset = groundedOffset;
    // }

    // IEnumerator TransitionToNotGroundedState()
    // {
    //     float elapsedTime = 0;
    //     Vector2 initialSize = boxCollider.size;
    //     Vector2 initialOffset = boxCollider.offset;

    //     while (elapsedTime < transitionDuration)
    //     {
    //         float t = elapsedTime / transitionDuration;
    //         boxCollider.size = Vector2.Lerp(initialSize, notGroundedSize, t);
    //         boxCollider.offset = Vector2.Lerp(initialOffset, notGroundedOffset, t);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }

    //     boxCollider.size = notGroundedSize;
    //     boxCollider.offset = notGroundedOffset;
    // }
}
