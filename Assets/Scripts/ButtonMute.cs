using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMute : MonoBehaviour
{
    public Button muteButton;
    public Sprite muteSprite;
    public Sprite unmuteSprite;
    void Start()
    {
        muteButton = GetComponent<Button>();
        muteButton.onClick.AddListener(MusicManager.Instance.ToggleMute);
        muteButton.onClick.AddListener(UpdateButtonSprite);
        UpdateButtonSprite();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateButtonSprite()
    {
        Image buttonImage = GetComponent<Image>();
        buttonImage.sprite = MusicManager.Instance.musicSource.mute ? muteSprite : unmuteSprite;
    }
}
