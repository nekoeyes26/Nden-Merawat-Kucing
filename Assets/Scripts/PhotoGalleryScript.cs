using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoGalleryScript : MonoBehaviour
{
    public GameObject imagePrefab;
    public Transform contentTransform;
    public GameObject noScreenshotText;
    public ScrollRect scrollRect;
    public float scrollDuration = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        WefieController.LoadScreenshots();
        noScreenshotText.SetActive(false);
        if (WefieController.SavedScreenshots.Count > 0)
        {
            DisplayPhotos();
            if (GameManager.instance.previousScene == "Wefie")
            {
                if (!GameManager.instance.isGalleryOpened)
                {
                    StartCoroutine(SmoothScroll());
                    GameManager.instance.isGalleryOpened = true;
                }
                else
                {
                    scrollRect.horizontalNormalizedPosition = 1f;
                }
            }
        }
        else
        {
            noScreenshotText.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DisplayPhotos()
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < WefieController.SavedScreenshots.Count; i++)
        {
            GameObject newImage = Instantiate(imagePrefab, contentTransform);
            Image imageComponent = newImage.GetComponent<Image>();
            Texture2D texture = WefieController.SavedScreenshots[i];
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            imageComponent.sprite = sprite;
        }
    }

    IEnumerator SmoothScroll()
    {
        float elapsedTime = 0f;
        float startValue = scrollRect.horizontalNormalizedPosition;
        float endValue = 1f;

        while (elapsedTime < scrollDuration)
        {
            elapsedTime += Time.deltaTime;
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(startValue, endValue, elapsedTime / scrollDuration);
            yield return null;
        }

        scrollRect.horizontalNormalizedPosition = endValue;
    }
}
