using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;

public class WefieController : MonoBehaviour
{
    public Image display;
    public GameObject[] wefieUI;
    public GameObject[] displayFrame;
    public Image charOnLeft;
    public Image charOnRight;
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
        LoadScreenshots();
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
        SavedScreenshots.Add(textureDisplay);
        Destroy(screenshot);

        texture2D.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture2D.Apply();

        // string name = "IMG" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";

        // byte[] bytes = texture2D.EncodeToPNG();
        // File.WriteAllBytes(Application.dataPath + "/" + name, bytes);

        // NativeGallery.SaveImageToGallery(texture2D, "Merawat Kucing", name);

        // Debug.Log("File saved as " + name);
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
        StartCoroutine(Screenshot());
        if (!isXPAdded)
        {
            // if (GameManager.instance.CatProfile.catScriptable.photoRemaining > 0)
            // {
            //     GameManager.instance.CatProfile.catScriptable.photoRemaining--;
            //     GameManager.instance.AddXP();
            //     GameManager.instance.LevelUpChecker();
            // }
            GameManager.instance.CompleteMissionChecker(ref GameManager.instance.CatProfile.catScriptable.photoRemaining);
            isXPAdded = true;
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

    public void ChangeLeftCharPose(Sprite charSprite)
    {
        charOnLeft.sprite = charSprite;
    }

    public void ChangeRightCharPose(Sprite charSprite)
    {
        charOnRight.sprite = charSprite;
    }

    public static void SaveScreenshots()
    {
        Debug.Log("saving screenshot");
        // Create a binary formatter
        BinaryFormatter formatter = new BinaryFormatter();

        string directoryPath = Application.persistentDataPath + "/ScreenshotData";
        Directory.CreateDirectory(directoryPath);
        string filePath = directoryPath + "/saved_screenshot.dat";
        FileStream file = File.Create(filePath);

        // Create a list of byte arrays to store the textures
        List<byte[]> textureBytes = new List<byte[]>();

        // Convert each texture to a byte array
        foreach (Texture2D texture in SavedScreenshots)
        {
            textureBytes.Add(texture.EncodeToPNG());
        }

        // Serialize the list of byte arrays
        formatter.Serialize(file, textureBytes);

        // Close the file stream
        file.Close();
    }

    public static void LoadScreenshots()
    {
        if (File.Exists(Application.persistentDataPath + "/ScreenshotData/saved_screenshot.dat"))
        {
            Debug.Log("Loading Screenshot");
            // Create a binary formatter
            BinaryFormatter formatter = new BinaryFormatter();

            // Create a file stream
            FileStream file = File.Open(Application.persistentDataPath + "/ScreenshotData/saved_screenshot.dat", FileMode.Open);

            // Deserialize the list of byte arrays
            List<byte[]> textureBytes = formatter.Deserialize(file) as List<byte[]>;

            // Close the file stream
            file.Close();

            // Clear the SavedScreenshots list
            SavedScreenshots.Clear();

            // Convert each byte array back to a texture
            foreach (byte[] bytes in textureBytes)
            {
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(bytes);
                SavedScreenshots.Add(texture);
            }
        }
    }

    void OnDisable()
    {
        SaveScreenshots();
    }
}
