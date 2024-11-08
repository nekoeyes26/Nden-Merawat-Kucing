﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Timer : MonoBehaviour
{
    public GameObject uiObject;
    public float cooldownTime = 60f;
    public float activeTime = 180f;

    private float timer = 0f;
    private bool isUIActive;
    private float time;
    public Image fill;
    public float maxFillAmount = 1f;
    public static bool firstTime = true;
    private CatScriptable catS;

    void Awake()
    {
        uiObject.SetActive(false);
        isUIActive = false;
        catS = GameManager.instance.CatProfile.catScriptable;
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
            string gameObjectName = gameObject.name.ToLower();
            if (gameObjectName.Contains("hungry"))
            {
                GameManager.instance.hungryMiss++;
            }
            else if (gameObjectName.Contains("shower"))
            {
                GameManager.instance.showerMiss++;
            }
            else if (gameObjectName.Contains("photo"))
            {
                GameManager.instance.photoMiss++;
            }
            else if (gameObjectName.Contains("play"))
            {
                GameManager.instance.playMiss++;
            }
        }
        GameManager.instance.totalMiss = GameManager.instance.hungryMiss + GameManager.instance.showerMiss + GameManager.instance.photoMiss + GameManager.instance.playMiss;
        Debug.Log(GameManager.instance.totalMiss);
        if (GameManager.instance.hungryMiss >= 2 && !catS.isHungry)
        {
            GameManager.instance.ChangeHungry();
        }
        if (GameManager.instance.showerMiss >= 2 && !catS.isDirty)
        {
            GameManager.instance.ChangeDirty();
        }
        if (GameManager.instance.playMiss >= 2 && !catS.isSad)
        {
            GameManager.instance.ChangeSad();
        }
        if (GameManager.instance.totalMiss >= 5 && !catS.isSick)
        {
            GameManager.instance.ChangeSick();
        }
        else if (GameManager.instance.totalMiss <= 0 && !catS.isHungry && !catS.isDirty && !catS.isSad && catS.isSick)
        {
            GameManager.instance.ChangeSick();
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

    [System.Serializable]
    public class TimerData
    {
        public float timer;
        public bool isUIActive;
        public float time;
    }

    // Save the state to a file
    public void SaveState()
    {
        Debug.Log("Saving");
        TimerData data = new TimerData();
        data.timer = timer;
        data.isUIActive = isUIActive;
        data.time = time;

        string json = JsonUtility.ToJson(data);
        string directoryPath = Application.persistentDataPath + "/" + gameObject.name;
        Directory.CreateDirectory(directoryPath);
        string filePath = directoryPath + "/timer_state.json";
        File.WriteAllText(filePath, json);
    }

    // Load the state from a file
    public void LoadState()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/" + gameObject.name + "/timer_state.json");
        TimerData data = JsonUtility.FromJson<TimerData>(json);

        timer = data.timer;
        isUIActive = data.isUIActive;
        time = data.time;
    }

    // Call SaveState() when the scene is unloaded
    void OnDisable()
    {
        SaveState();
    }

    // Call LoadState() when the scene is loaded
    void OnEnable()
    {
        if (firstTime)
        {
            if (File.Exists(Application.persistentDataPath + "/" + gameObject.name + "/timer_state.json"))
            {
                File.Delete(Application.persistentDataPath + "/" + gameObject.name + "/timer_state.json");
            }
            StartCoroutine(ChangeFirstTime());
        }
        if (File.Exists(Application.persistentDataPath + "/" + gameObject.name + "/timer_state.json"))
        {
            LoadState();
        }
        if (isUIActive)
        {
            uiObject.SetActive(true);
        }
        else if (!isUIActive)
        {
            uiObject.SetActive(false);
        }
    }

    public void Reset()
    {
        timer = 0f;
        isUIActive = false;
        time = 0;
        string gameObjectName = gameObject.name.ToLower();
        if (gameObjectName.Contains("hungry"))
        {
            GameManager.instance.hungryMiss = 0;
        }
        else if (gameObjectName.Contains("shower"))
        {
            GameManager.instance.showerMiss = 0;
        }
        else if (gameObjectName.Contains("photo"))
        {
            GameManager.instance.photoMiss = 0;
        }
        else if (gameObjectName.Contains("play"))
        {
            GameManager.instance.playMiss = 0;
        }
    }

    private IEnumerator ChangeFirstTime()
    {
        yield return new WaitForSeconds(1f);
        firstTime = false;
    }
}
