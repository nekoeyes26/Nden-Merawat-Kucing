using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static event Action<int> OnXpChange;
    public static event Action<int> OnLevelChange;

    public static void XpChanged(int xpNow)
    {
        OnXpChange?.Invoke(xpNow);
    }

    public static void LevelChanged(int levelNow)
    {
        OnXpChange?.Invoke(levelNow);
    }
}
