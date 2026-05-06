using System.Collections.Generic;
using UnityEngine;

/*
 * File: TargetManager.cs
 *
 * Description:
 * Manages the spawning, tracking, and replacement of targets in the VR archery game.
 * Ensures a consistent number of active targets while preventing duplicate spawn usage
 * and respecting game state (e.g., stopping when the timer ends).
 *
 * Core Responsibilities:
 * - Spawn an initial set of targets at game start
 * - Maintain a maximum number of active targets
 * - Randomly select spawn points while avoiding reuse
 * - Choose appropriate target types based on spawn location
 * - Replace destroyed targets dynamically
 * - Stop spawning when the game ends
 *
 * Key Components:
 * - sittingTargetPrefab: Prefab used for low-row targets
 * - floatingTargetPrefab: Prefab used for mid/high-row targets
 * - lowRow / midRow / highRow: Parent transforms containing spawn points
 *
 * Behavior:
 * - Start():
 *      - Spawns initial targets up to maxTargets
 *      - Subscribes to GameTimer event to stop spawning
 *
 * - SpawnTarget():
 *      - Selects a valid spawn point
 *      - Chooses correct prefab based on row
 *      - Instantiates target and assigns dependencies
 *      - Tracks active targets and used spawn points
 *
 * - GetRandomSpawnPoint():
 *      - Gathers all spawn points
 *      - Filters out currently used ones
 *      - Falls back to all spawns if none are available
 *
 * - OnTargetDestroyed():
 *      - Removes target and frees its spawn point
 *      - Spawns a replacement target (if allowed)
 *
 * - StopSpawning():
 *      - Disables further spawning when the game ends
 *
 * Dependencies:
 * - Target script (must expose manager and spawnPoint fields)
 * - GameTimer singleton (must expose OnTimerEnd event)
 *
 * Usage:
 * Attach this script to a scene manager object.
 * Assign prefabs and spawn row transforms in the inspector.
 * Ensure each row contains child transforms representing spawn positions.
 */

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