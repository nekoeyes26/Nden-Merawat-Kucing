using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVoice : MonoBehaviour
{
    private AudioSource audioSource;

    // Array to hold voice clips
    public AudioClip[] catNormal;
    public AudioClip[] catHappy;
    public AudioClip[] catSad;

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
    }

    // Method to play a random greeting voice line
    public void PlayNormal()
    {
        PlayRandomClip(catNormal);
    }

    // Method to play a random attack voice line
    public void PlayHappy()
    {
        PlayRandomClip(catHappy);
    }

    // Method to play a random damage voice line
    public void PlaySad()
    {
        PlayRandomClip(catSad);
    }

    public void PlayClip(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        audioSource.PlayOneShot(clip);
    }

    // Helper method to play a random clip from an array
    private void PlayRandomClip(AudioClip[] clips)
    {
        if (clips.Length == 0) return; // Check if there are any clips

        int randomIndex = Random.Range(0, clips.Length);
        audioSource.clip = clips[randomIndex];
        audioSource.Play();
    }
}
