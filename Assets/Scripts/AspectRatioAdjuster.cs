using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class AspectRatioAdjuster : MonoBehaviour
{
    public enum AspectRatios
    {
        None,
        Aspect_4_3,
        Aspect_5_3,
        Aspect_5_4,
        Aspect_16_9,
        Aspect_18_9,
        Aspect_20_9,
        Aspect_21_9,
        Aspect_22_9,
        Aspect_21_10
    }

    public AspectRatios selectedAspectRatio;

    public bool changeWidth = false;
    public bool changeHeight = false;
    public bool changeTopOffset = false;
    public bool changeBottomOffset = false;
    public bool changeLeftOffset = false;
    public bool changeRightOffset = false;
    public bool changePosX = false;
    public bool changePosY = false;
    public bool changeScaleX = false;
    public bool changeScaleY = false;
    public bool changeScaleZ = false;

    public float width = 0;
    public float height = 0;
    public float topOffset = 0;
    public float bottomOffset = 0;
    public float leftOffset = 0;
    public float rightOffset = 0;
    public float posX = 0;
    public float posY = 0;
    public float scaleX = 1;
    public float scaleY = 1;
    public float scaleZ = 1;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        AdjustImageForAspectRatio();
    }

    void AdjustImageForAspectRatio()
    {
        // float screenAspect = (float)Screen.width / Screen.height;
        float screenAspect = Camera.main.aspect;
        // Debug.Log("Screen Aspect Ratio: " + screenAspect);

        // Check for selected aspect ratio and apply changes only if it matches the screen's aspect ratio
        switch (selectedAspectRatio)
        {
            case AspectRatios.Aspect_4_3:
                if (Mathf.Abs(screenAspect - 4f / 3f) < 0.012f)
                    ApplyChanges("4:3");
                break;
            case AspectRatios.Aspect_5_3:
                if (Mathf.Abs(screenAspect - 5f / 3f) < 0.012f)
                    ApplyChanges("5:3");
                break;
            case AspectRatios.Aspect_5_4:
                if (Mathf.Abs(screenAspect - 5f / 4f) < 0.012f)
                    ApplyChanges("5:4");
                break;
            case AspectRatios.Aspect_16_9:
                if (Mathf.Abs(screenAspect - 16f / 9f) < 0.012f)
                    ApplyChanges("16:9");
                break;
            case AspectRatios.Aspect_18_9:
                if (Mathf.Abs(screenAspect - 18f / 9f) < 0.012f)
                    ApplyChanges("18:9");
                break;
            case AspectRatios.Aspect_20_9:
                if (Mathf.Abs(screenAspect - 20f / 9f) < 0.012f)
                    ApplyChanges("20:9");
                break;
            case AspectRatios.Aspect_21_9:
                if (Mathf.Abs(screenAspect - 21f / 9f) < 0.012f)
                    ApplyChanges("21:9");
                break;
            case AspectRatios.Aspect_22_9:
                if (Mathf.Abs(screenAspect - 22f / 9f) < 0.012f)
                    ApplyChanges("22:9");
                break;
            case AspectRatios.Aspect_21_10:
                if (Mathf.Abs(screenAspect - 21f / 10f) < 0.012f)
                    ApplyChanges("21:10");
                break;
            default:
                Debug.Log("No aspect ratio selected or does not match screen ratio.");
                break;
        }
    }

    private void ApplyChanges(string aspectRatio)
    {
        Debug.Log("Aspect ratio: " + aspectRatio);
        if (changeWidth)
            rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
        if (changeHeight)
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
        if (changeTopOffset)
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -topOffset);
        if (changeBottomOffset)
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, bottomOffset);
        if (changeLeftOffset)
            rectTransform.offsetMin = new Vector2(leftOffset, rectTransform.offsetMin.y);
        if (changeRightOffset)
            rectTransform.offsetMax = new Vector2(-rightOffset, rectTransform.offsetMax.y);
        if (changePosX)
            rectTransform.anchoredPosition = new Vector2(posX, rectTransform.anchoredPosition.y);
        if (changePosY)
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, posY);
        if (changeScaleX)
            rectTransform.localScale = new Vector3(scaleX, rectTransform.localScale.y, rectTransform.localScale.z);
        if (changeScaleY)
            rectTransform.localScale = new Vector3(rectTransform.localScale.x, scaleY, rectTransform.localScale.z);
        if (changeScaleZ)
            rectTransform.localScale = new Vector3(rectTransform.localScale.x, rectTransform.localScale.y, scaleZ);
    }
}
