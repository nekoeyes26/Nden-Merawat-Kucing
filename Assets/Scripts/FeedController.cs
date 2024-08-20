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

    private void Start()
    {
        isFull = false;
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
        }
        else
        {
            Debug.Log("The object is not tagged as Food. Point not added.");
        }
    }

    public void Full()
    {
        isFull = true;
        Debug.Log("Full");
        if (!isXPAdded)
        {
            GameManager.instance.AddXP();
            GameManager.instance.CatProfile.catScriptable.hungryRemaining--;
            isXPAdded = true;
        }
    }
}
