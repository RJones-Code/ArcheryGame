using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * File: ArrowController.cs
 *
 * Description:
 * Handles spawning and launching arrows in the VR archery system.
 * This script is responsible for preparing a visible arrow while the bow
 * is drawn and instantiating a physical arrow when the string is released.
 *
 * Core Responsibilities:
 * - Show/hide the preview arrow while aiming
 * - Spawn a new arrow on release
 * - Apply force to the arrow based on draw strength
 * - Play bow release sound effect
 *
 * Key Components:
 * - midPointVisual: Visual representation of the arrow while aiming
 * - arrowPrefab: Prefab of the physical arrow to be instantiated
 * - arrowSpawnPoint: Transform where the arrow is spawned
 * - AudioSource: Plays bow release sound
 *
 * Behavior:
 * - PrepareArrow(): Activates the visual arrow when the bow is being drawn
 * - ReleaseArrow(float strength):
 *      - Spawns a new arrow at the spawn point
 *      - Aligns it with the current aiming direction
 *      - Applies forward force scaled by input strength
 *      - Plays release sound and hides preview arrow
 *
 * Configuration Notes:
 * - arrowMaxSpeed controls the maximum launch force
 * - strength (0 -> 1) is expected from BowStringController
 *
 * Dependencies:
 * - Unity Physics (Rigidbody)
 * - BowStringController (provides strength value)
 *
 * Usage:
 * Attach this script to the bow object or a controller object.
 * Hook PrepareArrow() to bow pull events and ReleaseArrow() to bow release events.
 * Ensure all serialized fields are assigned in the inspector.
 */

public class ArrowController : MonoBehaviour
{
    [SerializeField]
    private GameObject midPointVisual, arrowPrefab, arrowSpawnPoint;

    [SerializeField]
    private float arrowMaxSpeed = 10;

    [SerializeField]
    private AudioSource bowReleaseAudioSource;

    public void PrepareArrow()
    {
        midPointVisual.SetActive(true);
    }

    public void ReleaseArrow(float strength)
    {
        bowReleaseAudioSource.Play();
        midPointVisual.SetActive(false);

        GameObject arrow = Instantiate(arrowPrefab);
        arrow.transform.position = arrowSpawnPoint.transform.position;
        arrow.transform.rotation = midPointVisual.transform.rotation;
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.AddForce(midPointVisual.transform.forward * strength * arrowMaxSpeed, ForceMode.Impulse);

    }
}