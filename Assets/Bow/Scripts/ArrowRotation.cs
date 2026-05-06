using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * File: ArrowRotation.cs
 *
 * Description:
 * Aligns the arrow's forward direction with its velocity during flight.
 * This creates realistic arrow behavior where the tip of the arrow points
 * in the direction it is traveling.
 *
 * Core Responsibilities:
 * - Continuously update arrow orientation based on Rigidbody velocity
 * - Smoothly interpolate rotation for natural motion
 *
 * Key Components:
 * - Rigidbody (rb): Provides the current velocity of the arrow
 *
 * Behavior:
 * - In FixedUpdate(), the arrow's forward vector is smoothly rotated
 *   toward the direction of its velocity using spherical interpolation (Slerp)
 *
 * Configuration Notes:
 * - Requires a valid Rigidbody reference
 * - Works best when the arrow is affected by physics (gravity, drag, etc.)
 *
 * Dependencies:
 * - Unity Physics (Rigidbody)
 *
 * Usage:
 * Attach this script to the arrow prefab.
 * Ensure the Rigidbody is assigned in the inspector.
 */

public class ArrowRotation : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    private void FixedUpdate()
    {
        transform.forward =
            Vector3.Slerp(transform.forward, rb.linearVelocity.normalized, Time.fixedDeltaTime);
    }

}