using UnityEngine;
using UnityEngine.InputSystem;

public class AssignTriggers : MonoBehaviour
{
    public InputActionProperty grabAction;
    public HandAnimation handAnimation;

    private bool wasPressed = false;

    void Start()
    {
        if (handAnimation == null)
            handAnimation = GetComponentInChildren<HandAnimation>();
    }

    void Update()
    {
        if (handAnimation == null)
            return;

        float value = grabAction.action.ReadValue<float>();
        bool isPressed = value > 0.1f;

        if (isPressed && !wasPressed)
        {
            handAnimation.TriggerGrab();
        }
        else if (!isPressed && wasPressed)
        {
            handAnimation.TriggerLet();
        }

        wasPressed = isPressed;
    }
}