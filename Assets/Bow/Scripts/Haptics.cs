using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*
 * File: StringHaptics.cs
 *
 * Description:
 * Provides haptic feedback to the VR controller while interacting with the bowstring.
 * This script generates vibration pulses during string draw and a stronger pulse on release,
 * enhancing immersion and player feedback.
 *
 * Core Responsibilities:
 * - Detect when the bowstring is grabbed and released
 * - Track controller movement relative to initial grab position
 * - Generate periodic haptic pulses while drawing the string
 * - Scale vibration intensity based on draw distance
 * - Trigger a strong haptic pulse when the string is released
 *
 * Key Components:
 * - XRGrabInteractable: Detects grab/release interactions
 * - XRBaseController: Sends haptic impulses to the VR controller
 *
 * Behavior:
 * - OnStringGrab():
 *      - Stores initial grab position
 *      - Identifies the interacting controller
 *      - Starts periodic haptic feedback
 *
 * - Update():
 *      - While drawing, sends pulses at fixed intervals
 *      - Calculates draw distance and maps it to vibration strength
 *
 * - OnStringRelease():
 *      - Sends a strong release pulse
 *      - Stops draw feedback
 *
 * - Pulse():
 *      - Sends a haptic impulse to the active controller
 *
 * Configuration Notes:
 * - MaxDrawAmplitude controls maximum vibration intensity while drawing
 * - DrawPulseInterval controls how frequently pulses are sent
 * - MaxDrawDistance defines the distance for maximum feedback scaling
 * - ReleaseAmplitude/Duration control the release feedback strength
 *
 * Dependencies:
 * - Unity XR Interaction Toolkit
 * - XR-compatible controllers with haptic support
 *
 * Usage:
 * Attach this script to the bowstring grab object.
 * Ensure an XRGrabInteractable is present on the same GameObject.
 */

public class StringHaptics : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private XRBaseController stringController;
    private Vector3 grabStartPosition;
    private bool isDrawing = false;
    private float drawTimer = 0f;

    private const float MaxDrawAmplitude = 0.8f;
    private const float DrawDuration = 0.07f;
    private const float DrawPulseInterval = 0.08f;
    private const float MaxDrawDistance = 1.5f;
    private const float ReleaseAmplitude = 1.0f;
    private const float ReleaseDuration = 0.25f;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnStringGrab);
        grabInteractable.selectExited.AddListener(OnStringRelease);
    }

    private void OnStringGrab(SelectEnterEventArgs args)
    {
        isDrawing = true;
        drawTimer = 0f;
        grabStartPosition = transform.position;

        var interactor = args.interactorObject.transform;
        stringController = interactor.GetComponentInParent<XRBaseController>();

        Pulse(0.1f, 0.1f);
    }

    private void OnStringRelease(SelectExitEventArgs args)
    {
        Pulse(ReleaseAmplitude, ReleaseDuration);

        isDrawing = false;
        stringController = null;
    }

    private void Update()
    {
        if (!isDrawing || stringController == null) return;

        drawTimer += Time.deltaTime;
        if (drawTimer >= DrawPulseInterval)
        {
            drawTimer = 0f;

            float drawDistance = Vector3.Distance(
                stringController.transform.position,
                grabStartPosition
            );

            float drawRatio = Mathf.Clamp01(drawDistance / MaxDrawDistance);
            float amplitude = Mathf.Lerp(0.1f, MaxDrawAmplitude, drawRatio);

            Pulse(amplitude, DrawDuration);
        }
    }

    private void Pulse(float amplitude, float duration)
    {
        if (stringController != null)
        {
            stringController.SendHapticImpulse(amplitude, duration);
        }
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnStringGrab);
        grabInteractable.selectExited.RemoveListener(OnStringRelease);
    }
}
