using UnityEngine;
using UnityEngine.Events;

/*
 * File: GameTimer.cs
 *
 * Description:
 * Manages the overall game timing system, including a pre-game countdown,
 * active round timing, and end-of-game state. This script acts as a central
 * controller for game flow and exposes events for other systems to react to.
 *
 * Core Responsibilities:
 * - Handle countdown before gameplay begins
 * - Track remaining time during the round
 * - Manage game state (running, countdown, game over)
 * - Notify other systems via events (start, end, countdown)
 * - Provide formatted time for UI display
 *
 * Key Components:
 * - roundDuration: Total duration of the game round
 * - countdownStart: Duration of the pre-game countdown
 * - TimeRemaining: Current time left in the round
 * - CountdownTime: Current time left in countdown
 *
 * Game States:
 * - IsCountingDown: Countdown is active before gameplay starts
 * - IsRunning: Game timer is actively counting down
 * - IsGameOver: Game has ended
 *
 * Events:
 * - OnTimerStart: Invoked when the timer is initialized
 * - OnCountdownStart: Invoked when countdown finishes and gameplay begins
 * - OnTimerEnd: Invoked when the timer reaches zero
 *
 * Behavior:
 * - StartTimer():
 *      - Initializes countdown and round duration
 *      - Prevents restarting if already active
 *
 * - Update():
 *      - Handles countdown phase first
 *      - Then processes active game timer
 *      - Triggers end-of-game logic when time runs out
 *
 * - StopTimer():
 *      - Pauses the timer without resetting state
 *
 * - ResetTimer():
 *      - Resets all timing and state variables
 *
 * - GetFormattedTime():
 *      - Returns remaining time as a MM:SS string for UI display
 *
 * Dependencies:
 * - ScoreManager (used for final score logging)
 *
 * Usage:
 * Attach this script to a persistent GameObject in the scene.
 * Access via GameTimer.Instance.
 * Subscribe to events for UI, spawning, and gameplay reactions.
 */

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    [SerializeField] private float roundDuration = 60f;
    public float TimeRemaining { get; private set; }
    public bool IsRunning { get; private set; }

    public UnityEvent OnCountdownStart;
    public UnityEvent OnTimerStart;
    public UnityEvent OnTimerEnd;

    public bool IsGameOver { get; private set; }

    public bool IsCountingDown { get; private set; }
    public float CountdownTime { get; private set; }

    [SerializeField] private float countdownStart = 3f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (IsCountingDown)
        {
            CountdownTime -= Time.deltaTime;

            if (CountdownTime <= 0f)
            {
                IsCountingDown = false;
                IsRunning = true;
                CountdownTime = 0f;

                OnCountdownStart?.Invoke();
            }

            return;
        }

        if (!IsRunning) return;

        TimeRemaining -= Time.deltaTime;

        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            IsRunning = false;
            IsGameOver = true;
            OnTimerEnd?.Invoke();
            Debug.Log($"Time's up! Final Score: {ScoreManager.Instance.Score}");
        }
    }

    public void StartTimer()
    {
        if (IsRunning || IsCountingDown)
            return;

        TimeRemaining = roundDuration;
        IsGameOver = false;
        IsCountingDown = true;
        IsRunning = true;

        CountdownTime = countdownStart;

        OnTimerStart?.Invoke();
    }

    public void StopTimer()
    {
        IsRunning = false;
    }

    public void ResetTimer()
    {
        TimeRemaining = roundDuration;

        IsRunning = false;
        IsCountingDown = false;
        IsGameOver = false;

        CountdownTime = 0f;
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(TimeRemaining / 60f);
        int seconds = Mathf.FloorToInt(TimeRemaining % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}
