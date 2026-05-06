using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*
 * File: BowPickup.cs
 *
 * Description:
 * Handles the behavior when the player picks up the bow in VR.
 * This script listens for grab interactions and starts the game timer
 * when the bow is first picked up.
 *
 * Core Responsibilities:
 * - Detect when the bow is grabbed using XR interaction
 * - Trigger the game timer if the game is not already over
 * - Manage event listener lifecycle (subscribe/unsubscribe)
 *
 * Key Components:
 * - XRGrabInteractable: Enables grab interaction on the bow
 * - GameTimer: Singleton that manages game timing and state
 *
 * Behavior:
 * - On Awake():
 *      - Retrieves XRGrabInteractable component
 *      - Subscribes to the selectEntered (grab) event
 *
 * - OnBowGrabbed():
 *      - Checks if the GameTimer exists and the game is not over
 *      - Starts the timer on first valid grab
 *
 * - OnDestroy():
 *      - Unsubscribes from the grab event to prevent memory leaks
 *
 * Dependencies:
 * - Unity XR Interaction Toolkit
 * - GameTimer singleton (must expose Instance, StartTimer(), and IsGameOver)
 *
 * Usage:
 * Attach this script to the bow GameObject.
 * Ensure an XRGrabInteractable component is present on the same object.
 */

public class BowPickup : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable != null)
            grabInteractable.selectEntered.AddListener(OnBowGrabbed);

    }

    private void OnBowGrabbed(SelectEnterEventArgs args)
    {
        if (GameTimer.Instance == null || GameTimer.Instance.IsGameOver)
            return;

        GameTimer.Instance.StartTimer();
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.RemoveListener(OnBowGrabbed);
    }
}
