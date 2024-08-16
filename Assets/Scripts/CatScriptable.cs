using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cat", menuName = "Cat")]
public class CatScriptable : ScriptableObject
{
    public new string name;
    public int xp;
    public int level;
    public int phase;
    public int hungryRemaining;
    public int showerRemaining;
    public int playRemaining;
    public int photoRemaining;
    public string animationFolderPath;
}
