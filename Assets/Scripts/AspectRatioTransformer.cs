using UnityEngine;

public class AspectRatioTransformer : MonoBehaviour
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

    public bool changePosX = false;
    public bool changePosY = false;
    public bool changePosZ = false;
    public bool changeScaleX = false;
    public bool changeScaleY = false;
    public bool changeScaleZ = false;

    public float posX = 0;
    public float posY = 0;
    public float posZ = 0;
    public float scaleX = 1;
    public float scaleY = 1;
    public float scaleZ = 1;

    // private Transform transform;

    void Start()
    {
        // transform = GetComponent<Transform>();
        AdjustTransformForAspectRatio();
    }

    void AdjustTransformForAspectRatio()
    {
        float screenAspect = Camera.main.aspect;

        // Check for selected aspect ratio and apply changes only if it matches the screen's aspect ratio
        switch (selectedAspectRatio)
        {
            case AspectRatios.Aspect_4_3:
                if (Mathf.Abs(screenAspect - 4f / 3f) < 0.012f)
                    ApplyTransformChanges("4:3");
                break;
            case AspectRatios.Aspect_5_3:
                if (Mathf.Abs(screenAspect - 5f / 3f) < 0.012f)
                    ApplyTransformChanges("5:3");
                break;
            case AspectRatios.Aspect_5_4:
                if (Mathf.Abs(screenAspect - 5f / 4f) < 0.012f)
                    ApplyTransformChanges("5:4");
                break;
            case AspectRatios.Aspect_16_9:
                if (Mathf.Abs(screenAspect - 16f / 9f) < 0.012f)
                    ApplyTransformChanges("16:9");
                break;
            case AspectRatios.Aspect_18_9:
                if (Mathf.Abs(screenAspect - 18f / 9f) < 0.012f)
                    ApplyTransformChanges("18:9");
                break;
            case AspectRatios.Aspect_20_9:
                if (Mathf.Abs(screenAspect - 20f / 9f) < 0.012f)
                    ApplyTransformChanges("20:9");
                break;
            case AspectRatios.Aspect_21_9:
                if (Mathf.Abs(screenAspect - 21f / 9f) < 0.012f)
                    ApplyTransformChanges("21:9");
                break;
            case AspectRatios.Aspect_22_9:
                if (Mathf.Abs(screenAspect - 22f / 9f) < 0.012f)
                    ApplyTransformChanges("22:9");
                break;
            case AspectRatios.Aspect_21_10:
                if (Mathf.Abs(screenAspect - 21f / 10f) < 0.012f)
                    ApplyTransformChanges("21:10");
                break;
            default:
                Debug.Log("No aspect ratio selected or does not match screen ratio.");
                break;
        }
    }

    private void ApplyTransformChanges(string aspectRatio)
    {
        Debug.Log("Aspect ratio: " + aspectRatio);
        if (changePosX)
            transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        if (changePosY)
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        if (changePosZ)
            transform.position = new Vector3(transform.position.x, transform.position.y, posZ);
        if (changeScaleX)
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        if (changeScaleY)
            transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z);
        if (changeScaleZ)
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, scaleZ);
    }
}