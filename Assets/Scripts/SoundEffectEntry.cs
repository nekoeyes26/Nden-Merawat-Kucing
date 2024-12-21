using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundEffect
{
    Select,
    Swipe,
    Back,
    Continue,
    PopUpOpen,
    PopCloseClose
    // Add more sound effects as needed
}

[System.Serializable]
public class SoundEffectEntry
{
    public SoundEffect soundEffect; // The enum value
    public AudioClip audioClip;      // The corresponding audio clip
}
