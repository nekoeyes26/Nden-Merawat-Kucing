using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorMainPage : MonoBehaviour
{
    private Button button;
    private Image doorImage;
    public Sprite doorOpen;
    public Sprite doorClose;
    private bool isDoorClosed = true;
    private float doorIdleTimer;
    public float idleTime = 3f;

    void Start()
    {
        button = GetComponent<Button>();
        doorImage = GetComponent<Image>();
        button.onClick.AddListener(OpenDoorLoadScene);
        doorIdleTimer = 0f;
    }

    void Update()
    {
        if (!isDoorClosed)
        {
            doorIdleTimer += Time.deltaTime;
            if (doorIdleTimer >= idleTime)
            {
                doorImage.sprite = doorClose;
                isDoorClosed = true;
                doorIdleTimer = 0;
            }
        }

    }

    void OpenDoorLoadScene()
    {
        if (isDoorClosed)
        {
            doorImage.sprite = doorOpen;
            isDoorClosed = false;
        }
        else
        {
            SceneLoad sceneLoad = new SceneLoad();
            sceneLoad.sceneName = "Playing";
            sceneLoad.LoadScene();
        }
    }
}
