using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/*
 * File: GameOverManager.cs
 *
 * Description:
 * Handles the game-over state UI. Displays the final score, positions the
 * game-over canvas in front of the player in VR, and focuses the name input
 * field for leaderboard submission when the timer ends.
 *
 * Core Responsibilities:
 * - Listen for game end event from GameTimer
 * - Display and position the game-over UI in front of the player
 * - Show the final score
 * - Focus input field for player name entry
 *
 * Key Components:
 * - gameOverCanvas: UI canvas shown at the end of the game
 * - playerCamera: Used to position the UI relative to the player
 * - scoreText: Displays the final score
 * - nameInputField: Input field for entering player name
 *
 * Behavior:
 * - Start():
 *      - Hides the game-over UI
 *      - Subscribes to GameTimer.OnTimerEnd event
 *
 * - ShowGameOver():
 *      - Activates the game-over canvas
 *      - Positions it in front of the player
 *      - Updates score text
 *      - Focuses the input field for immediate typing
 *
 * - ShowInFrontOfPlayer():
 *      - Positions the canvas at a fixed distance in front of the player
 *      - Aligns rotation to face the player horizontally
 *
 * Dependencies:
 * - GameTimer singleton (must expose OnTimerEnd)
 * - ScoreManager singleton (must expose Score)
 * - Unity UI EventSystem
 * - TextMeshPro (TMP_Text, TMP_InputField)
 *
 * Usage:
 * Attach this script to a manager object in the scene.
 * Assign references in the inspector.
 * Ensure GameTimer exists and triggers OnTimerEnd.
 */

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    public Transform playerCamera;
    public float distanceFromPlayer = 2f;
    public float heightOffset = 1.2f;

    public TMP_Text scoreText;

    public TMP_InputField nameInputField;

    void Start()
    {
        gameOverCanvas.SetActive(false);

        GameTimer.Instance.OnTimerEnd.AddListener(ShowGameOver);
    }

    void ShowGameOver()
    {
        gameOverCanvas.SetActive(true);

        ShowInFrontOfPlayer();

        // Set score
        scoreText.text = "Score: " + ScoreManager.Instance.Score;

        nameInputField.ActivateInputField();
        EventSystem.current.SetSelectedGameObject(nameInputField.gameObject);
    }

    void ShowInFrontOfPlayer()
    {
        Transform canvasTransform = gameOverCanvas.transform;

        // Flatten forward direction (no tilt)
        Vector3 forward = playerCamera.forward;
        forward.y = 0;
        forward.Normalize();

        // Position in front + raise height
        Vector3 position = playerCamera.position + forward * distanceFromPlayer;
        position.y += heightOffset;

        canvasTransform.position = position;

        // Face player
        canvasTransform.rotation = Quaternion.LookRotation(forward);
    }
}