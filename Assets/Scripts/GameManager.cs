using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private CatScriptable cat;
    private int hungryCooldown;
    private int showerCooldown;
    private int playCooldown;
    private int photoCooldown;

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

    public void AssignCat(CatScriptable catScriptable)
    {
        cat = catScriptable;
    }

    public string CatName
    {
        get { return cat.name; }
        set { cat.name = value; }
    }

    public int CatXP
    {
        get { return cat.xp; }
        set { cat.xp = value; }
    }
    public int CatLevel
    {
        get { return cat.level; }
        set { cat.level = value; }
    }
}
