using UnityEngine;
using UnityEngine.Events;

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
