using UnityEngine;

/*
 * File: ScoreManager.cs
 *
 * Description:
 * Manages the player's score during gameplay.
 * Implements a singleton pattern to provide global access to the current score
 * and exposes methods for modifying and resetting it.
 *
 * Core Responsibilities:
 * - Track the current player score
 * - Provide a global access point via singleton pattern
 * - Increment score when targets are hit
 * - Reset score between rounds
 *
 * Key Components:
 * - Score: Current player score (read-only externally)
 *
 * Behavior:
 * - AddPoint():
 *      - Increments the score by 1
 *      - Logs updated score for debugging
 *
 * - ResetScore():
 *      - Resets score to 0 (used at game restart)
 *
 * Dependencies:
 * - None directly, but used by:
 *      - Target (to award points)
 *      - GameTimer (for final score logging)
 *      - LeaderboardRecorder (to submit final score)
 *
 * Usage:
 * Attach this script to a persistent GameObject in the scene.
 * Access via ScoreManager.Instance.
 * Call AddPoint() when a target is successfully hit.
 */

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int Score { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddPoint()
    {
        Score++;
        Debug.Log($"Score: {Score}");
    }

    public void ResetScore()
    {
        Score = 0;
    }
}
