using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinigameController : MonoBehaviour
{
    public float jumpForce = 500;

    [SerializeField]
    public bool isGrounded = false;

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
    private float jumpCooldownTime = 0.25f;
    private bool isJumping = false;
    // SpineAnimationController SpineAnimationController.instance;
    private bool isFalling = false;
    private float distanceToGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SpineAnimationController.instance.initialized)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            isGrounded = IsGrounded();
            //Debug.Log("isGrounded: " + isGrounded);
            if (isGrounded)
            {
                isFalling = false;
                if (SpineAnimationController.instance.currentAnimation.Equals(SpineAnimationController.instance.jumpDown.name) || SpineAnimationController.instance.currentAnimation.Equals(SpineAnimationController.instance.jumpUp.name))
                {
                    // Debug.Log("Landing");
                    SpineAnimationController.instance.PlayAnimation(SpineAnimationController.instance.landing, false, 1f);
                }
                else if (SpineAnimationController.instance.currentAnimation.Equals(SpineAnimationController.instance.landing.name))
                {
                    // code to wait until animation finish
                    // TrackEntry trackEntry = SpineAnimationController.instance.skeletonAnimation.state.GetCurrent(0);
                    // if (trackEntry != null && !trackEntry.IsComplete)
                    // {
                    //     return; // wait until next frame
                    // }
                    if (SpineAnimationController.instance.isLandingPlaying)
                    {
                        return;
                    }
                    SpineAnimationController.instance.PlayAnimation(SpineAnimationController.instance.run, true, 1f);
                    isJumping = false;
                }
                else
                {
                    if (SpineAnimationController.instance.isBumpPlaying) return;
                    SpineAnimationController.instance.PlayAnimation(SpineAnimationController.instance.run, true, 1f);
                }
            }
            else
            {
                // isGrounded = false;  // Player is in the air

                // Check the Rigidbody2D's velocity for upward or downward movement
                if (rb.velocity.y > 0 && isJumping)
                {
                    // Player is going up, ensure the jump up animation is playing
                    if (!SpineAnimationController.instance.currentAnimation.Equals(SpineAnimationController.instance.jumpUp.name))
                    {
                        // Debug.Log("naikk");
                        SpineAnimationController.instance.PlayAnimation(SpineAnimationController.instance.jumpUp, false, 1f);
                    }
                }
                else if (rb.velocity.y < 0)
                {
                    // Player is falling, play jump down animation
                    if (!isFalling)
                    {
                        // Debug.Log("turunnn");
                        SpineAnimationController.instance.PlayAnimation(SpineAnimationController.instance.jumpDown, false, 1f);
                        isFalling = true;  // Ensure we only play this once until landing
                    }
                }
            }
        }
    }

    public void Jump()
    {
        if (isGrounded && !isJumping && !isEnemyHitCooldown)
        {
            rb.AddForce(Vector2.up * jumpForce);
            SpineAnimationController.instance.PlayAnimation(SpineAnimationController.instance.jumpUp, false, 0.5f);
            isGrounded = false;
            isJumping = true;
            Invoke(nameof(ResetJumping), jumpCooldownTime);
        }
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         if (isGrounded == false)
    //         {
    //             isGrounded = true;
    //         }
    //     }
    // }

    // private void OnCollisionExit2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         if (isGrounded == true)
    //         {
    //             isGrounded = false;
    //         }
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Score"))
        {
            manager.AddScore();
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Enemy") && !isEnemyHitCooldown)
        {
            manager.HittingEnemy();
            isEnemyHitCooldown = true;
            GameEvents.HitEnemyCooldown(isEnemyHitCooldown);
            SpineAnimationController.instance.PlayAnimation(SpineAnimationController.instance.bump, false, 1f);
            Invoke(nameof(ResetEnemyHitCooldown), enemyHitCooldown);
        }
    }

    private void ResetEnemyHitCooldown()
    {
        isEnemyHitCooldown = false;
        GameEvents.HitEnemyCooldown(isEnemyHitCooldown);
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

    private void ResetJumping()
    {
        isJumping = false;
    }

    private bool IsGrounded()
    {
        float rayLength = 0.2f;
        Vector2 rayOrigin = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength);

        if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            return true;
        }

        return false;
    }

    private bool IsFalling()
    {
        float rayLength = 2f; // Longer ray to check if player is falling back down
        Vector2 rayOrigin = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.center.y);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength);

        if (hit.collider == null)
        {
            return true;
        }

        return false;
    }

    private float GetDistanceToGround()
    {
        float rayLength = 100f;
        Vector2 rayOrigin = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength);

        if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            float distanceToObjectBottom = boxCollider.bounds.min.y - transform.position.y;
            return hit.distance - distanceToObjectBottom;
        }

        return -1;
    }
}
