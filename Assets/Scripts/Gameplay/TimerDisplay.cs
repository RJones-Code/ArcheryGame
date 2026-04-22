using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    public TMP_Text timerText;

    void Update()
    {
        if (GameTimer.Instance == null) return;

        var timer = GameTimer.Instance;

        // Show countdown (3,2,1)
        if (timer.IsCountingDown)
        {
            timerText.fontSize = 50;
            timerText.color = Color.yellow;

            timerText.gameObject.SetActive(true);
            timerText.text = Mathf.Ceil(timer.CountdownTime).ToString();
            return;
        }

        // Show main timer
        if (timer.IsRunning)
        {
            timerText.fontSize = 25;
            timerText.color = Color.white;

            timerText.gameObject.SetActive(true);
            timerText.text = timer.GetFormattedTime();
            return;
        }

        // Hide when not active (optional)
        timerText.gameObject.SetActive(false);
    }
}