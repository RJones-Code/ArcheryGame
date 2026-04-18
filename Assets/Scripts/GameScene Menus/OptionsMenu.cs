using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public Transform playerCamera;
    public float distanceFromPlayer = 2f;
    public float heightOffset = 1.2f;

    public GameObject leftNearFar;
    public GameObject rightNearFar;
    
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

        Time.timeScale = 0f; //Freeze game while menu is open
    }

    void HideMenu()
    {
        SetUIInteractors(false);

        gameObject.SetActive(false);

        Time.timeScale = 1f; // Unfreeze game when menu is closed
    }

    void HideMenuInstant()
    {
        gameObject.SetActive(false);
    }

    void SetUIInteractors(bool state)
    {
        if (leftNearFar != null)
            leftNearFar.SetActive(state);

        if (rightNearFar != null)
            rightNearFar.SetActive(state);
    }

    void ShowInFrontOfPlayer()
    {
        // Flatten forward direction (no tilt)
        Vector3 forward = playerCamera.forward;
        forward.y = 0;
        forward.Normalize();

        // Position in front + raise height
        Vector3 position = playerCamera.position + forward * distanceFromPlayer;
        position.y += heightOffset;

        transform.position = position;

        // Face player
        transform.rotation = Quaternion.LookRotation(forward);
    }
}