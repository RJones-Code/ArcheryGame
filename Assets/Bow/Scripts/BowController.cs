using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/*
 * File: BowStringController.cs
 * 
 * Description:
 * Controls the interaction and behavior of the bowstring in the VR archery system.
 * This script handles grabbing, pulling, and releasing the bowstring using XR interaction.
 * It calculates the draw strength based on how far the string is pulled and triggers
 * corresponding events and audio feedback.
 *
 * Core Responsibilities:
 * - Detect when the bowstring is grabbed and released 
 * - Track the interactor (player hand/controller) position
 * - Calculate bow draw strength (0 -> 1) based on pull distance
 * - Clamp pull distance to a configurable maximum
 * - Update the visual position of the bowstring
 * - Play audio feedback based on pull movement and direction
 * - Invoke events when the bow is pulled and released
 *
 * Key Components:
 * - BowString (bowStringRenderer): Handles rendering of the bowstring
 * - XRGrabInteractable (midPointGrabObject): Enables grabbing interaction
 * - midPointVisualObject: Visual representation of the string midpoint
 * - midPointParent: Reference space for calculating local pull distance
 * - AudioSource: Plays string tension sound effects
 *
 * Events:
 * - OnBowPulled: Invoked when the string is initially grabbed
 * - OnBowReleased(float strength): Invoked when released, passing final draw strength
 *
 * Configuration Notes:
 * - bowStringStretchLimit is dynamically calibrated using player wingspan
 * - drawMultiplier controls how far the string can be pulled relative to wingspan
 * - stringSoundThreshold determines sensitivity of audio playback
 *
 * Dependencies:
 * - Unity XR Interaction Toolkit
 * - BowString renderer script
 *
 * Usage:
 * Attach this script to the bowstring midpoint object.
 * Ensure all serialized references are properly assigned in the inspector.
 */

public class BowStringController : MonoBehaviour
{
    [SerializeField]
    private BowString bowStringRenderer;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactable;

    [SerializeField]
    private Transform midPointGrabObject, midPointVisualObject, midPointParent;

    [SerializeField]
    private float bowStringStretchLimit = 0.3f;

    [SerializeField]
    private float drawMultiplier = 0.5f;

    private Transform interactor;

    private float strength, previousStrength;

    [SerializeField]
    private float stringSoundThreshold = 0.001f;

    [SerializeField]
    private AudioSource audioSource;

    public UnityEvent OnBowPulled;
    public UnityEvent<float> OnBowReleased;



    private void Awake()
    {
        interactable = midPointGrabObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    private void Start()
    {
        interactable.selectEntered.AddListener(PrepareBowString);
        interactable.selectExited.AddListener(ResetBowString);

        ApplyCalibration();
    }

    private void ResetBowString(SelectExitEventArgs arg0)
    {
        OnBowReleased?.Invoke(strength);
        strength = 0;
        previousStrength = 0;
        audioSource.pitch = 1;
        audioSource.Stop();

        interactor = null;
        midPointGrabObject.localPosition = Vector3.zero;
        midPointVisualObject.localPosition = Vector3.zero;
        bowStringRenderer.CreateString(null);

    }

    private void PrepareBowString(SelectEnterEventArgs arg0)
    {
        interactor = arg0.interactorObject.transform;
        OnBowPulled?.Invoke();
    }

    void ApplyCalibration()
    {
        float wingspan = PlayerPrefs.GetFloat("Wingspan", 1.5f);

        bowStringStretchLimit = wingspan * drawMultiplier;

        Debug.Log("Bow draw distance set to: " + bowStringStretchLimit);
    }

    private void Update()
    {
        if (interactor != null)
        {
            //convert bow string mid point position to the local space of the MidPoint
            Vector3 midPointLocalSpace =
                midPointParent.InverseTransformPoint(midPointGrabObject.position); // localPosition

            //get the offset
            float midPointLocalZAbs = Mathf.Abs(midPointLocalSpace.z);

            previousStrength = strength;

            HandleStringPushedBackToStart(midPointLocalSpace);

            HandleStringPulledBackTolimit(midPointLocalZAbs, midPointLocalSpace);

            HandlePullingString(midPointLocalZAbs, midPointLocalSpace);

            bowStringRenderer.CreateString(midPointVisualObject.position);
        }
    }

    private void HandlePullingString(float midPointLocalZAbs, Vector3 midPointLocalSpace)
    {
        //what happens when we are between point 0 and the string pull limit
        if (midPointLocalSpace.z < 0 && midPointLocalZAbs < bowStringStretchLimit)
        {
            if (audioSource.isPlaying == false && strength <= 0.01f)
            {
                audioSource.Play();
            }

            strength = Remap(midPointLocalZAbs, 0, bowStringStretchLimit, 0, 1);
            midPointVisualObject.localPosition = new Vector3(0, 0, midPointLocalSpace.z);

            PlayStringPullinSound();
        }
    }

    private void PlayStringPullinSound()
    {
        //Check if we have moved the string enought to play the sound unpause it
        if (Mathf.Abs(strength - previousStrength) > stringSoundThreshold)
        {
            if (strength < previousStrength)
            {
                //Play string sound in reverse if we are pusing the string towards the bow
                audioSource.pitch = -1;
            }
            else
            {
                //Play the sound normally
                audioSource.pitch = 1;
            }
            audioSource.UnPause();
        }
        else
        {
            //if we stop moving Pause the sounds
            audioSource.Pause();
        }

    }

    private float Remap(float value, int fromMin, float fromMax, int toMin, int toMax)
    {
        return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }

    private void HandleStringPulledBackTolimit(float midPointLocalZAbs, Vector3 midPointLocalSpace)
    {
        //We specify max pulling limit for the string. We don't allow the string to go any farther than "bowStringStretchLimit"
        if (midPointLocalSpace.z < 0 && midPointLocalZAbs >= bowStringStretchLimit)
        {
            audioSource.Pause();
            strength = 1;
            //Vector3 direction = midPointParent.TransformDirection(new Vector3(0, 0, midPointLocalSpace.z));
            midPointVisualObject.localPosition = new Vector3(0, 0, -bowStringStretchLimit);
        }
    }

    private void HandleStringPushedBackToStart(Vector3 midPointLocalSpace)
    {
        if (midPointLocalSpace.z >= 0)
        {
            audioSource.pitch = 1;
            audioSource.Stop();
            strength = 0;
            midPointVisualObject.localPosition = Vector3.zero;
        }
    }
}