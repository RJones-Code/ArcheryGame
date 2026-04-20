using UnityEngine;
using UnityEngine.SceneManagement;

public static class TimeTrialSceneLoader
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    static void HookScenes()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!IsGameplayScene(scene))
            return;

        if (Object.FindFirstObjectByType<TimeTrialHud>(FindObjectsInactive.Include) != null)
            return;

        var root = new GameObject("GameSystems");

        if (Object.FindFirstObjectByType<ScoreManager>(FindObjectsInactive.Include) == null)
            root.AddComponent<ScoreManager>();

        if (Object.FindFirstObjectByType<GameTimer>(FindObjectsInactive.Include) == null)
            root.AddComponent<GameTimer>();

        //root.AddComponent<TimeTrialHud>();

        if (Object.FindFirstObjectByType<LeaderboardRecorder>(FindObjectsInactive.Include) == null)
            root.AddComponent<LeaderboardRecorder>();
    }

    static bool IsGameplayScene(Scene scene)
    {
        if (!scene.IsValid() || !scene.isLoaded)
            return false;

        var name = scene.name.ToLowerInvariant();
        if (name is "gamescene" or "game scene")
            return true;

        return scene.path.Replace('\\', '/').ToLowerInvariant().Contains("gamescene");
    }
}
