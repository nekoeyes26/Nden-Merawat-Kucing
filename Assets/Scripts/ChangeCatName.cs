using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCatName : MonoBehaviour
{
    public GameObject changeNameCanvas;
    public InputField inputField;
    public Button giveNameButton;
    // Start is called before the first frame update
    void Start()
    {
        changeNameCanvas.SetActive(false);
    }

    void Update()
    {
        if (changeNameCanvas.activeSelf)
        {
            if (string.IsNullOrWhiteSpace(inputField.text) || !Regex.IsMatch(inputField.text, @"^(?=.*[a-zA-Z])[a-zA-Z0-9'\s]+$"))
            {
                giveNameButton.interactable = false;
            }
            else
            {
                giveNameButton.interactable = true;
            }
        }
    }

    public void OpenChangeNameMenu()
    {
        changeNameCanvas.SetActive(true);
        inputField.text = GameManager.instance.CatProfile.catScriptable.name;
    }

    public void CloseChangeNameMenu()
    {
        changeNameCanvas.SetActive(false);
    }

    public void ChangeNameApply()
    {
        GameManager.instance.GiveName(inputField.text.Trim());
        changeNameCanvas.SetActive(false);
    }
}
