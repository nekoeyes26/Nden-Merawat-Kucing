using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatSpritePreview : MonoBehaviour
{
    private Cat catScript;
    private Image preview;
    // Start is called before the first frame update
    void Start()
    {
        catScript = GetComponent<Cat>();
        preview = GetComponent<Image>();
        Sprite sprite;
        if (catScript.catScriptable.phase == CatPhase.Baby)
        {
            sprite = Resources.Load<Sprite>(catScript.catScriptable.spriteFolderPath + "baby");
        }
        else if (catScript.catScriptable.phase == CatPhase.Child)
        {
            sprite = Resources.Load<Sprite>(catScript.catScriptable.spriteFolderPath + "child");
        }
        else if (catScript.catScriptable.phase == CatPhase.Adult)
        {
            sprite = Resources.Load<Sprite>(catScript.catScriptable.spriteFolderPath + "adult");
        }
        else
        {
            sprite = preview.sprite;
        }
        preview.sprite = sprite;
        Debug.Log(sprite.ToString());
    }

    // Update is called once per frame
    void Update()
    {
    }
}
