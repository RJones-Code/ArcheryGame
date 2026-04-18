using System;
using System.Collections;
using UnityEngine;
using TMPro;

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
        DontDestroyOnLoad(gameObject);
    }

    public void CalibrateWingspan()
    {
        StartCoroutine(CalibrationRoutine());
    }

    public float GetWingspan()
    {
        if (calibratedWingspan > 0f)
            return calibratedWingspan;

        return 1.5f; // fallback default
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

        // Works even if Time.timeScale = 0
        yield return new WaitForSecondsRealtime(calibrationDelay);

        float total = 0f;

        for (int i = 0; i < sampleFrames; i++)
        {
            float distance = Vector3.Distance(leftHand.position, rightHand.position);
            total += distance;

            yield return null; // wait a frame
        }

        calibratedWingspan = total / sampleFrames;

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