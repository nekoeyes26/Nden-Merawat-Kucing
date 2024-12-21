using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    //public string sceneName;
    public GameObject loadingCanvas;
    public Image loadingBar;

    public void LoadScene(string sceneName)
    {
        if (IsSceneNameValid(sceneName))
        {
            ResumeTimeScale();
            GameManager.instance.previousScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is not specified or invalid!");
        }
    }

    public void Exit()
    {
        PlaySoundEffect(true);
        Application.Quit();
        Debug.Log("Game Closed");
    }

    public void PreviousScene()
    {
        if (IsSceneNameValid(GameManager.instance.previousScene))
        {
            ResumeTimeScale();
            SceneManager.LoadScene(GameManager.instance.previousScene);
            GameManager.instance.previousScene = SceneManager.GetActiveScene().name;
            PlaySoundEffect(true);
        }
        else
        {
            Debug.LogWarning("Previous Scene name is not specified or invalid!");
        }
    }

    //public async void LoadSceneAsync(string sceneName)
    //{
    //    if (Time.timeScale == 0f) Time.timeScale = 1f;
    //    GameManager.instance.previousScene = SceneManager.GetActiveScene().name;
    //    var scene = SceneManager.LoadSceneAsync(sceneName);
    //    scene.allowSceneActivation = false;

    //    loadingCanvas.SetActive(true);
    //    do
    //    {
    //        await Task.Delay(100);
    //        loadingBar.fillAmount = scene.progress;
    //    } while (scene.progress < 0.9f);

    //    await Task.Delay(1000);

    //    scene.allowSceneActivation = true;
    //    loadingCanvas.SetActive(false);
    //}

    public void LoadSceneAsync(string sceneName)
    {
        if (IsSceneNameValid(sceneName))
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }
        else
        {
            Debug.LogWarning("Scene name is not specified or invalid!");
        }
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        ResumeTimeScale();
        GameManager.instance.previousScene = SceneManager.GetActiveScene().name;
        PlaySoundEffect(false);

        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        loadingCanvas.SetActive(true);
        while (scene.progress < 0.9f)
        {
            loadingBar.fillAmount = scene.progress;
            yield return null; // Wait for the next frame
        }

        // Optional: Add a delay for user experience
        yield return new WaitForSeconds(0.1f);

        scene.allowSceneActivation = true;
        loadingCanvas.SetActive(false);
    }

    private bool IsSceneNameValid(string sceneName)
    {
        return !string.IsNullOrEmpty(sceneName) && Application.CanStreamedLevelBeLoaded(sceneName);
    }

    private void ResumeTimeScale()
    {
        if (Time.timeScale == 0f) Time.timeScale = 1f;
    }

    public void PlaySoundEffect(bool isBack)
    {
        if (isBack)
        {
            SFXManager.Instance.PlaySFX(SoundEffect.Back);
        }
        else
        {
            SFXManager.Instance.PlaySFX(SoundEffect.Select);
        }
    }

    public static void LoadSceneStatic(string sceneName)
    {
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        if (sceneLoader != null)
        {
            sceneLoader.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("No SceneLoader instance found in the scene!");
        }
    }
}
