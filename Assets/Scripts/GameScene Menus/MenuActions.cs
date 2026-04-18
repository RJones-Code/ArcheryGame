using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;

        SceneFader.GetOrCreate().FadeToScene("MainMenu");
    }
}