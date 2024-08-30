using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PhotoTimer : Timer
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

        if (!GameManager.instance.isPhotoTimerOn && time > 0)
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
            GameManager.instance.photoMiss++;
            GameEvents.MissChanged();
        }

        if (GameManager.instance.photoMiss >= 2 && !catS.isSad)
        {
            GameManager.instance.ChangeSad();
        }
    }
    public void Reset()
    {
        base.Reset();
        GameManager.instance.photoMiss = 0;
        // GameEvents.MissChanged();
    }

    public void ActivateUI()
    {
        base.ActivateUI();
        GameManager.instance.isPhotoTimerOn = true;
    }

    public void DeactivateUI()
    {
        base.DeactivateUI();
        GameManager.instance.isPhotoTimerOn = false;
    }
}
