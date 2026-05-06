using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * File: BowString.cs
 *
 * Description:
 * Renders the bowstring using a LineRenderer.
 * This script dynamically updates the string shape based on whether
 * the bow is idle (2 points) or being pulled (3 points with a midpoint).
 *
 * Core Responsibilities:
 * - Maintain and update the visual representation of the bowstring
 * - Support both relaxed and pulled states of the string
 * - Convert world-space midpoint into local space for proper rendering
 *
 * Key Components:
 * - LineRenderer: Used to visually draw the bowstring
 * - endpoint_1 / endpoint_2: Fixed ends of the bowstring
 *
 * Behavior:
 * - CreateString(Vector3? midPosition):
 *      - If midPosition is null -> renders a straight string (2 points)
 *      - If midPosition is provided -> inserts a midpoint (3 points)
 *      - Converts midpoint from world space to local space
 *
 * - Start():
 *      - Initializes the bowstring in its default (unpulled) state
 *
 * Dependencies:
 * - Requires a LineRenderer component on the same GameObject
 *
 * Usage:
 * Attach this script to the bow object with a LineRenderer.
 * Assign both endpoints in the inspector.
 * Call CreateString() from BowStringController to update the string during interaction.
 */

[RequireComponent(typeof(LineRenderer))]
public class BowString : MonoBehaviour
{
    [SerializeField]
    private Transform endpoint_1, endpoint_2;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void CreateString(Vector3? midPosition)
    {
        Vector3[] linePoints = new Vector3[midPosition == null ? 2 : 3];
        linePoints[0] = endpoint_1.localPosition;
        if (midPosition != null)
        {
            linePoints[1] = transform.InverseTransformPoint(midPosition.Value);
        }
        linePoints[^1] = endpoint_2.localPosition;

        lineRenderer.positionCount = linePoints.Length;
        lineRenderer.SetPositions(linePoints);
    }

    private void Start()
    {
        CreateString(null);
    }
}