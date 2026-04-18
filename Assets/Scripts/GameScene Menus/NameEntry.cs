using UnityEngine;
using TMPro;

public class NameEntry : MonoBehaviour
{
    public TMP_InputField nameInput;
    public LeaderboardRecorder recorder;

    public void Submit()
    {
        string playerName = nameInput.text;

        if (string.IsNullOrWhiteSpace(playerName))
            playerName = "Archer";

        recorder.SubmitScore(playerName);
    }
}