using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BathController : MonoBehaviour
{
    private bool isWet;
    public bool IsWet { get { return isWet; } set { isWet = value; } }

    private bool isSoapy;
    public bool IsSoapy { get { return isSoapy; } set { isSoapy = value; } }

    private bool isShowered;
    public bool IsShowered { get { return isShowered; } set { isShowered = value; } }

    private bool isDried;
    public bool IsDried { get { return isDried; } set { isDried = value; } }
    public Image bar;
    private float lerpSpeed;
    private bool isXPAdded = false;
    public Button exitButton;
    public Animator catAnimator;
    // Start is called before the first frame update
    void Start()
    {
        isWet = false;
        isSoapy = false;
        isShowered = false;
        isDried = false;
        bar.fillAmount = 0;
    }

    private void Update()
    {
        lerpSpeed = 2f * Time.deltaTime;
        BarFill();
        if (isWet && !isXPAdded)
        {
            exitButton.interactable = false;
        }
        else if (isXPAdded)
        {
            exitButton.interactable = true;
        }
    }

    public void BarFill()
    {
        if (isWet && !isSoapy && !isShowered && !isDried)
        {
            bar.fillAmount = Mathf.Lerp(bar.fillAmount, (float)0.25, lerpSpeed);
            catAnimator.SetBool("isWet", true);
        }

        if (isWet && isSoapy && !isShowered && !isDried)
        {
            bar.fillAmount = Mathf.Lerp(bar.fillAmount, (float)0.5, lerpSpeed);
        }

        if (isWet && isSoapy && isShowered && !isDried)
        {
            bar.fillAmount = Mathf.Lerp(bar.fillAmount, (float)0.75, lerpSpeed);
        }

        if (isWet && isSoapy && isShowered && isDried)
        {
            bar.fillAmount = Mathf.Lerp(bar.fillAmount, 1, lerpSpeed);
            if (!isXPAdded)
            {
                GameManager.instance.CatProfile.catScriptable.showerRemaining--;
                GameManager.instance.AddXP();
                isXPAdded = true;
                GameManager.instance.LevelUpChecker();
                if (GameManager.instance.CatProfile.catScriptable.isDirty) GameManager.instance.ChangeDirty();
                GameManager.instance.isShowerTimerOn = false;
                catAnimator.SetBool("isWet", false);
            }
        }
    }
}
