using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PhotoGalleryScript : MonoBehaviour
{
    public GameObject imagePrefab;
    public Transform contentTransform;
    public GameObject noScreenshotText;
    public ScrollRect scrollRect;
    public float scrollDuration = 0.5f;
    public Image display;
    public GameObject[] displayFrame;
    public GameObject popUpConfirmation;
    public GameObject popUpSaved;
    private GameObject[] glows;
    private GameObject[] allButtonLihat;
    private bool isImageClicked = false;
    // Start is called before the first frame update
    void Start()
    {
        WefieController.LoadScreenshots();
        noScreenshotText.SetActive(false);
        if (WefieController.SavedScreenshots.Count > 0)
        {
            glows = new GameObject[WefieController.SavedScreenshots.Count];
            allButtonLihat = new GameObject[WefieController.SavedScreenshots.Count];
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
                    scrollRect.verticalNormalizedPosition = 0f;
                }
            }
        }
        else
        {
            noScreenshotText.SetActive(true);
        }
        CloseImage();
        ClosePopUpConfirmation();
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

        for (int i = 0; i < WefieController.SavedScreenshots.Count; i += 3)
        {
            GameObject newPanel = Instantiate(imagePrefab, contentTransform);

            // Loop child ke 3 sampe 5
            for (int j = 2; j < 5; j++)
            {
                // Get the Picture Frame by accessing the prefab's children directly
                Transform frameParent = newPanel.transform.GetChild(j);
                Transform glow = frameParent.transform.GetChild(0);
                glow.gameObject.SetActive(false);

                Transform pictureFrame = frameParent.transform.GetChild(1);

                // Get the Image component in the child of the Picture Frame
                Transform imageMask = pictureFrame.GetChild(0);
                Transform imageObject = imageMask.GetChild(0);
                Transform buttonObject = pictureFrame.GetChild(2);
                buttonObject.gameObject.SetActive(false);
                Image imageComponent = imageObject.GetComponent<Image>();


                // Calculate the index of the screenshot
                int screenshotIndex = i + (j - 2);

                // If we are within bounds of SavedScreenshots, assign the screenshot to the Image component
                if (screenshotIndex < WefieController.SavedScreenshots.Count)
                {
                    // Retrieve the corresponding screenshot and assign it to the Image component
                    Texture2D texture = WefieController.SavedScreenshots[screenshotIndex];
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    imageComponent.sprite = sprite;
                    Button buttonComponent = pictureFrame.GetComponent<Button>();
                    buttonComponent.onClick.AddListener(() => ClickImage(new GameObject[] { glow.gameObject, buttonObject.gameObject }));
                    Button buttonLihat = buttonObject.GetComponent<Button>();
                    buttonLihat.onClick.AddListener(() => OpenImage(sprite));
                    glows[screenshotIndex] = glow.gameObject;
                    allButtonLihat[screenshotIndex] = buttonLihat.gameObject;
                }
                else
                {
                    // If there are no more screenshots left, hide the entire Picture Frame
                    pictureFrame.gameObject.SetActive(false);
                }
            }
        }

    }

    IEnumerator SmoothScroll()
    {
        float elapsedTime = 0f;
        float startValue = 1f;
        float endValue = 0f;

        while (elapsedTime < scrollDuration)
        {
            elapsedTime += Time.deltaTime;
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(startValue, endValue, elapsedTime / scrollDuration);
            yield return null;
        }

        scrollRect.verticalNormalizedPosition = endValue;
    }

    public void OpenImage(Sprite spriteImage)
    {
        foreach (GameObject UI in displayFrame)
        {
            UI.SetActive(true);
        }
        display.enabled = true;
        display.sprite = spriteImage;
    }

    public void CloseImage()
    {
        foreach (GameObject UI in displayFrame)
        {
            UI.SetActive(false);
        }
        display.enabled = false;
    }

    public void DownloadImage(Image image)
    {
        Sprite sprite = image.sprite;
        Texture2D texture2D = sprite.texture;
        // texture2D.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        // texture2D.Apply();

        string name = "IMG" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";

        // byte[] bytes = texture2D.EncodeToPNG();
        // File.WriteAllBytes(Application.dataPath + "/" + name, bytes);

        NativeGallery.SaveImageToGallery(texture2D, "Merawat Kucing", name);
        ShowPopUpSaved();
    }

    public void ShowPopUpConfirmation()
    {
        popUpConfirmation.SetActive(true);
    }

    public void ClosePopUpConfirmation()
    {
        popUpConfirmation.SetActive(false);
    }

    public void ShowPopUpSaved()
    {
        popUpSaved.SetActive(true);
    }

    public void ClosePopUpSaved()
    {
        popUpSaved.SetActive(false);
    }

    public void Share(Image image)
    {
        Sprite sprite = image.sprite;
        Texture2D texture2D = sprite.texture;
        string name = "IMG" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        string path = Path.Combine(Application.temporaryCachePath, name);
        File.WriteAllBytes(path, texture2D.EncodeToPNG());
        new NativeShare().AddFile(path).SetSubject("This is my cat").SetText("My cat is very cute isn't it?").Share();
    }

    public GameObject[] previousObjs;
    public void ClickImage(GameObject[] objs)
    {
        if (previousObjs == null || !previousObjs.SequenceEqual(objs))
        {
            foreach (GameObject glow in glows)
            {
                glow.SetActive(false);
            }
            foreach (GameObject button in allButtonLihat)
            {
                button.SetActive(false);
            }
            previousObjs = objs;
            isImageClicked = false;
        }
        else
        {
            isImageClicked = true;
            previousObjs = null;
        }

        if (!isImageClicked)
        {
            foreach (GameObject obj in objs)
            {
                obj.SetActive(true);
            }
            isImageClicked = true;
        }
        else if (isImageClicked)
        {
            foreach (GameObject obj in objs)
            {
                obj.SetActive(false);
            }
            isImageClicked = false;
        }
    }
}
