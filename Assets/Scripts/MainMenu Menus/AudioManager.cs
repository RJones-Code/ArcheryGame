using UnityEngine;
using UnityEngine.Audio;

/*
 * File: AudioManager.cs
 *
 * Description:
 * Global audio management system responsible for controlling master volume
 * through a Unity AudioMixer. Implements a persistent singleton so audio
 * settings persist across scene loads.
 *
 * Core Responsibilities:
 * - Manage global audio volume (Master Volume)
 * - Apply volume changes to Unity AudioMixer
 * - Persist volume settings using PlayerPrefs
 * - Maintain a single persistent instance across scenes
 *
 * Key Components:
 * - AudioMixer (audioMixer): Unity mixer used to control audio groups
 * - Instance: Singleton reference for global access
 *
 * Behavior:
 * - Awake():
 *      - Enforces singleton pattern
 *      - Persists object across scene loads
 *
 * - SetVolume(float volume):
 *      - Converts linear volume (0–1) to decibel scale
 *      - Applies value to AudioMixer parameter "MasterVolume"
 *      - Saves value to PlayerPrefs for persistence
 *
 * - Start():
 *      - Loads saved volume from PlayerPrefs
 *      - Applies it on initialization
 *
 * Dependencies:
 * - Unity AudioMixer system
 * - PlayerPrefs for persistence
 *
 * Usage:
 * Attach this script to a persistent GameObject in the initial scene.
 * Call AudioManager.Instance.SetVolume(value) from UI sliders or settings menus.
 */

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioMixer audioMixer;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetVolume(float volume)
    {
        float dB = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("MasterVolume", dB);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    private void Start()
    {
        float volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        SetVolume(volume);
    }
}