using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandednessOptions : MonoBehaviour
{
    public Toggle handednessToggle;
    public TMP_Text handednessLabel;

    private void Start()
    {
        // Initialize from saved setting
        handednessToggle.isOn = PlayerSettings.IsLeftHanded;
        UpdateLabel(handednessToggle.isOn);

        handednessToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isLeftHanded)
    {
        PlayerSettings.IsLeftHanded = isLeftHanded;
        UpdateLabel(isLeftHanded);
    }

    private void UpdateLabel(bool isLeftHanded)
    {
        if (isLeftHanded)
            handednessLabel.text = "Left-Handed";
        else
            handednessLabel.text = "Right-Handed";
    }
}