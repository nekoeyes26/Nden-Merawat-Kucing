using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainPageController : MonoBehaviour
{
    public Text catName;
    public Text catLevel;
    // public Text catXP;
    public GameObject hungryUI;
    public GameObject showerUI;
    public GameObject playUI;
    public GameObject photoUI;
    private CatScriptable catS;
    public Image bar;
    private float lerpSpeed = 1f;
    public SpriteRenderer catSprite;
    public Animator catAnimator;
    public Collider2D catCollider;
    private bool isPetted = false;
    private float idlingTimer = 0f;

    void OnEnable()
    {
        GameEvents.OnXpChange += BarFill;
        GameEvents.OnLevelChange += LevelUI;
        GameEvents.OnHungryChange += CatHungry;
        GameEvents.OnDirtyChange += CatDirty;
        GameEvents.OnSadChange += CatSad;
        GameEvents.OnSickChange += CatSick;
        GameEvents.OnMissChange += MissCalc;
    }

    void OnDisable()
    {
        GameEvents.OnXpChange -= BarFill;
        GameEvents.OnLevelChange -= LevelUI;
        GameEvents.OnHungryChange -= CatHungry;
        GameEvents.OnDirtyChange -= CatDirty;
        GameEvents.OnSadChange -= CatSad;
        GameEvents.OnSickChange -= CatSick;
        GameEvents.OnMissChange -= MissCalc;
    }
    void Start()
    {
        catS = GameManager.instance.CatProfile.catScriptable;
        LevelUI(catS.level);
        BarFill(catS.xp);
        CatHungry();
        CatDirty();
        CatSad();
        CatSick();
        MissCalc();
        idlingTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        idlingTimer += Time.deltaTime;
        if (catS.level < 15)
        {
            if (catS.hungryRemaining <= 0) hungryUI.SetActive(false);
            if (catS.showerRemaining <= 0) showerUI.SetActive(false);
            if (catS.playRemaining <= 0) playUI.SetActive(false);
            if (catS.photoRemaining <= 0) photoUI.SetActive(false);
        }
        else if (catS.level >= 15)
        {
            hungryUI.SetActive(true);
            showerUI.SetActive(true);
            playUI.SetActive(true);
            photoUI.SetActive(true);
        }
        if (IsTapped())
        {
            isPetted = !isPetted;
            catAnimator.SetBool("isPet", isPetted);
            idlingTimer = 0f;
            StartCoroutine(ResetIsPetted());
        }

        if (idlingTimer >= 5f)
        {
            catAnimator.SetBool("isIdling", true);
            idlingTimer = 0f;
            StartCoroutine(ResetIdling());
        }
    }

    // void BarFill(int xp)
    // {
    //     // bar.fillAmount = Mathf.Lerp(bar.fillAmount, xp / (float)catS.xpNeeded, lerpSpeed);
    // }

    void BarFill(int xp)
    {
        float targetFillAmount = xp / (float)catS.xpNeeded;
        // Debug.Log(xp);
        StartCoroutine(FillBarSmoothly(targetFillAmount, lerpSpeed));
    }

    IEnumerator FillBarSmoothly(float targetFillAmount, float duration)
    {
        float elapsedTime = 0;
        float startFillAmount = bar.fillAmount;

        while (elapsedTime < duration)
        {
            bar.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bar.fillAmount = targetFillAmount;
    }

    void LevelUI(int level)
    {
        catName.text = catS.name;
        catLevel.text = level.ToString();
        Sprite sprite;
        if (catS.phase == CatPhase.Baby)
        {
            sprite = Resources.Load<Sprite>(catS.spriteFolderPath + "baby");
        }
        else if (catS.phase == CatPhase.Child)
        {
            sprite = Resources.Load<Sprite>(catS.spriteFolderPath + "child");
        }
        else if (catS.phase == CatPhase.Adult)
        {
            sprite = Resources.Load<Sprite>(catS.spriteFolderPath + "adult");
        }
        else
        {
            sprite = catSprite.sprite;
        }
        catSprite.sprite = sprite;
    }

    private bool IsTapped()
    {
        Vector2 inputPosition;

        // For computer (mouse click)
        if (Input.GetMouseButtonDown(0))
        {
            inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        // For mobile (touch input)
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            inputPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        else
        {
            return false;
        }

        RaycastHit2D hit = Physics2D.Raycast(inputPosition, Vector2.zero);
        if (hit.collider != null)
        {
            Debug.Log($"Hit: {hit.collider.gameObject.name}");
            if (hit.collider == catCollider)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator ResetIsPetted()
    {
        yield return new WaitForSeconds(2f);
        isPetted = false;
        catAnimator.SetBool("isPet", isPetted);
    }

    private void CatHungry()
    {
        catAnimator.SetBool("isHungry", catS.isHungry);
    }
    private void CatDirty()
    {
        catAnimator.SetBool("isDirty", catS.isDirty);
    }
    private void CatSad()
    {
        catAnimator.SetBool("isSad", catS.isSad);
    }
    private void CatSick()
    {
        catAnimator.SetBool("isSick", catS.isSick);
    }

    private void MissCalc()
    {
        GameManager.instance.totalMiss = GameManager.instance.hungryMiss + GameManager.instance.showerMiss + GameManager.instance.photoMiss + GameManager.instance.playMiss;
        if (GameManager.instance.totalMiss >= 5 && !catS.isSick)
        {
            GameManager.instance.ChangeSick();
        }
        else if (GameManager.instance.totalMiss < 5 && (!catS.isHungry || !catS.isDirty || !catS.isSad) && catS.isSick)
        {
            GameManager.instance.ChangeSick();
        }
    }

    private IEnumerator ResetIdling()
    {
        yield return new WaitForSeconds(2f);
        catAnimator.SetBool("isIdling", false);
    }
}
