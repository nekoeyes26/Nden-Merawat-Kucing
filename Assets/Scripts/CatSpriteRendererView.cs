using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatSpriteRendererView : MonoBehaviour
{
    private SpriteRenderer preview;
    private BoxCollider2D boxCollider;
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
        // Debug.Log(sprite.ToString());
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider2D is not attached to the GameObject.");
            return;
        }
        UpdateColliderSize();
    }

    void UpdateColliderSize()
    {
        Bounds spriteBounds = preview.bounds;
        boxCollider.size = new Vector2(spriteBounds.size.x, spriteBounds.size.y);
    }
}
