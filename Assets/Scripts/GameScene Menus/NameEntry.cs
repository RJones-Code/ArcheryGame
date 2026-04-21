using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NameEntry : MonoBehaviour
{
    public TMP_InputField nameInput;
    public LeaderboardRecorder recorder;
    public Button submitButton;

    bool hasSubmitted;

    public void Submit()
    {
        if (hasSubmitted || recorder == null)
            return;

        string playerName = nameInput.text;

        if (string.IsNullOrWhiteSpace(playerName))
            playerName = "Archer";

        recorder.SubmitScore(playerName);
        hasSubmitted = true;

        if (submitButton != null)
            submitButton.gameObject.SetActive(false);
    }
}