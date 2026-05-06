using UnityEngine;
using TMPro;
using UnityEngine.UI;

/*
 * File: NameEntry.cs
 *
 * Description:
 * Handles player name input and submission to the leaderboard system.
 * Prevents duplicate submissions and validates user input before passing
 * the data to the LeaderboardRecorder.
 *
 * Core Responsibilities:
 * - Read player name from UI input field
 * - Validate and sanitize input
 * - Submit score to leaderboard via LeaderboardRecorder
 * - Prevent multiple submissions
 * - Disable submit UI after successful entry
 *
 * Key Components:
 * - TMP_InputField (nameInput): Field where player enters their name
 * - LeaderboardRecorder (recorder): Handles score submission
 * - Button (submitButton): UI button used to trigger submission
 *
 * Behavior:
 * - Submit():
 *      - Prevents duplicate submissions using hasSubmitted flag
 *      - Retrieves player name from input field
 *      - Falls back to default name if input is empty
 *      - Submits score through LeaderboardRecorder
 *      - Disables submit button after successful submission
 *
 * Dependencies:
 * - TextMeshPro (TMP_InputField)
 * - Unity UI (Button)
 * - LeaderboardRecorder (must expose SubmitScore)
 *
 * Usage:
 * Attach this script to a UI object in the end-game screen.
 * Assign input field, submit button, and recorder in the inspector.
 * Hook Submit() to the button’s OnClick() event.
 */

public class NameEntry : MonoBehaviour
{
    public TMP_InputField nameInput;
    public LeaderboardRecorder recorder;
    public Button submitButton;

    bool hasSubmitted;

    public void Submit()
    {
        if (hasSubmitted || recorder == null)
            return;

        string playerName = nameInput.text;

        if (string.IsNullOrWhiteSpace(playerName))
            playerName = "Archer";

        recorder.SubmitScore(playerName);
        hasSubmitted = true;

        if (submitButton != null)
            submitButton.gameObject.SetActive(false);
    }
}