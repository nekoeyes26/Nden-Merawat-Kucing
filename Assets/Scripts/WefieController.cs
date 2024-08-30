using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WefieController : MonoBehaviour
{
    public Image display;
    public GameObject[] wefieUI;
    public GameObject[] displayFrame;
    public Image charKiri;
    public Image charKanan;
    public Image cat;
    private CatScriptable catS;
    private bool isXPAdded = false;
    public static List<Texture2D> SavedScreenshots = new List<Texture2D>();
    // Start is called before the first frame update
    void Start()
    {
        catS = GameManager.instance.CatProfile.catScriptable;
        Sprite sprite;
        if (catS.phase == CatPhase.Baby)
        {
            sprite = Resources.Load<Sprite>(catS.spriteFolderPath + "baby");
        }
        else if (catS.phase == CatPhase.Child)
        {
            sprite = Resources.Load<Sprite>(catS.spriteFolderPath + "child");
        }
        else if (catS.phase == CatPhase.Adult)
        {
            sprite = Resources.Load<Sprite>(catS.spriteFolderPath + "adult");
        }
        else
        {
            sprite = cat.sprite;
        }
        cat.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Screenshot()
    {
        yield return new WaitForEndOfFrame();
        Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();
        Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        Texture2D textureDisplay = new Texture2D(screenshot.width, screenshot.height, TextureFormat.RGB24, false);
        textureDisplay.SetPixels(screenshot.GetPixels());
        textureDisplay.Apply();
        WefieController.SavedScreenshots.Add(textureDisplay);
        Destroy(screenshot);

        texture2D.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture2D.Apply();

        string name = "IMG" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";

        // byte[] bytes = texture2D.EncodeToPNG();
        // File.WriteAllBytes(Application.dataPath + "/" + name, bytes);

        NativeGallery.SaveImageToGallery(texture2D, "Merawat Kucing", name);

        Debug.Log("File saved as " + name);
        Sprite screenshotSprite = Sprite.Create(textureDisplay, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        foreach (GameObject UI in displayFrame)
        {
            UI.SetActive(true);
        }
        display.enabled = true;
        display.sprite = screenshotSprite;

        Destroy(texture2D);
    }

    // private IEnumerator ScreenshotWithAspectRatio()
    // {
    //     yield return new WaitForEndOfFrame();

    //     int screenWidth = Screen.width;
    //     int screenHeight = Screen.height;

    //     // Calculate the new width and height to maintain 16:9 aspect ratio
    //     float aspectRatio = 16f / 9f;
    //     int newWidth, newHeight;
    //     if (screenWidth / (float)screenHeight > aspectRatio)
    //     {
    //         newWidth = (int)(screenHeight * aspectRatio);
    //         newHeight = screenHeight;
    //     }
    //     else
    //     {
    //         newWidth = screenWidth;
    //         newHeight = (int)(screenWidth / aspectRatio);
    //     }

    //     // Create a new Texture2D with the calculated dimensions
    //     Texture2D texture2D = new Texture2D(newWidth, newHeight, TextureFormat.RGB24, false);

    //     // Calculate the offset to center the crop
    //     int offsetX = (screenWidth - newWidth) / 2;
    //     int offsetY = (screenHeight - newHeight) / 2;

    //     // ReadPixels from the screen into the new texture
    //     texture2D.ReadPixels(new Rect(offsetX, offsetY, newWidth, newHeight), 0, 0);
    //     texture2D.Apply();

    //     string name = "IMG" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";

    //     byte[] bytes = texture2D.EncodeToPNG();
    //     File.WriteAllBytes(Application.dataPath + "/" + name, bytes);

    //     // NativeGallery.SaveImageToGallery(texture2D, "Merawat Kucing", name);

    //     Debug.Log("File saved as " + name);
    //     Destroy(texture2D);
    //     foreach (GameObject UI in wefieUI)
    //     {
    //         UI.SetActive(true);
    //     }
    // }

    public void TakeScreenshot()
    {
        foreach (GameObject UI in wefieUI)
        {
            UI.SetActive(false);
        }
        StartCoroutine("Screenshot");
        if (!isXPAdded && GameManager.instance.CatProfile.catScriptable.photoRemaining > 0)
        {
            GameManager.instance.AddXP();
            GameManager.instance.CatProfile.catScriptable.photoRemaining--;
            isXPAdded = true;
            GameManager.instance.LevelUpChecker();
            if (GameManager.instance.CatProfile.catScriptable.isSad) GameManager.instance.ChangeSad();
            GameManager.instance.isPhotoTimerOn = false;
        }
    }

    public void closeDisplay()
    {
        foreach (GameObject UI in wefieUI)
        {
            UI.SetActive(true);
        }
        display.enabled = false;
        foreach (GameObject UI in displayFrame)
        {
            UI.SetActive(false);
        }
    }

    public void ChangeParizPose(Sprite pariz)
    {
        charKiri.sprite = pariz;
    }

    public void ChangeCobozPose(Sprite coboz)
    {
        charKanan.sprite = coboz;
    }
}
