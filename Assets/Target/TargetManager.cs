using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject sittingTargetPrefab;
    public GameObject floatingTargetPrefab;

    public Transform lowRow;
    public Transform midRow;
    public Transform highRow;

    public int maxTargets = 3;

    private List<GameObject> activeTargets = new List<GameObject>();
    private List<Transform> usedSpawns = new List<Transform>();

    private bool spawningEnabled = true;

    void Start()
    {
        SpawnInitialTargets();

        GameTimer.Instance.OnTimerEnd.AddListener(StopSpawning);
    }

    void StopSpawning()
    {
        spawningEnabled = false;
    }

    void SpawnInitialTargets()
    {
        for (int i = 0; i < maxTargets; i++)
        {
            SpawnTarget();
        }
    }

    public void SpawnTarget()
    {
        if (!spawningEnabled) return;

        Transform spawn = GetRandomSpawnPoint();

        GameObject prefabToUse = GetPrefabForSpawn(spawn);

        GameObject target = Instantiate(
            prefabToUse,
            spawn.position,
            spawn.rotation
        );

        Target targetScript = target.GetComponent<Target>();
        targetScript.manager = this;
        targetScript.spawnPoint = spawn;

        activeTargets.Add(target);
        usedSpawns.Add(spawn);
    }

    Transform GetRandomSpawnPoint()
    {
        List<Transform> allSpawns = new List<Transform>();

        foreach (Transform t in lowRow) allSpawns.Add(t);
        foreach (Transform t in midRow) allSpawns.Add(t);
        foreach (Transform t in highRow) allSpawns.Add(t);

        List<Transform> available = new List<Transform>(allSpawns);

        foreach (var used in usedSpawns)
        {
            available.Remove(used);
        }

        if (available.Count == 0)
            available = allSpawns;

        return available[Random.Range(0, available.Count)];
    }

    GameObject GetPrefabForSpawn(Transform spawn)
    {
        if (spawn.parent == lowRow)
            return sittingTargetPrefab;

        return floatingTargetPrefab;
    }

    public void OnTargetDestroyed(GameObject target, Transform spawn)
    {
        activeTargets.Remove(target);
        usedSpawns.Remove(spawn);

        SpawnTarget();
    }
}