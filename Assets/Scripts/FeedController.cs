using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FeedController : MonoBehaviour
{
    public int targetPoint = 5;
    private int point;
    public Image bar;
    private float lerpSpeed;
    private bool isFull = false;
    private bool isXPAdded = false;
    public Animator catAnimator;
    public GameObject completePopUp;
    private bool isDragging;
    public bool activityComplete = false;

    void OnEnable()
    {
        GameEvents.OnDraggingFood += HandleOnDraggingFood;
    }

    private void Start()
    {
        isFull = false;
        completePopUp.SetActive(false);
        if (SpineAnimationController.instance.initialized)
        {
            SpineAnimationController.instance.PlayAnimation(SpineAnimationController.instance.normal, true, 1f);
        }
    }

    private void Update()
    {
        lerpSpeed = 2f * Time.deltaTime;
        BarFill();
        if (SpineAnimationController.instance.initialized)
        {
            if (!isDragging && !SpineAnimationController.instance.isMakanPlaying)
            {
                SpineAnimationController.instance.PlayAnimation(SpineAnimationController.instance.normal, true, 1f);
            }
            else if (isDragging)
            {
                SpineAnimationController.instance.isMakanPlaying = false;
                SpineAnimationController.instance.PlayAnimation(SpineAnimationController.instance.suap, true, 1f);
            }
        }
    }

    public void BarFill()
    {
        bar.fillAmount = Mathf.Lerp(bar.fillAmount, (float)point / (float)targetPoint, lerpSpeed);
    }
    public void AddPoint(GameObject feedingObject)
    {
        if (feedingObject.CompareTag("Food"))
        {
            if (!isFull)
            {
                point++;
                Debug.Log(point);
            }
            if (point >= targetPoint)
            {
                Full();
                Debug.Log(point);
            }
            catAnimator.SetBool("isSteady", false);
            catAnimator.SetBool("isEating", true);
            StartCoroutine(ResetEatingState());
            if (SpineAnimationController.instance.initialized)
            {
                SpineAnimationController.instance.PlayAnimation(SpineAnimationController.instance.makan, false, 1f);
            }
        }
        else
        {
            Debug.Log("The object is not tagged as Food. Point not added.");
            catAnimator.SetBool("isSteady", false);
            catAnimator.SetBool("isEating", false);
            if (SpineAnimationController.instance.initialized)
            {
                SpineAnimationController.instance.PlayAnimation(SpineAnimationController.instance.salah, false, 1f);
            }
        }
    }

    public void Full()
    {
        isFull = true;
        Debug.Log("Full");
        if (!isXPAdded)
        {
            // if (GameManager.instance.CatProfile.catScriptable.hungryRemaining > 0)
            // {
            //     GameManager.instance.CatProfile.catScriptable.hungryRemaining--;
            //     GameManager.instance.AddXP();
            //     GameManager.instance.LevelUpChecker();
            // }
            GameManager.instance.CompleteMissionChecker(ref GameManager.instance.CatProfile.catScriptable.hungryRemaining);
            isXPAdded = true;
            if (GameManager.instance.CatProfile.catScriptable.isHungry) GameManager.instance.ChangeHungry();
            GameManager.instance.isHungryTimerOn = false;
            activityComplete = true;
            Invoke("ShowPopUp", 1f);
            //DisableFoodObject();
            //DisableFoodCollider();
            DisableFoodInteractable();
        }
    }

    private void HandleOnDraggingFood(bool dragging)
    {
        isDragging = dragging;
    }

    public void CatSteady()
    {
        catAnimator.SetBool("isSteady", true);
    }

    private IEnumerator ResetEatingState()
    {
        yield return new WaitForSeconds(2f);
        catAnimator.SetBool("isEating", false);
    }

    public void CatBackIdle()
    {
        catAnimator.SetBool("isSteady", false);
    }

    public void ShowPopUp()
    {
        completePopUp.SetActive(true);
        //Time.timeScale = 0f;
    }

    public void ClosePopUp()
    {
        completePopUp.SetActive(false);
        Time.timeScale = 1f;
    }

    private void DisableFoodObject()
    {
        FoodObject[] foodObjects = FindObjectsOfType<FoodObject>();

        foreach (FoodObject obj in foodObjects)
        {
            obj.enabled = false;
        }
    }

    private void DisableFoodCollider()
    {
        PolygonCollider2D[] polygonCollider = FindObjectsOfType<PolygonCollider2D>();

        foreach (PolygonCollider2D collider in polygonCollider)
        {
            collider.enabled = false;
        }
    }

    private void DisableFoodInteractable()
    {
        FoodObject[] foodObjects = FindObjectsOfType<FoodObject>();

        foreach (FoodObject obj in foodObjects)
        {
            obj.interactable = false;
        }
    }
}
