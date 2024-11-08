using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PetshopStory : MonoBehaviour, IPointerClickHandler
{
    public GameObject[] storyLayout;
    public GameObject[] mainLayout;
    private bool inStory = true;

    void Start()
    {
        if (GameManager.instance.previousScene == "CoverTitle")
        {
            inStory = true;
            foreach (GameObject ui in storyLayout)
            {
                ui.SetActive(true);
            }
            foreach (GameObject ui in mainLayout)
            {
                ui.SetActive(false);
            }
            StartCoroutine(DeactivateUIAfterDelay(3f));
        }
        else
        {
            inStory = false;
            foreach (GameObject ui in storyLayout)
            {
                ui.SetActive(false);
            }
            foreach (GameObject ui in mainLayout)
            {
                ui.SetActive(true);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (inStory)
        {
            foreach (GameObject ui in storyLayout)
            {
                ui.SetActive(false);
            }
            foreach (GameObject ui in mainLayout)
            {
                ui.SetActive(true);
            }
            inStory = false;
        }
    }

    void CloseStory()
    {
        if (inStory)
        {
            foreach (GameObject ui in storyLayout)
            {
                ui.SetActive(false);
            }
            foreach (GameObject ui in mainLayout)
            {
                ui.SetActive(true);
            }
            inStory = false;
        }
    }

    private IEnumerator DeactivateUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        CloseStory();
    }
}
