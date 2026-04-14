using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public Transform playerCamera;
    public float distanceFromPlayer = 2f;

    public GameObject leftRayInteractor;
    public GameObject rightRayInteractor;

    private bool isVisible = false;

    void Start()
    {
        // Start hidden but DO NOT disable object itself
        HideMenuInstant();
        SetUIInteractors(false);
    }

    public void ToggleMenu()
    {
        isVisible = !isVisible;

        if (isVisible)
        {
            ShowMenu();
        }
        else
        {
            HideMenu();
        }
    }

    void ShowMenu()
    {
        gameObject.SetActive(true);

        SetUIInteractors(true);

        ShowInFrontOfPlayer();

        Time.timeScale = 0f;
    }

    void HideMenu()
    {
        SetUIInteractors(false);

        gameObject.SetActive(false);

        Time.timeScale = 1f;
    }

    void HideMenuInstant()
    {
        gameObject.SetActive(false);
    }

    void SetUIInteractors(bool state)
    {
        if (leftRayInteractor != null)
            leftRayInteractor.SetActive(state);

        if (rightRayInteractor != null)
            rightRayInteractor.SetActive(state);
    }

    void ShowInFrontOfPlayer()
    {
        transform.position = playerCamera.position + playerCamera.forward * distanceFromPlayer;

        transform.LookAt(playerCamera);
        transform.Rotate(0, 180, 0);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;

        // SAFE Scene transition (your new system)
        SceneFader.GetOrCreate().FadeToScene("MainMenu");
    }
}