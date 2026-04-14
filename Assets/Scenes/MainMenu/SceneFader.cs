using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance;

    public Image fadeImage;
    public float fadeOutDuration = 1f;
    public float fadeInDuration = 2f;

    private bool isFirstScene = true;

    void Awake()
    {
        // If instance already exists, destroy duplicate
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // SAFE ACCESS POINT
    public static SceneFader GetOrCreate()
    {
        if (Instance != null)
            return Instance;

        // Try find existing one in scene
        Instance = FindFirstObjectByType<SceneFader>();

        if (Instance != null)
            return Instance;

        // Otherwise create new one
        GameObject obj = new GameObject("SceneFader");
        Instance = obj.AddComponent<SceneFader>();

        DontDestroyOnLoad(obj);

        return Instance;
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
            isFirstScene = false;
            return;
        }

        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        float t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime / fadeOutDuration;
            SetAlpha(t);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeIn()
    {
        float t = 1f;

        while (t > 0)
        {
            t -= Time.deltaTime / fadeInDuration;
            SetAlpha(t);
            yield return null;
        }

        SetAlpha(0);
    }

    void SetAlpha(float a)
    {
        if (fadeImage == null) return;

        Color c = fadeImage.color;
        c.a = a;
        fadeImage.color = c;
    }
}