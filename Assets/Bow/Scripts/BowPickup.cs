using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BowPickup : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool hasStarted = false;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnBowGrabbed);
    }

    private void OnBowGrabbed(SelectEnterEventArgs args)
    {
        if (hasStarted) return;

        hasStarted = true;
        GameTimer.Instance.StartTimer();
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnBowGrabbed);
    }
}
