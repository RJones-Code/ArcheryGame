using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class WeaponRack : MonoBehaviour
{
    public GameObject bowPrefabRight;
    public GameObject bowPrefabLeft;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        var interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor;
        if (interactor == null) return;

        // Release the rack first to allow the hand to grab the bow
        grabInteractable.interactionManager.SelectExit(interactor, grabInteractable);

        // Choose correct prefab
        GameObject prefabToSpawn = PlayerSettings.IsLeftHanded
            ? bowPrefabLeft
            : bowPrefabRight;
        // Spawn the bow
        GameObject bow = Instantiate(prefabToSpawn);
        bow.SetActive(true);

        var bowGrab = bow.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (bowGrab == null || !bowGrab.enabled) return;

        // Delay grab one frame to ensure registration
        StartCoroutine(ForceGrabNextFrame(interactor, bowGrab));
    }

    private IEnumerator ForceGrabNextFrame(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor, UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable bowGrab)
    {
        yield return null; // wait one frame
        grabInteractable.interactionManager.SelectEnter(interactor, (UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable)bowGrab);
    }
}