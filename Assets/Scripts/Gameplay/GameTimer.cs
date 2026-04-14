using UnityEngine;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    [SerializeField] private float roundDuration = 60f;
    public float TimeRemaining { get; private set; }
    public bool IsRunning { get; private set; }

    public UnityEvent OnTimerStart;
    public UnityEvent OnTimerEnd;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (!IsRunning) return;

        TimeRemaining -= Time.deltaTime;

        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            IsRunning = false;
            OnTimerEnd?.Invoke();
            Debug.Log($"Time's up! Final Score: {ScoreManager.Instance.Score}");
        }
    }

    public void StartTimer()
    {
        if (IsRunning)
            return;

        TimeRemaining = roundDuration;
        IsRunning = true;
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
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(TimeRemaining / 60f);
        int seconds = Mathf.FloorToInt(TimeRemaining % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}
