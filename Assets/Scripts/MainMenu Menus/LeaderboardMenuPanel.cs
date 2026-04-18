using System.Text;
using TMPro;
using UnityEngine;

public class LeaderboardMenuPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoresText;

    public void BindScoresText(TextMeshProUGUI text)
    {
        scoresText = text;
    }

    void OnEnable()
    {
        RefreshScores();
    }

    public void RefreshScores()
    {
        if (scoresText == null)
            scoresText = GetComponentInChildren<TextMeshProUGUI>(true);

        if (scoresText == null)
            return;

        var top = LeaderboardDatabase.GetTop(15);
        var sb = new StringBuilder();
        if (top.Count == 0)
            sb.Append("No scores yet.\nPlay a round to record one.");
        else
        {
            for (var i = 0; i < top.Count; i++)
            {
                var e = top[i];
                sb.AppendLine($"{i + 1}. {e.playerName,-14}  {e.score}");
            }
        }

        scoresText.text = sb.ToString().TrimEnd();
        scoresText.enableWordWrapping = true;
    }

    public void BackToMainMenu()
    {
        var menu = Object.FindFirstObjectByType<MainMenu>(FindObjectsInactive.Include);
        if (menu != null)
            menu.CloseLeaderboard();
    }
}
