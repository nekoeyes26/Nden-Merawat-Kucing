using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HungryTimer : Timer
{
    void OnEnable()
    {
        base.OnEnable();
        if (isUIActive)
        {
            uiObject.SetActive(true);
        }
        else if (!isUIActive)
        {
            uiObject.SetActive(false);
        }

        if (!GameManager.instance.isHungryTimerOn && time > 0)
        {
            Reset();
            DeactivateUI();
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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
            GameManager.instance.hungryMiss++;
            GameEvents.MissChanged();
        }

        if (GameManager.instance.hungryMiss >= 2 && !catS.isHungry)
        {
            GameManager.instance.ChangeHungry();
        }
    }
    public void Reset()
    {
        base.Reset();
        GameManager.instance.hungryMiss = 0;
        // GameEvents.MissChanged();
    }

    public void ActivateUI()
    {
        base.ActivateUI();
        GameManager.instance.isHungryTimerOn = true;
    }

    public void DeactivateUI()
    {
        base.DeactivateUI();
        GameManager.instance.isHungryTimerOn = false;
    }
}