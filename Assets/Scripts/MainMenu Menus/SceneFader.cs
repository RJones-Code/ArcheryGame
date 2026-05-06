using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/*
 * File: SceneFader.cs
 *
 * Description:
 * Manages smooth scene transitions using fade-in and fade-out effects.
 * Implements a persistent singleton that survives across scenes and provides
 * a centralized API for loading scenes with visual transitions.
 *
 * Core Responsibilities:
 * - Provide fade-out transition before scene loading
 * - Provide fade-in transition after scene load
 * - Persist across scenes using DontDestroyOnLoad
 * - Offer global access via singleton pattern (GetOrCreate)
 *
 * Key Components:
 * - fadeImage: UI Image used as fullscreen fade overlay
 * - SceneManager: Handles Unity scene loading events
 *
 * Behavior:
 * - Awake():
 *      - Ensures only one instance exists (singleton enforcement)
 *      - Persists object across scene loads
 *
 * - GetOrCreate():
 *      - Returns existing instance if available
 *      - Finds or creates SceneFader if missing
 *
 * - FadeToScene(string sceneName):
 *      - Starts fade-out animation
 *      - Loads target scene after fade completes
 *
 * - OnSceneLoaded():
 *      - Triggers fade-in after scene transition
 *      - Skips first scene load to avoid unwanted fade-in
 *
 * - FadeOutAndLoad():
 *      - Gradually increases alpha of fade overlay
 *      - Loads new scene when fully opaque
 *
 * - FadeIn():
 *      - Gradually decreases alpha to reveal scene
 *
 * - SetAlpha():
 *      - Applies transparency value to fade image
 *
 * Dependencies:
 * - UnityEngine.SceneManagement
 * - Unity UI (Image component)
 *
 * Usage:
 * Attach this script to a persistent GameObject with a full-screen Image.
 * Call SceneFader.GetOrCreate().FadeToScene("SceneName") to transition scenes.
 */

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
        Instance = FindAnyObjectByType<SceneFader>();

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