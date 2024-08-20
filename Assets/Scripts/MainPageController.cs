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
    private CatScriptable cat;
    public Image bar;
    private float lerpSpeed = 1f;
    public SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        GameEvents.OnXpChange += BarFill;
    }

    void OnDisable()
    {
        GameEvents.OnXpChange -= BarFill;
    }
    void Start()
    {
        cat = GameManager.instance.CatProfile.catScriptable;
        catName.text = cat.name;
        catLevel.text = cat.level.ToString();
        BarFill(cat.xp);
        Sprite sprite;
        if (cat.phase == CatPhase.Baby)
        {
            sprite = Resources.Load<Sprite>(cat.spriteFolderPath + "baby");
        }
        else if (cat.phase == CatPhase.Child)
        {
            sprite = Resources.Load<Sprite>(cat.spriteFolderPath + "child");
        }
        else if (cat.phase == CatPhase.Adult)
        {
            sprite = Resources.Load<Sprite>(cat.spriteFolderPath + "adult");
        }
        else
        {
            sprite = spriteRenderer.sprite;
        }
        spriteRenderer.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {

        if (cat.hungryRemaining <= 0) hungryUI.SetActive(false);
        if (cat.showerRemaining <= 0) showerUI.SetActive(false);
        if (cat.playRemaining <= 0) playUI.SetActive(false);
        if (cat.photoRemaining <= 0) photoUI.SetActive(false);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.AddXP();
        }
    }

    // void BarFill(int xp)
    // {
    //     // bar.fillAmount = Mathf.Lerp(bar.fillAmount, xp / (float)cat.xpNeeded, lerpSpeed);
    // }

    void BarFill(int xp)
    {
        float targetFillAmount = xp / (float)cat.xpNeeded;
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
}
