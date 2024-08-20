using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatSpriteRendererView : MonoBehaviour
{
    private SpriteRenderer preview;
    // Start is called before the first frame update
    void Start()
    {
        preview = GetComponent<SpriteRenderer>();
        Sprite sprite;
        if (GameManager.instance.CatProfile.catScriptable.phase == CatPhase.Baby)
        {
            sprite = Resources.Load<Sprite>(GameManager.instance.CatProfile.catScriptable.spriteFolderPath + "baby");
        }
        else if (GameManager.instance.CatProfile.catScriptable.phase == CatPhase.Child)
        {
            sprite = Resources.Load<Sprite>(GameManager.instance.CatProfile.catScriptable.spriteFolderPath + "child");
        }
        else if (GameManager.instance.CatProfile.catScriptable.phase == CatPhase.Adult)
        {
            sprite = Resources.Load<Sprite>(GameManager.instance.CatProfile.catScriptable.spriteFolderPath + "adult");
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
