using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainPageController : MonoBehaviour
{
    public Text catName;
    public Text catLevel;
    public Text catXP;
    public GameObject hungryUI;
    public GameObject showerUI;
    public GameObject playUI;
    public GameObject photoUI;
    private CatScriptable catS;
    public Image bar;
    private float lerpSpeed = 1f;
    public SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        GameEvents.OnXpChange += BarFill;
        GameEvents.OnLevelChange += LevelUI;
    }

    void OnDisable()
    {
        GameEvents.OnXpChange -= BarFill;
        GameEvents.OnLevelChange -= LevelUI;
    }
    void Start()
    {
        catS = GameManager.instance.CatProfile.catScriptable;
        LevelUI(catS.level);
        BarFill(catS.xp);
    }

    // Update is called once per frame
    void Update()
    {
        if (catS.hungryRemaining <= 0) hungryUI.SetActive(false);
        if (catS.showerRemaining <= 0) showerUI.SetActive(false);
        if (catS.playRemaining <= 0) playUI.SetActive(false);
        if (catS.photoRemaining <= 0) photoUI.SetActive(false);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.AddXP();
        }
    }

    // void BarFill(int xp)
    // {
    //     // bar.fillAmount = Mathf.Lerp(bar.fillAmount, xp / (float)catS.xpNeeded, lerpSpeed);
    // }

    void BarFill(int xp)
    {
        float targetFillAmount = xp / (float)catS.xpNeeded;
        Debug.Log(xp);
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
            sprite = spriteRenderer.sprite;
        }
        spriteRenderer.sprite = sprite;
    }
}
