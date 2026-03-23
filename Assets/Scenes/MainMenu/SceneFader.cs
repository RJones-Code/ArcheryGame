using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeOutDuration = 1f;
    public float fadeInDuration = 2f;

    private bool isFirstScene = true;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isFirstScene)
        {
            // Skip fade-in the very first scene
            isFirstScene = false;
            return;
        }

        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    IEnumerator FadeIn()
    {
        float t = 1f;
        while (t > 0)
        {
            t -= Time.deltaTime / fadeInDuration;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / fadeOutDuration;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}