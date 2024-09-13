using System.Collections;
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

    protected float timer = 0f;
    protected bool isUIActive;
    protected float time;
    public Image fill;
    public float maxFillAmount = 1f;
    public static bool firstTime = true;
    protected CatScriptable catS;

    void Awake()
    {
        uiObject.SetActive(false);
        isUIActive = false;
        catS = GameManager.instance.CatProfile.catScriptable;
    }

    void Update()
    {
        // timer += Time.deltaTime;
        // // Debug.Log(time);
        // if (!isUIActive && timer >= cooldownTime)
        // {
        //     ActivateUI();
        //     time = activeTime;
        // }

        // if (isUIActive && timer < cooldownTime + activeTime)
        // {
        //     time -= Time.deltaTime;
        //     fill.fillAmount = (time / activeTime) * maxFillAmount;

        //     if (time < 0)
        //     {
        //         time = 0;
        //     }
        // }
        // else if (isUIActive && timer >= cooldownTime + activeTime)
        // {
        //     DeactivateUI();
        //     string gameObjectName = gameObject.name.ToLower();
        //     if (gameObjectName.Contains("hungry"))
        //     {
        //         GameManager.instance.hungryMiss++;
        //     }
        //     else if (gameObjectName.Contains("shower"))
        //     {
        //         GameManager.instance.showerMiss++;
        //     }
        //     else if (gameObjectName.Contains("photo"))
        //     {
        //         GameManager.instance.photoMiss++;
        //     }
        //     else if (gameObjectName.Contains("play"))
        //     {
        //         GameManager.instance.playMiss++;
        //     }
        // }
        // GameManager.instance.totalMiss = GameManager.instance.hungryMiss + GameManager.instance.showerMiss + GameManager.instance.photoMiss + GameManager.instance.playMiss;
        // Debug.Log(GameManager.instance.totalMiss);
        // if (GameManager.instance.hungryMiss >= 2 && !catS.isHungry)
        // {
        //     GameManager.instance.ChangeHungry();
        // }
        // if (GameManager.instance.showerMiss >= 2 && !catS.isDirty)
        // {
        //     GameManager.instance.ChangeDirty();
        // }
        // if (GameManager.instance.playMiss >= 2 && !catS.isSad)
        // {
        //     GameManager.instance.ChangeSad();
        // }
        // if (GameManager.instance.totalMiss >= 5 && !catS.isSick)
        // {
        //     GameManager.instance.ChangeSick();
        // }
        // else if (GameManager.instance.totalMiss <= 0 && !catS.isHungry && !catS.isDirty && !catS.isSad && catS.isSick)
        // {
        //     GameManager.instance.ChangeSick();
        // }
    }

    public void ActivateUI()
    {
        uiObject.SetActive(true);
        isUIActive = true;
        timer = cooldownTime;
    }

    public void DeactivateUI()
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
        // Debug.Log("Saving");
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

    protected void OnDisable()
    {
        SaveState();
    }

    protected void OnEnable()
    {
        if (GameManager.instance.currentCatName != GameManager.instance.previousCatName && GameManager.instance.previousScene == "ChoosingCat")
        {
            Reset();
            DeactivateUI();
            GameManager.instance.isHungryTimerOn = false;
            GameManager.instance.isShowerTimerOn = false;
            GameManager.instance.isPhotoTimerOn = false;
            GameManager.instance.isPlayTimerOn = false;
            GameManager.instance.hungryMiss = 0;
            GameManager.instance.showerMiss = 0;
            GameManager.instance.photoMiss = 0;
            GameManager.instance.playMiss = 0;
            GameManager.instance.totalMiss = 0;
        }
        else
        {
            if (File.Exists(Application.persistentDataPath + "/" + gameObject.name + "/timer_state.json"))
            {
                LoadState();
            }
        }
        if (firstTime)
        {
            if (File.Exists(Application.persistentDataPath + "/" + gameObject.name + "/timer_state.json"))
            {
                File.Delete(Application.persistentDataPath + "/" + gameObject.name + "/timer_state.json");
            }
            StartCoroutine(ChangeFirstTime());
        }
    }

    public void Reset()
    {
        timer = 0f;
        isUIActive = false;
        time = 0;
        // string gameObjectName = gameObject.name.ToLower();
        // if (gameObjectName.Contains("hungry"))
        // {
        //     GameManager.instance.hungryMiss = 0;
        // }
        // else if (gameObjectName.Contains("shower"))
        // {
        //     GameManager.instance.showerMiss = 0;
        // }
        // else if (gameObjectName.Contains("photo"))
        // {
        //     GameManager.instance.photoMiss = 0;
        // }
        // else if (gameObjectName.Contains("play"))
        // {
        //     GameManager.instance.playMiss = 0;
        // }
    }

    private IEnumerator ChangeFirstTime()
    {
        yield return new WaitForSeconds(1f);
        firstTime = false;
    }

    protected Color GetFillColor(float fillAmount)
    {
        Color color;
        Color customGreen = new Color(0.2f, 0.8f, 0.2f); // a darker green
        Color customYellow = new Color(0.9f, 0.7f, 0.1f); // a more vibrant yellow
        Color customRed = new Color(0.8f, 0.1f, 0.1f); // a deeper red
        if (fillAmount >= 0.8f)
        {
            // Green
            color = customGreen;
        }
        else if (fillAmount >= 0.5f)
        {
            // Green to yellow
            color = Color.Lerp(customGreen, customYellow, 1 - (fillAmount - 0.5f) / 0.3f);
        }
        else if (fillAmount >= 0.2f)
        {
            // Yellow to red
            color = Color.Lerp(customYellow, customRed, 1 - (fillAmount - 0.2f) / 0.3f);
        }
        else if (fillAmount >= 0.0f)
        {
            // Red
            color = customRed;
        }
        else
        {
            color = Color.black;
        }

        return color;
    }
}
