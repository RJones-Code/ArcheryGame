using UnityEngine;
using TMPro;

public class NameEntry : MonoBehaviour
{
    public TMP_InputField inputField;

    public void SubmitName()
    {
        string playerName = inputField.text;

        Debug.Log("Player Name: " + playerName);
        Debug.Log("Score: " + ScoreManager.Instance.Score);

        // Later: save to leaderboard
    }
}