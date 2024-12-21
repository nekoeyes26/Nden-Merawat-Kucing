using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Audio Settings")]
    [SerializeField] private float musicVolume = 0.5f;

    [Header("Audio Clip")]
    public AudioClip mainMusic;
    public AudioClip minigameMusic;
    public AudioClip bathMusic;
    public AudioClip feedMusic;

    [Header("Audio Source")]
    public AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Set initial volume
        musicSource = GetComponent<AudioSource>();
        musicSource.volume = musicVolume;
    }

    public void PlayMusic(AudioClip music, bool loop = true)
    {
        if (music != null)
        {
            musicSource.clip = music;
            musicSource.loop = loop;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp(volume, 0f, 1f);
        musicSource.volume = musicVolume;
    }

    public void MuteMusic(bool mute)
    {
        musicSource.mute = mute;
    }

    public void ToggleMute()
    {
        MuteMusic(!musicSource.mute);
    }
}
