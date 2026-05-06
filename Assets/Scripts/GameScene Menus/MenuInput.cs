using UnityEngine;
using UnityEngine.InputSystem;

/*
 * File: MenuInput.cs
 *
 * Description:
 * Handles player input for toggling the options menu using Unity's Input System.
 * Listens for a configured input action (e.g., controller button press) and
 * triggers the OptionsMenu to open or close.
 *
 * Core Responsibilities:
 * - Enable and disable input actions appropriately
 * - Detect menu button presses
 * - Toggle the options menu in response to input
 *
 * Key Components:
 * - OptionsMenu (optionsMenu): Controls menu visibility and behavior
 * - InputActionProperty (menuButton): Input binding for menu toggle action
 *
 * Behavior:
 * - OnEnable():
 *      - Enables the assigned input action
 *
 * - OnDisable():
 *      - Disables the input action to prevent unintended input
 *
 * - Update():
 *      - Checks if the menu button was pressed this frame
 *      - Calls ToggleMenu() on the OptionsMenu
 *
 * Dependencies:
 * - Unity Input System
 * - OptionsMenu script
 *
 * Usage:
 * Attach this script to a controller or input manager object.
 * Assign the OptionsMenu reference and bind the menuButton action
 * (e.g., controller menu button) in the inspector.
 */

public class MenuInput : MonoBehaviour
{
    public OptionsMenu optionsMenu;
    public InputActionProperty menuButton;

    void OnEnable()
    {
        menuButton.action.Enable();
    }

    void OnDisable()
    {
        menuButton.action.Disable();
    }

    void Update()
    {
        if (menuButton.action.WasPressedThisFrame())
        {
            optionsMenu.ToggleMenu();
        }
    }
}