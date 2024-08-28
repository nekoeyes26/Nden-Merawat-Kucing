using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public string sceneName;
    public static string previousScene;
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            if (Time.timeScale == 0f) Time.timeScale = 1f;
            previousScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is not specified!");
        }
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Game Close");
    }

    public void PreviousScene()
    {
        if (!string.IsNullOrEmpty(previousScene))
        {
            if (Time.timeScale == 0f) Time.timeScale = 1f;
            SceneManager.LoadScene(previousScene);
            previousScene = SceneManager.GetActiveScene().name;
        }
        else
        {
            Debug.LogWarning("Previous Scene name is not specified!");
        }
    }
}
