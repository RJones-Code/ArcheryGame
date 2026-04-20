using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    public TMP_Text timerText;

    void Update()
    {
        if (GameTimer.Instance == null) return;

        bool running = GameTimer.Instance.IsRunning;

        if (timerText.gameObject.activeSelf != running)
            timerText.gameObject.SetActive(running);

        timerText.text = GameTimer.Instance.GetFormattedTime();
    }
}