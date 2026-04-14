using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        Time.timeScale = 1f;

        SceneFader.GetOrCreate().FadeToScene("GameScene");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
