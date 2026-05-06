using UnityEngine;
using TMPro;

/*
 * File: TimerDisplay.cs
 *
 * Description:
 * Handles the visual display of the game timer and countdown using TextMeshPro.
 * Dynamically updates the UI based on the current state of the GameTimer,
 * including countdown, active gameplay timer, and hidden state.
 *
 * Core Responsibilities:
 * - Display countdown before the game starts (3, 2, 1)
 * - Display formatted time during gameplay
 * - Show/hide UI elements based on timer state
 * - Adjust visual styling (font size, color) for different phases
 *
 * Key Components:
 * - TMP_Text (timerText): Displays countdown numbers and time
 * - GameObject (backdrop): Optional background UI for visibility
 *
 * Behavior:
 * - Update():
 *      - If GameTimer is counting down:
 *          - Displays countdown numbers in large yellow text
 *      - If GameTimer is running:
 *          - Displays formatted time (MM:SS) in standard style
 *      - Otherwise:
 *          - Hides the timer UI
 *
 * Dependencies:
 * - GameTimer singleton (must expose state flags and timing values)
 * - TextMeshPro (TMP_Text component)
 *
 * Usage:
 * Attach this script to a UI GameObject.
 * Assign the TMP_Text and optional backdrop in the inspector.
 * Ensure GameTimer exists in the scene.
 */

public class TimerDisplay : MonoBehaviour
{
    public TMP_Text timerText;
    public GameObject backdrop;

    void Update()
    {
        if (GameTimer.Instance == null) return;

        var timer = GameTimer.Instance;

        // Show countdown (3,2,1)
        if (timer.IsCountingDown)
        {
            timerText.fontSize = 90;
            timerText.color = Color.yellow;

            timerText.gameObject.SetActive(true);
            backdrop.SetActive(true);
            timerText.text = Mathf.Ceil(timer.CountdownTime).ToString();
            return;
        }

        // Show main timer
        if (timer.IsRunning)
        {
            timerText.fontSize = 75;
            timerText.color = Color.white;

            timerText.gameObject.SetActive(true);
            backdrop.SetActive(true);
            timerText.text = timer.GetFormattedTime();
            return;
        }

        // Hide when not active (optional)
        timerText.gameObject.SetActive(false);
        backdrop.SetActive(false);
    }
}