using UnityEngine;

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
