using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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
