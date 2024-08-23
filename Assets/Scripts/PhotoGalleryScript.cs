using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoGalleryScript : MonoBehaviour
{
    public GameObject imagePrefab; // Prefab for the Image UI element
    public Transform contentTransform; // The content of the ScrollView
    // Start is called before the first frame update
    void Start()
    {
        DisplayPhotos();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DisplayPhotos()
    {
        // Clear any existing children in the content transform
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        // Loop through each saved screenshot in WefieController
        for (int i = 0; i < WefieController.SavedScreenshots.Count; i++)
        {
            // Instantiate a new Image UI element from the prefab
            GameObject newImage = Instantiate(imagePrefab, contentTransform);

            // Get the Image component from the new UI element
            Image imageComponent = newImage.GetComponent<Image>();

            // Convert the Texture2D to a Sprite and assign it to the Image component
            Texture2D texture = WefieController.SavedScreenshots[i];
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            imageComponent.sprite = sprite;
        }
    }
}
