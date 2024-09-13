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
    public RectTransform[] checkpointImage;
    public VerticalLayoutGroup CheckpointVerticalLayout;
    public RectTransform barTransform;
    public RectTransform bathubTransform;
    public Image checklist1st;
    public Image checklist2nd;
    public Image checklist3rd;
    private float timeInstructionAppear = 3f;
    private float timeUntilInstruction;
    public GameObject instruction1;
    public GameObject instruction2;
    public GameObject instruction3;
    public GameObject instruction4;

    // Start is called before the first frame update
    void Start()
    {
        isWet = false;
        isSoapy = false;
        isShowered = false;
        isDried = false;
        bar.fillAmount = 0;
        UIScalingAspect();
        checklist1st.enabled = false;
        checklist2nd.enabled = false;
        checklist3rd.enabled = false;
        instruction1.SetActive(false);
        instruction2.SetActive(false);
        instruction3.SetActive(false);
        instruction4.SetActive(false);
    }

    private void Update()
    {
        lerpSpeed = 2f * Time.deltaTime;
        ProgressBar();
        if (isWet && !isXPAdded)
        {
            exitButton.interactable = false;
        }
        else if (isXPAdded)
        {
            exitButton.interactable = true;
        }
        if (!(instruction1.activeSelf || instruction2.activeSelf || instruction3.activeSelf || instruction4.activeSelf))
        {
            timeUntilInstruction += Time.deltaTime;
            if (timeUntilInstruction >= timeInstructionAppear)
            {
                InstructionNotification();
                timeUntilInstruction = 0;
            }
        }
    }

    public void ProgressBar()
    {
        if (isWet && !isSoapy && !isShowered && !isDried)
        {
            bar.fillAmount = Mathf.Lerp(bar.fillAmount, (float)0.25, lerpSpeed);
            catAnimator.SetBool("isWet", true);
            instruction1.SetActive(false);
            StartCoroutine(EnableChecklist(checklist1st));
        }

        if (isWet && isSoapy && !isShowered && !isDried)
        {
            bar.fillAmount = Mathf.Lerp(bar.fillAmount, (float)0.5, lerpSpeed);
            instruction2.SetActive(false);
            StartCoroutine(EnableChecklist(checklist2nd));
        }

        if (isWet && isSoapy && isShowered && !isDried)
        {
            bar.fillAmount = Mathf.Lerp(bar.fillAmount, (float)0.75, lerpSpeed);
            instruction3.SetActive(false);
            StartCoroutine(EnableChecklist(checklist3rd));
        }

        if (isWet && isSoapy && isShowered && isDried)
        {
            bar.fillAmount = Mathf.Lerp(bar.fillAmount, 1, lerpSpeed);
            instruction4.SetActive(false);
            if (!isXPAdded)
            {
                if (GameManager.instance.CatProfile.catScriptable.showerRemaining > 0)
                {
                    GameManager.instance.CatProfile.catScriptable.showerRemaining--;
                    GameManager.instance.AddXP();
                    GameManager.instance.LevelUpChecker();
                }
                isXPAdded = true;
                if (GameManager.instance.CatProfile.catScriptable.isDirty) GameManager.instance.ChangeDirty();
                GameManager.instance.isShowerTimerOn = false;
                catAnimator.SetBool("isWet", false);
            }
        }
    }

    public void UIScalingAspect()
    {
        float screenAspect = (float)Screen.width / Screen.height;

        // Check if the screen aspect ratio is close to 4:3
        if (Mathf.Abs(screenAspect - 4f / 3f) < 0.01f)
        {
            Debug.Log("Screen size 4:3");
            // Set the desired width and height
            float width = 150f; // Replace with your desired width
            float height = 150f; // Replace with your desired height

            // Change the Rect Transform size
            foreach (RectTransform check in checkpointImage)
            {
                check.sizeDelta = new Vector2(width, height);
            }

            CheckpointVerticalLayout.padding.top = -141;
            CheckpointVerticalLayout.padding.bottom = 129;

            float topAnchor = 27.1f;
            float bottomAnchor = 32f;
            barTransform.offsetMax = new Vector2(barTransform.offsetMax.x, -topAnchor);
            barTransform.offsetMin = new Vector2(barTransform.offsetMin.x, bottomAnchor);

            float posX = -547f;
            bathubTransform.anchoredPosition = new Vector2(posX, bathubTransform.anchoredPosition.y);
        }
        // Check if the screen aspect ratio is close to 5:4
        else if (Mathf.Abs(screenAspect - 5f / 4f) < 0.01f)
        {
            float width = 170f; // Replace with your desired width
            float height = 170f; // Replace with your desired height

            // Change the Rect Transform size
            foreach (RectTransform check in checkpointImage)
            {
                check.sizeDelta = new Vector2(width, height);
            }

            CheckpointVerticalLayout.padding.top = -141;
            CheckpointVerticalLayout.padding.bottom = 129;

            float topAnchor = 12.4f;
            float bottomAnchor = 2.4f;
            barTransform.offsetMax = new Vector2(barTransform.offsetMax.x, -topAnchor);
            barTransform.offsetMin = new Vector2(barTransform.offsetMin.x, bottomAnchor);

            float posX = -547f;
            bathubTransform.anchoredPosition = new Vector2(posX, bathubTransform.anchoredPosition.y);
        }
        else
        {
            float width = 80f; // Replace with your desired width
            float height = 80f; // Replace with your desired height

            // Change the Rect Transform size
            foreach (RectTransform check in checkpointImage)
            {
                check.sizeDelta = new Vector2(width, height);
            }

            CheckpointVerticalLayout.padding.top = -90;
            CheckpointVerticalLayout.padding.bottom = 47;

            float topAnchor = 14f;
            float bottomAnchor = 15.2f;
            barTransform.offsetMax = new Vector2(barTransform.offsetMax.x, -topAnchor);
            barTransform.offsetMin = new Vector2(barTransform.offsetMin.x, bottomAnchor);

            float posX = -187f;
            bathubTransform.anchoredPosition = new Vector2(posX, bathubTransform.anchoredPosition.y);
        }
    }

    IEnumerator EnableChecklist(Image checklist)
    {
        yield return new WaitForSeconds(0.6f);
        checklist.enabled = true;
    }

    private void InstructionNotification()
    {
        if (!isWet)
        {
            instruction1.SetActive(true);
        }
        else if (!isSoapy)
        {
            instruction2.SetActive(true);
        }
        else if (!isShowered)
        {
            instruction3.SetActive(true);
        }
        else if (!isDried)
        {
            instruction4.SetActive(true);
        }
    }
}
