using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton instance for easy access from other scripts
    public static AudioManager instance;

    // Array of audio sources for different sound effects
    [SerializeField] private AudioSource[] sfx;

    private void Awake()
    {
        // Set this as the singleton instance
        instance = this;
    }

    // Play a sound effect by index
    public void PlaySFX(int i)
    {
        // Check if index is valid before playing
        if (i < sfx.Length)
            sfx[i].Play();
    }

    // Stop a sound effect by index
    public void StopSFX(int i)
    {
        sfx[i].Stop();
    }
}