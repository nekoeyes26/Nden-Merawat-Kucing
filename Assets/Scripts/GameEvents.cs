using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static event Action<int> OnXpChange;
    public static event Action<int> OnLevelChange;
    public static event Action OnHungryChange;
    public static event Action OnDirtyChange;
    public static event Action OnSadChange;
    public static event Action OnSickChange;
    public static event Action OnMissChange;
    public static event Action OnNameChange;

    public static void XpChanged(int xpNow)
    {
        OnXpChange?.Invoke(xpNow);
    }

    public static void LevelChanged(int levelNow)
    {
        OnXpChange?.Invoke(levelNow);
    }

    public static void HungryChanged()
    {
        OnHungryChange?.Invoke();
    }

    public static void DirtyChanged()
    {
        OnDirtyChange?.Invoke();
    }

    public static void SadChanged()
    {
        OnSadChange?.Invoke();
    }

    public static void SickChanged()
    {
        OnSickChange?.Invoke();
    }

    public static void MissChanged()
    {
        OnMissChange?.Invoke();
    }

    public static void NameChanged()
    {
        OnNameChange?.Invoke();
    }
}
