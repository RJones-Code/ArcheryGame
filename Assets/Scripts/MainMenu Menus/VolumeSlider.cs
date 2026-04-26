using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

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