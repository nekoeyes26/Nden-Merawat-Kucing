using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("Sound Effects")]
    public List<SoundEffectEntry> soundEffects;

    private AudioSource audioSource; // AudioSource component to play sounds.

    private void Awake()
    {
        // Ensure that there is only one instance of SFXManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }

        // Get the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySFX(SoundEffect soundEffect)
    {
        // Find the corresponding SoundEffectEntry
        foreach (var entry in soundEffects)
        {
            if (entry.soundEffect == soundEffect)
            {
                audioSource.PlayOneShot(entry.audioClip);
                //Debug.Log(entry.audioClip.name);
                return;
            }
        }

        Debug.LogWarning("SFXManager: Sound effect not found.");
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("SFXManager: Attempted to play a null AudioClip.");
            return;
        }

        audioSource.PlayOneShot(clip);
    }
}
