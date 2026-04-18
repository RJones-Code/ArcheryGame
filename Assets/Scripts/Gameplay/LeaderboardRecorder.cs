using UnityEngine;

public class LeaderboardRecorder : MonoBehaviour
{
    public void SubmitScore(string playerName)
    {
        int score = ScoreManager.Instance != null ? ScoreManager.Instance.Score : 0;

        LeaderboardDatabase.AddEntry(playerName, score);

        Debug.Log($"Saved Score: {playerName} - {score}");
    }
}
