using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ResetData : MonoBehaviour
{
    public GameObject confirmationUI;
    public Button[] someButton;
    private bool isConfiriming = false;
    // Start is called before the first frame update
    void Start()
    {
        confirmationUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DeleteConfirmation()
    {
        if (!isConfiriming)
        {
            confirmationUI.SetActive(true);
            DisableButtons();
            isConfiriming = true;
        }
        else if (isConfiriming)
        {
            CancelConfirmation();
            isConfiriming = false;
        }
    }

    public void CancelConfirmation()
    {
        confirmationUI.SetActive(false);
        EnableButtons();
        isConfiriming = false;
    }

    public void ConfirmDeletion()
    {
        confirmationUI.SetActive(false);
        ResetCatData();
        EnableButtons();
        isConfiriming = false;
    }

    public void ResetCatData()
    {
        for (int i = 1; i <= 6; i++)
        {
            if (File.Exists(Application.persistentDataPath + "/CatData/Cat_" + i + ".json"))
            {
                File.Delete(Application.persistentDataPath + "/CatData/Cat_" + i + ".json");
            }
        }
        if (File.Exists(Application.persistentDataPath + "/ScreenshotData/saved_screenshot.dat"))
        {
            File.Delete(Application.persistentDataPath + "/ScreenshotData/saved_screenshot.dat");
        }
        Application.Quit();
    }

    public void DisableButtons()
    {
        foreach (Button button in someButton)
        {
            button.interactable = false;
        }
    }

    public void EnableButtons()
    {
        foreach (Button button in someButton)
        {
            button.interactable = true;
        }
    }
}
