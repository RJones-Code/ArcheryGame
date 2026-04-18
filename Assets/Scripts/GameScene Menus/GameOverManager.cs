using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    public Transform playerCamera;
    public float distanceFromPlayer = 2f;
    public float heightOffset = 1.2f;

    public TMP_Text scoreText;

    public TMP_InputField nameInputField;

    void Start()
    {
        gameOverCanvas.SetActive(false);

        GameTimer.Instance.OnTimerEnd.AddListener(ShowGameOver);
    }

    void ShowGameOver()
    {
        gameOverCanvas.SetActive(true);

        ShowInFrontOfPlayer();

        // Set score
        scoreText.text = "Score: " + ScoreManager.Instance.Score;

        nameInputField.ActivateInputField();
        EventSystem.current.SetSelectedGameObject(nameInputField.gameObject);
    }

    void ShowInFrontOfPlayer()
    {
        Transform canvasTransform = gameOverCanvas.transform;

        // Flatten forward direction (no tilt)
        Vector3 forward = playerCamera.forward;
        forward.y = 0;
        forward.Normalize();

        // Position in front + raise height
        Vector3 position = playerCamera.position + forward * distanceFromPlayer;
        position.y += heightOffset;

        canvasTransform.position = position;

        // Face player
        canvasTransform.rotation = Quaternion.LookRotation(forward);
    }
}