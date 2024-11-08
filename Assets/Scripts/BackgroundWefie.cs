using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BackgroundWefie
{
    public Sprite bgSprite;
    public Vector2 charOnLeftPosition;
    public Vector2 charOnRightPosition;

    public BackgroundWefie(Sprite bgSprite, Vector2 leftPos, Vector2 rightPos)
    {
        this.bgSprite = bgSprite;
        this.charOnLeftPosition = leftPos;
        this.charOnRightPosition = rightPos;
    }
}
