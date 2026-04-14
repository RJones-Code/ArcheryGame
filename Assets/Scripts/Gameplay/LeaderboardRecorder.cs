using UnityEngine;

public class LeaderboardRecorder : MonoBehaviour
{
    void Start()
    {
        var timer = GameTimer.Instance;
        if (timer != null)
            timer.OnTimerEnd.AddListener(OnRoundEnd);
    }

    void OnDestroy()
    {
        var timer = GameTimer.Instance;
        if (timer != null)
            timer.OnTimerEnd.RemoveListener(OnRoundEnd);
    }

    void OnRoundEnd()
    {
        int score = ScoreManager.Instance != null ? ScoreManager.Instance.Score : 0;
        LeaderboardDatabase.AddEntry(LeaderboardDatabase.PlayerName, score);
    }
}
