using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CatState
{
    Unnamed,
    Named
}

public enum CatPhase
{
    Baby,
    Child,
    Adult
}
[CreateAssetMenu(fileName = "New Cat", menuName = "Cat")]
public class CatScriptable : ScriptableObject
{
    public Sprite preview;
    [SerializeField] private string catName;
    public int xp;
    public int xpNeeded;
    public int level;
    public CatPhase phase;
    public int hungryRemaining;
    public int showerRemaining;
    public int playRemaining;
    public int photoRemaining;
    public CatState state;
    public string spriteFolderPath;
    public string animationFolderPath;

    public void Awake()
    {
        state = CatState.Unnamed;
        phase = CatPhase.Baby;
    }

    public new string name
    {
        get { return catName; }
        set
        {
            state = CatState.Named;
            catName = value;
        }
    }
}
