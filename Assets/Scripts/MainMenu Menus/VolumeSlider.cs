using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public TMP_Text volumeText;

    void Start()
    {
        float volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        slider.value = volume;
        UpdateText(volume);
    }

    public void OnSliderChanged(float value)
    {
        UpdateText(value);
    }

    void UpdateText(float value)
    {
        int percent = Mathf.RoundToInt(value * 100);
        volumeText.text = "" + percent + "%";
    }
}