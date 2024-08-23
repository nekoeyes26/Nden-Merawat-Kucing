using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private Cat cat;
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
        set { cat = value; }
    }

    public void AddXP()
    {
        cat.catScriptable.xp++;
        GameEvents.XpChanged(cat.catScriptable.xp);
    }

    public void LevelUp()
    {
        cat.catScriptable.level++;
        cat.catScriptable.xp = 0;
        cat.RenewRequirement();
        cat.RenewXpNeeded();
        cat.RenewPhase(cat.catScriptable.level);
        GameEvents.LevelChanged(cat.catScriptable.level);
    }

    public void LevelUpChecker()
    {
        if (cat.catScriptable.hungryRemaining <= 0 && cat.catScriptable.showerRemaining <= 0 && cat.catScriptable.playRemaining <= 0 && cat.catScriptable.photoRemaining <= 0)
        {
            LevelUp();
        }
    }
}
