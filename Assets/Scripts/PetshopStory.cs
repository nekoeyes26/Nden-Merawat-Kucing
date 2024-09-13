using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PetshopStory : MonoBehaviour, IPointerClickHandler
{
    public GameObject storyCanvas;
    public GameObject mainCanvas;
    private bool inStory = true;

    void Start()
    {
        if (GameManager.instance.previousScene == "CoverTitle")
        {
            inStory = true;
            storyCanvas.SetActive(true);
            mainCanvas.SetActive(false);
        }
        else
        {
            inStory = false;
            storyCanvas.SetActive(false);
            mainCanvas.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (inStory)
        {
            storyCanvas.SetActive(false);
            mainCanvas.SetActive(true);
            inStory = false;
        }
    }
}
