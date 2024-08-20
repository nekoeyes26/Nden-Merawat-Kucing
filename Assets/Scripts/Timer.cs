using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Timer : MonoBehaviour
{
    public GameObject uiObject;
    public float cooldownTime = 60f;
    public float activeTime = 180f;

    private float timer = 0f;
    private bool isUIActive = false;
    private float time;
    public Image fill;
    public float maxFillAmount = 1f;

    void Start()
    {
        uiObject.SetActive(false);
    }

    void Update()
    {
        timer += Time.deltaTime;
        // Debug.Log(time);
        if (!isUIActive && timer >= cooldownTime)
        {
            ActivateUI();
            time = activeTime;
        }

        if (isUIActive && timer < cooldownTime + activeTime)
        {
            time -= Time.deltaTime;
            fill.fillAmount = (time / activeTime) * maxFillAmount;

            if (time < 0)
            {
                time = 0;
            }
        }
        else if (isUIActive && timer >= cooldownTime + activeTime)
        {
            DeactivateUI();
        }
    }

    private void ActivateUI()
    {
        uiObject.SetActive(true);
        isUIActive = true;
        timer = cooldownTime;
    }

    private void DeactivateUI()
    {
        uiObject.SetActive(false);
        isUIActive = false;
        timer = 0f;
    }
}
