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
}
