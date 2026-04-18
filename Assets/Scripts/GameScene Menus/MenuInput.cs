using UnityEngine;
using UnityEngine.InputSystem;

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