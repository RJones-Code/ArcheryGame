using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

/*
 * File: WeaponRack.cs
 *
 * Description:
 * Handles spawning and equipping a bow when the player interacts with the weapon rack.
 * When the rack is grabbed, it immediately releases itself, spawns the appropriate bow
 * (left- or right-handed), and forces the player's controller to grab the spawned bow.
 *
 * Core Responsibilities:
 * - Detect interaction with the weapon rack
 * - Spawn the correct bow prefab based on player handedness
 * - Transfer the grab interaction from the rack to the spawned bow
 * - Ensure proper timing for XR interaction registration
 * - Optionally start the game timer upon equipping the bow
 *
 * Key Components:
 * - XRGrabInteractable: Enables grab interaction on the weapon rack
 * - bowPrefabRight / bowPrefabLeft: Prefabs for right- and left-handed bows
 * - PlayerSettings: Determines handedness (IsLeftHanded)
 *
 * Behavior:
 * - OnGrab():
 *      - Identifies the interactor (controller)
 *      - Forces release of the rack
 *      - Selects appropriate bow prefab based on handedness
 *      - Instantiates the bow
 *      - Starts a coroutine to force the interactor to grab the bow
 *
 * - ForceGrabNextFrame():
 *      - Waits one frame to ensure the new object is registered
 *      - Forces the XR interaction system to grab the spawned bow
 *      - Starts the game timer if available
 *
 * Dependencies:
 * - Unity XR Interaction Toolkit
 * - PlayerSettings (must expose IsLeftHanded)
 * - GameTimer singleton (optional, used to start gameplay)
 *
 * Usage:
 * Attach this script to the weapon rack GameObject.
 * Ensure it has an XRGrabInteractable component.
 * Assign both bow prefabs in the inspector.
 */

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

        if (GameTimer.Instance != null)
            GameTimer.Instance.StartTimer();
    }
}