using System.Collections;
using System.Collections.Generic;
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

    void Awake()
    {
        catScriptable.Load();
    }

    void Start()
    {
        if (catScriptable.level <= 0)
        {
            catScriptable.level = 1;
            RenewRequirement();
            RenewXpNeeded();
        }
        RenewPhase(catScriptable.level);
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
        // RenewRemaining(0);

    }

    // public void RenewRemaining(int level)
    // {
    //     if (level <= 0)
    //     {
    //         catScriptable.hungryRemaining = 2;
    //         catScriptable.showerRemaining = 3;
    //         catScriptable.playRemaining = 3;
    //         catScriptable.photoRemaining = 1;
    //     }
    //     else if (level == 1)
    //     {
    //         catScriptable.hungryRemaining = 3;
    //         catScriptable.showerRemaining = 3;
    //         catScriptable.playRemaining = 3;
    //         catScriptable.photoRemaining = 1;
    //     }
    //     else if (level == 2)
    //     {
    //         catScriptable.hungryRemaining = 4;
    //         catScriptable.showerRemaining = 4;
    //         catScriptable.playRemaining = 4;
    //         catScriptable.photoRemaining = 1;
    //     }
    //     else if (level == 3)
    //     {
    //         catScriptable.hungryRemaining = 4;
    //         catScriptable.showerRemaining = 4;
    //         catScriptable.playRemaining = 4;
    //         catScriptable.photoRemaining = 2;
    //     }
    //     else if (level == 4)
    //     {
    //         catScriptable.hungryRemaining = 5;
    //         catScriptable.showerRemaining = 5;
    //         catScriptable.playRemaining = 5;
    //         catScriptable.photoRemaining = 2;
    //     }
    // }
    public void RenewXpNeeded()
    {
        catScriptable.xpNeeded = catScriptable.hungryRemaining + catScriptable.showerRemaining + catScriptable.playRemaining + catScriptable.photoRemaining;
    }

    public void RenewRequirement()
    {
        int currentLevel = catScriptable.level;
        if (currentLevel <= 0) currentLevel = 1;
        RequirementScriptable requirementScriptable;
        requirementScriptable = Resources.Load<RequirementScriptable>("RequirementScriptable/" + currentLevel.ToString());
        if (requirementScriptable != null)
        {
            catScriptable.hungryRemaining = requirementScriptable.hungry;
            catScriptable.showerRemaining = requirementScriptable.shower;
            catScriptable.playRemaining = requirementScriptable.play;
            catScriptable.photoRemaining = requirementScriptable.photo;
        }
        else
        {
            catScriptable.hungryRemaining = 1;
            catScriptable.showerRemaining = 1;
            catScriptable.playRemaining = 1;
            catScriptable.photoRemaining = 1;
        }
    }

    public void RenewPhase(int currentLevel)
    {
        if (currentLevel == 6)
        {
            catScriptable.phase = CatPhase.Child;
        }
        else if (currentLevel == 11)
        {
            catScriptable.phase = CatPhase.Adult;
        }
    }

}
