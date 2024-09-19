using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        isFull = false;
        completePopUp.SetActive(false);
    }

    private void Update()
    {
        lerpSpeed = 2f * Time.deltaTime;
        BarFill();
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
        }
        else
        {
            Debug.Log("The object is not tagged as Food. Point not added.");
            catAnimator.SetBool("isSteady", false);
            catAnimator.SetBool("isEating", false);
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
            Invoke("ShowPopUp", 1f);
        }
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
        Time.timeScale = 0f;
    }

    public void ClosePopUp()
    {
        completePopUp.SetActive(false);
        Time.timeScale = 1f;
    }
}
