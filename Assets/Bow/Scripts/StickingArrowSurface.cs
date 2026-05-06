using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * File: StickingArrowToSurface.cs
 *
 * Description:
 * Handles arrow collision behavior. When an arrow hits a surface, it either:
 * - Delegates to a target (for scoring/destruction), or
 * - Sticks into the surface by spawning a static arrow and removing the physics-driven one.
 *
 * Core Responsibilities:
 * - Detect collisions with targets and non-target surfaces
 * - Stop arrow physics upon impact
 * - Replace the moving arrow with a "stuck" version
 * - Align the stuck arrow with the impact direction
 * - Parent the arrow to moving objects when applicable
 *
 * Key Components:
 * - Rigidbody (rb): Controls arrow physics
 * - SphereCollider (myCollider): Handles collision detection
 * - stickingArrow: Prefab used for the embedded/stuck arrow
 *
 * Behavior:
 * - OnCollisionEnter():
 *      - If the object hit has a Target component:
 *          - Destroy this arrow and let the target handle logic (e.g., scoring)
 *      - Otherwise:
 *          - Disable physics (set Rigidbody to kinematic)
 *          - Convert collider to trigger to avoid further collisions
 *          - Spawn a "sticking" arrow prefab at the impact point
 *          - Align it with the current forward direction
 *          - Slightly offset it forward to simulate penetration
 *          - Parent it to the hit object if it has a Rigidbody
 *          - Destroy the original arrow object
 *
 * Configuration Notes:
 * - pushInAmount controls how far the arrow embeds into surfaces
 * - stickingArrow should be a non-physics visual prefab
 *
 * Dependencies:
 * - Target script (used to detect valid targets)
 * - Unity Physics system
 *
 * Usage:
 * Attach this script to the arrow prefab.
 * Assign Rigidbody, Collider, and stickingArrow prefab in the inspector.
 */

public class StickingArrowToSurface : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private SphereCollider myCollider;
    [SerializeField]
    private GameObject stickingArrow;

    //private Vector3 lastVelocity;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Target>() != null)
        {
            // Let target handle score/destruction
            Destroy(gameObject);
            return;
        }

        rb.isKinematic = true;
        myCollider.isTrigger = true;

        GameObject arrow = Instantiate(stickingArrow);

        // Place exactly where the collision happened
        arrow.transform.position = transform.position;
        arrow.transform.forward = transform.forward;

        // small offset so it doesn't clip awkwardly
        float pushInAmount = 0.75f;
        arrow.transform.position += arrow.transform.forward * pushInAmount;

        if (collision.collider.attachedRigidbody != null)
        {
            arrow.transform.parent = collision.collider.attachedRigidbody.transform;
        }

        Destroy(gameObject);

    }
}