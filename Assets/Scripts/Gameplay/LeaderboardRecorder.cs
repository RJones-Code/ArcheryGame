using UnityEngine;

/*
 * File: LeaderboardRecorder.cs
 *
 * Description:
 * Handles submitting the player's score to the local leaderboard system.
 * Acts as a bridge between gameplay (ScoreManager) and persistent storage
 * (LeaderboardDatabase).
 *
 * Core Responsibilities:
 * - Retrieve the current player score
 * - Submit score entries to the leaderboard database
 * - Provide a simple interface for UI or game flow to trigger score saving
 *
 * Key Components:
 * - ScoreManager: Provides the current score
 * - LeaderboardDatabase: Handles storage and ranking of scores
 *
 * Behavior:
 * - SubmitScore(string playerName):
 *      - Retrieves the current score from ScoreManager
 *      - Falls back to 0 if ScoreManager is unavailable
 *      - Submits the score and player name to the leaderboard
 *      - Logs the saved result for debugging
 *
 * Dependencies:
 * - ScoreManager singleton (must expose Instance and Score)
 * - LeaderboardDatabase (static storage system)
 *
 * Usage:
 * Attach this script to a UI controller or game manager object.
 * Call SubmitScore() when the game ends (e.g., from GameTimer.OnTimerEnd).
 * Typically triggered by a UI input field or end screen.
 */

public class LeaderboardRecorder : MonoBehaviour
{
    public void SubmitScore(string playerName)
    {
        int score = ScoreManager.Instance != null ? ScoreManager.Instance.Score : 0;

        LeaderboardDatabase.AddEntry(playerName, score);

        Debug.Log($"Saved Score: {playerName} - {score}");
    }
}
