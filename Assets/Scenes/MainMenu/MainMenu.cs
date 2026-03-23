using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public SceneFader fader;

    public void PlayGame()
    {
        fader.FadeToScene("GameScene");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
