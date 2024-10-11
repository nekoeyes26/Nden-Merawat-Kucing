using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private Cat cat;
    public string currentCatName;
    public string previousCatName;
    public int hungryMiss;
    public int showerMiss;
    public int photoMiss;
    public int playMiss;
    public int totalMiss;
    public bool isHungryTimerOn = false;
    public bool isShowerTimerOn = false;
    public bool isPhotoTimerOn = false;
    public bool isPlayTimerOn = false;
    public string previousScene;
    public bool isGalleryOpened = false;
    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Cat CatProfile
    {
        get { return cat; }
        set
        {
            cat = value;
        }
    }

    public void AddXP()
    {
        cat.catScriptable.xp++;
        GameEvents.XpChanged(cat.catScriptable.xp);
        cat.catScriptable.Save();
    }

    public void LevelUp()
    {
        cat.catScriptable.level++;
        cat.catScriptable.xp = 0;
        cat.RenewRequirement();
        cat.RenewXpNeeded();
        cat.RenewPhase(cat.catScriptable.level);
        GameEvents.LevelChanged(cat.catScriptable.level);
        GameEvents.PhaseChanged(cat.phase);
        cat.catScriptable.Save();
    }

    public void LevelUpChecker()
    {
        if (cat.catScriptable.level < 15)
        {
            if (cat.catScriptable.hungryRemaining <= 0 && cat.catScriptable.showerRemaining <= 0 && cat.catScriptable.playRemaining <= 0 && cat.catScriptable.photoRemaining <= 0)
            {
                LevelUp();
            }
        }
    }
    public void ChangeHungry()
    {
        cat.catScriptable.isHungry = !cat.catScriptable.isHungry;
        GameEvents.HungryChanged();
        cat.catScriptable.Save();
    }

    public void ChangeDirty()
    {
        cat.catScriptable.isDirty = !cat.catScriptable.isDirty;
        GameEvents.DirtyChanged();
        cat.catScriptable.Save();
    }

    public void ChangeSad()
    {
        cat.catScriptable.isSad = !cat.catScriptable.isSad;
        GameEvents.SadChanged();
        cat.catScriptable.Save();
    }

    public void ChangeSick()
    {
        cat.catScriptable.isSick = !cat.catScriptable.isSick;
        GameEvents.SickChanged();
        cat.catScriptable.Save();
    }

    private void OnApplicationQuit()
    {
        if (cat != null)
        {
            cat.catScriptable.Save();
        }
        else
        {
            Debug.Log("No cat selected");
        }
        PlayerPrefs.DeleteAll();
    }

    public void CompleteMissionChecker(ref int remaining)
    {
        if (cat.catScriptable.level < 15)
        {
            if (remaining > 0)
            {
                remaining--;
                AddXP();
                LevelUpChecker();
            }
        }
    }

    public void GiveName(string name)
    {
        cat.catScriptable.name = name;
        cat.catScriptable.Save();
        GameEvents.NameChanged();
    }
}
