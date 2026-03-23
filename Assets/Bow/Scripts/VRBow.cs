using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRBowTwoHanded : MonoBehaviour
{
    [Header("Bow Parts")]
    public Transform arrowSpawnPoint;
    public Transform secondaryAttachPoint;

    [Header("Arrow")]
    public GameObject arrowPrefab;

    [Header("Settings")]
    public float maxPullDistance = 0.5f;
    public float shootForceMultiplier = 40f;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable bowGrab;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor primaryHand;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor secondaryHand;

    private bool isPulling = false;
    private GameObject currentArrow;

    void Awake()
    {
        bowGrab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        bowGrab.selectEntered.AddListener(OnBowGrabbed);
        bowGrab.selectExited.AddListener(OnBowReleased);
    }

    void Update()
    {
        if (bowGrab.interactorsSelecting.Count > 1)
        {
            UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor ixr = bowGrab.interactorsSelecting[1];
            secondaryHand = ixr.transform.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor>();

            if (!isPulling)
                StartPull();
        }
        else
        {
            secondaryHand = null;
            if (isPulling)
                ReleaseArrow();
        }

        if (isPulling && primaryHand != null && secondaryHand != null)
        {
            UpdateArrowAndString();
        }
    }

    private void OnBowGrabbed(SelectEnterEventArgs args)
    {
        // Use interactorObject instead of interactor
        primaryHand = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
    }

    private void OnBowReleased(SelectExitEventArgs args)
    {
        primaryHand = null;

        if (isPulling)
        {
            isPulling = false;
            if (currentArrow != null)
            {
                Destroy(currentArrow);
                currentArrow = null;
            }
        }
    }

    private void StartPull()
    {
        if (primaryHand == null || secondaryHand == null) return;

        isPulling = true;

        if (currentArrow == null)
        {
            currentArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
            currentArrow.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void ReleaseArrow()
    {
        if (!isPulling || primaryHand == null || secondaryHand == null) return;

        float pullDistance = Vector3.Distance(primaryHand.transform.position, secondaryHand.transform.position);
        pullDistance = Mathf.Clamp(pullDistance, 0f, maxPullDistance);
        float force = (pullDistance / maxPullDistance) * shootForceMultiplier;

        if (currentArrow != null)
        {
            Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.linearVelocity = arrowSpawnPoint.forward * force;
            currentArrow = null;
        }

        isPulling = false;
    }

    private void UpdateArrowAndString()
    {
        if (currentArrow != null)
        {
            currentArrow.transform.position = arrowSpawnPoint.position;
            currentArrow.transform.rotation = arrowSpawnPoint.rotation;
        }

        // Optional: animate string here
    }
}