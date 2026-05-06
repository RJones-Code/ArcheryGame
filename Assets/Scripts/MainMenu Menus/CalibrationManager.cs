using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/*
 * File: CalibrationManager.cs
 *
 * Description:
 * Handles player physical calibration for VR gameplay by measuring
 * the player's wingspan (distance between hands). This value is used
 * to scale gameplay interactions such as bow draw distance, ensuring
 * consistent feel across different players.
 *
 * Core Responsibilities:
 * - Measure real-world distance between VR controllers (hands)
 * - Compute and store calibrated wingspan value
 * - Persist calibration data using PlayerPrefs
 * - Provide events for systems that depend on calibration updates
 * - Manage optional calibration UI feedback
 *
 * Key Components:
 * - leftHand / rightHand:
 *      VR hand transforms used for distance measurement
 *
 * - calibratedWingspan:
 *      Final computed wingspan used by gameplay systems
 *
 * - WingspanKey:
 *      PlayerPrefs key for persistent storage
 *
 * Behavior:
 * - CalibrateWingspan():
 *      Starts calibration coroutine
 *
 * - CalibrationRoutine():
 *      1. Shows calibration UI (if assigned)
 *      2. Waits for player to stabilize arms
 *      3. Samples hand distance over multiple frames
 *      4. Computes average wingspan
 *      5. Saves result to PlayerPrefs
 *      6. Triggers OnCalibrationUpdated event
 *      7. Displays completion UI feedback
 *
 * - GetWingspan():
 *      Returns runtime-calibrated value or saved fallback default
 *
 * Dependencies:
 * - VR tracking (hand transforms)
 * - Unity PlayerPrefs for persistence
 * - TMPro for UI display
 *
 * Usage:
 * Call CalibrationManager.Instance.CalibrateWingspan()
 * at game start or in settings menu.
 */

public class CalibrationManager : MonoBehaviour
{
    public static CalibrationManager Instance;

    [Header("Hand References")]
    public Transform leftHand;
    public Transform rightHand;

    [Header("Calibration Settings")]
    public float calibrationDelay = 1f;
    public int sampleFrames = 30;

    [Header("Optional UI")]
    public GameObject calibrationUI; // "Hold arms out..." panel
    public GameObject completionUI;  // "Calibration complete!" panel
    public TMP_Text completionText;

    public float calibratedWingspan { get; private set; }

    public event Action OnCalibrationUpdated;

    private const string WingspanKey = "Wingspan";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void CalibrateWingspan()
    {
        StartCoroutine(CalibrationRoutine());
    }

    public float GetWingspan()
    {
        if (calibratedWingspan > 0f)
            return calibratedWingspan;

        return PlayerPrefs.GetFloat(WingspanKey, 1.5f);
    }

    private IEnumerator CalibrationRoutine()
    {
        if (completionUI != null) 
            completionUI.SetActive(false);

        if (leftHand == null || rightHand == null)
        {
            Debug.LogError("Calibration failed: Hand references not assigned.");
            yield break;
        }

        if (calibrationUI != null)
            calibrationUI.SetActive(true);

        Debug.Log("Hold your arms out steady...");

        yield return new WaitForSecondsRealtime(calibrationDelay);

        float total = 0f;

        for (int i = 0; i < sampleFrames; i++)
        {
            float distance = Vector3.Distance(leftHand.position, rightHand.position);
            total += distance;

            yield return null; // wait a frame
        }

        calibratedWingspan = total / sampleFrames;

        PlayerPrefs.SetFloat(WingspanKey, calibratedWingspan);
        PlayerPrefs.Save();

        if (calibrationUI != null)
            calibrationUI.SetActive(false);

        if (completionUI != null)
        {
            completionUI.SetActive(true);

            completionText.text =
                $"Calibration Complete!\nWingspan: {calibratedWingspan:F2} m";
        }

        Debug.Log("Calibration complete: " + calibratedWingspan);

        OnCalibrationUpdated?.Invoke();
    }
}