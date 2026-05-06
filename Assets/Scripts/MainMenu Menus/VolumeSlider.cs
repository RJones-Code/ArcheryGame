using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

/*
 * File: VolumeSlider.cs
 *
 * Description:
 * Handles audio volume control through a UI slider and Unity AudioMixer.
 * Stores user preferences using PlayerPrefs and updates both the mixer
 * and on-screen text in real time.
 *
 * Core Responsibilities:
 * - Read and save volume settings using PlayerPrefs
 * - Convert linear slider values to decibel (dB) values for AudioMixer
 * - Update UI text to reflect current volume percentage
 * - Apply changes immediately when the slider is moved
 *
 * Key Components:
 * - Slider (slider): UI control for volume adjustment
 * - TMP_Text (volumeText): Displays volume percentage
 * - AudioMixer (mixer): Applies volume changes to audio groups
 *
 * Behavior:
 * - Start():
 *      - Loads saved volume from PlayerPrefs (default = 1f)
 *      - Initializes slider and UI text
 *
 * - OnSliderChanged(float value):
 *      - Saves new volume to PlayerPrefs
 *      - Applies volume to AudioMixer
 *      - Updates UI text
 *
 * - ApplyVolume():
 *      - Converts linear 0–1 slider value into logarithmic dB scale
 *      - Sends value to AudioMixer parameter
 *
 * - UpdateText():
 *      - Converts volume to percentage display (0–100%)
 *
 * Dependencies:
 * - Unity AudioMixer system
 * - Unity UI Slider
 * - TextMeshPro (TMP_Text)
 *
 * Usage:
 * Attach this script to a volume settings UI object.
 * Assign slider, text, and AudioMixer in the inspector.
 * Link OnSliderChanged() to the slider’s OnValueChanged event.
 */

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public TMP_Text volumeText;

    public AudioMixer mixer;
    public string mixerParameter; // MasterVolume, MusicVolume, etc.
    public string volumeKey;

    void Start()
    {
        float volume = PlayerPrefs.GetFloat(volumeKey, 1f);
        slider.value = volume;
        UpdateText(volume);
    }

    public void OnSliderChanged(float value)
    {
        PlayerPrefs.SetFloat(volumeKey, value);
        ApplyVolume(value);
        UpdateText(value);
    }

    void ApplyVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
        mixer.SetFloat(mixerParameter, dB);
    }

    void UpdateText(float value)
    {
        volumeText.text = Mathf.RoundToInt(value * 100) + "%";
    }
}