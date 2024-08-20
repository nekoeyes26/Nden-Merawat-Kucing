using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public CatScriptable catScriptable;
    [SerializeField] private new string name;
    [SerializeField] private int xp;
    [SerializeField] private int xpNeeded;
    [SerializeField] private int level;
    [SerializeField] public CatPhase phase;
    [SerializeField] private int hungryRemaining;
    [SerializeField] private int showerRemaining;
    [SerializeField] private int playRemaining;
    [SerializeField] private int photoRemaining;
    [SerializeField] private CatState state;


    void Start()
    {
        RenewRemaining(0);
        RenewXpNeeded();
        name = catScriptable.name;
        xp = catScriptable.xp;
        xpNeeded = catScriptable.xpNeeded;
        level = catScriptable.level;
        phase = catScriptable.phase;
        hungryRemaining = catScriptable.hungryRemaining;
        showerRemaining = catScriptable.showerRemaining;
        playRemaining = catScriptable.playRemaining;
        photoRemaining = catScriptable.photoRemaining;
        state = catScriptable.state;
    }

    public void RenewRemaining(int level)
    {
        if (level <= 0)
        {
            catScriptable.hungryRemaining = 2;
            catScriptable.showerRemaining = 3;
            catScriptable.playRemaining = 3;
            catScriptable.photoRemaining = 1;
        }
        else if (level == 1)
        {
            catScriptable.hungryRemaining = 3;
            catScriptable.showerRemaining = 3;
            catScriptable.playRemaining = 3;
            catScriptable.photoRemaining = 1;
        }
        else if (level == 2)
        {
            catScriptable.hungryRemaining = 4;
            catScriptable.showerRemaining = 4;
            catScriptable.playRemaining = 4;
            catScriptable.photoRemaining = 1;
        }
        else if (level == 3)
        {
            catScriptable.hungryRemaining = 4;
            catScriptable.showerRemaining = 4;
            catScriptable.playRemaining = 4;
            catScriptable.photoRemaining = 2;
        }
        else if (level == 4)
        {
            catScriptable.hungryRemaining = 5;
            catScriptable.showerRemaining = 5;
            catScriptable.playRemaining = 5;
            catScriptable.photoRemaining = 2;
        }
    }
    public void RenewXpNeeded()
    {
        catScriptable.xpNeeded = catScriptable.hungryRemaining + catScriptable.showerRemaining + catScriptable.playRemaining + catScriptable.photoRemaining;
    }


}
