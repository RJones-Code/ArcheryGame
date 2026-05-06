using UnityEngine;

/*
 * File: Target.cs
 *
 * Description:
 * Represents a hittable target in the VR archery game.
 * This script handles collision detection with arrows, scoring, and notifying
 * the TargetManager when the target is destroyed. It also respects game state
 * (e.g., disabling hits when the timer ends).
 *
 * Core Responsibilities:
 * - Detect collisions with arrows
 * - Award points when successfully hit
 * - Notify TargetManager to handle respawning or replacement
 * - Enable/disable whether the target can be hit based on game state
 *
 * Key Components:
 * - TargetManager (manager): Handles spawning/replacing targets
 * - Transform (spawnPoint): Location used for spawning a new target
 * - GameTimer: Controls whether targets are active
 * - ScoreManager: Tracks and updates player score
 *
 * Behavior:
 * - Start():
 *      - Subscribes to GameTimer's OnTimerEnd event to disable the target
 *
 * - OnCollisionEnter():
 *      - Checks if the colliding object is tagged as "Arrow"
 *      - If the target is active:
 *          - Adds a point via ScoreManager
 *          - Notifies TargetManager of destruction
 *          - Destroys this target object
 *
 * - DisableTarget():
 *      - Prevents the target from being hit (used when the game ends)
 *
 * - EnableTarget():
 *      - Allows the target to be hit again (used for resets/restarts)
 *
 * Dependencies:
 * - GameTimer singleton (must expose Instance and OnTimerEnd event)
 * - ScoreManager singleton (must expose AddPoint())
 * - TargetManager (must implement OnTargetDestroyed())
 *
 * Usage:
 * Attach this script to target GameObjects.
 * Assign the TargetManager and spawnPoint in the inspector.
 * Ensure arrows are tagged with "Arrow".
 */

public class Target : MonoBehaviour
{
    public TargetManager manager;
    public Transform spawnPoint;

    private bool canBeHit = true;

    private void Start()
    {
        GameTimer.Instance.OnTimerEnd.AddListener(DisableTarget);
    }

    public void DisableTarget()
    {
        canBeHit = false;
    }

    public void EnableTarget()
    {
        canBeHit = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            if (!canBeHit) return;

            ScoreManager.Instance.AddPoint();

            manager.OnTargetDestroyed(gameObject, spawnPoint);

            Destroy(gameObject);
        }
    }
}