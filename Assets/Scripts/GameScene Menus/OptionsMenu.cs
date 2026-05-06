using UnityEngine;

/*
 * File: OptionsMenu.cs
 *
 * Description:
 * Controls the visibility, positioning, and interaction behavior of the in-game options menu.
 * The menu appears in front of the player in VR, enables UI interaction, and pauses the game
 * while open.
 *
 * Core Responsibilities:
 * - Toggle menu visibility on/off
 * - Position the menu in front of the player’s view
 * - Enable/disable UI interaction rays (near/far interactors)
 * - Pause and resume gameplay using Time.timeScale
 *
 * Key Components:
 * - playerCamera: Reference to the player’s camera (used for positioning)
 * - leftNearFar / rightNearFar: UI interaction objects (XR ray/near interactors)
 *
 * Behavior:
 * - ToggleMenu():
 *      - Switches between visible and hidden states
 *
 * - ShowMenu():
 *      - Activates menu GameObject
 *      - Enables UI interactors
 *      - Positions menu in front of the player
 *      - Pauses the game
 *
 * - HideMenu():
 *      - Disables UI interactors
 *      - Deactivates menu GameObject
 *      - Resumes the game
 *
 * - ShowInFrontOfPlayer():
 *      - Positions the menu at a fixed distance in front of the player
 *      - Aligns rotation to face the player horizontally (no vertical tilt)
 *
 * Dependencies:
 * - Player camera transform (VR rig)
 * - XR interaction system (near/far interactors)
 *
 * Usage:
 * Attach this script to the options menu canvas.
 * Assign playerCamera and UI interactors in the inspector.
 * Call ToggleMenu() from input (e.g., controller button).
 */

public class OptionsMenu : MonoBehaviour
{
    public Transform playerCamera;
    public float distanceFromPlayer = 2f;
    public float heightOffset = 1.2f;

    public GameObject leftNearFar;
    public GameObject rightNearFar;
    
    private bool isVisible = false;

    void Start()
    {
        // Start hidden but DO NOT disable object itself
        HideMenuInstant();
        SetUIInteractors(false);
    }

    public void ToggleMenu()
    {
        isVisible = !isVisible;

        if (isVisible)
        {
            ShowMenu();
        }
        else
        {
            HideMenu();
        }
    }

    void ShowMenu()
    {
        gameObject.SetActive(true);

        SetUIInteractors(true);

        ShowInFrontOfPlayer();

        Time.timeScale = 0f; //Freeze game while menu is open
    }

    void HideMenu()
    {
        SetUIInteractors(false);

        gameObject.SetActive(false);

        Time.timeScale = 1f; // Unfreeze game when menu is closed
    }

    void HideMenuInstant()
    {
        gameObject.SetActive(false);
    }

    void SetUIInteractors(bool state)
    {
        if (leftNearFar != null)
            leftNearFar.SetActive(state);

        if (rightNearFar != null)
            rightNearFar.SetActive(state);
    }

    void ShowInFrontOfPlayer()
    {
        // Flatten forward direction (no tilt)
        Vector3 forward = playerCamera.forward;
        forward.y = 0;
        forward.Normalize();

        // Position in front + raise height
        Vector3 position = playerCamera.position + forward * distanceFromPlayer;
        position.y += heightOffset;

        transform.position = position;

        // Face player
        transform.rotation = Quaternion.LookRotation(forward);
    }
}