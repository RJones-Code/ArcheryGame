using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * File: HandednessOptions.cs
 *
 * Description:
 * Handles the UI for selecting player handedness (left-handed or right-handed).
 * Synchronizes a UI Toggle with the PlayerSettings system and updates the
 * on-screen label to reflect the current selection.
 *
 * Core Responsibilities:
 * - Initialize UI state from saved player settings
 * - Update PlayerSettings when toggle value changes
 * - Provide user feedback through label updates
 *
 * Key Components:
 * - Toggle (handednessToggle): UI control for selecting handedness
 * - TMP_Text (handednessLabel): Displays current selection state
 * - PlayerSettings: Stores global handedness preference
 *
 * Behavior:
 * - Start():
 *      - Reads saved handedness setting
 *      - Sets toggle state accordingly
 *      - Updates label text
 *      - Subscribes to toggle change events
 *
 * - OnToggleChanged():
 *      - Updates PlayerSettings value
 *      - Updates UI label to match selection
 *
 * - UpdateLabel():
 *      - Displays "Left-Handed" or "Right-Handed" based on selection
 *
 * Dependencies:
 * - Unity UI Toggle
 * - TextMeshPro (TMP_Text)
 * - PlayerSettings static configuration class
 *
 * Usage:
 * Attach to a settings menu object.
 * Assign Toggle and Label references in inspector.
 * Ensure PlayerSettings is used by interaction systems (e.g., WeaponRack).
 */

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