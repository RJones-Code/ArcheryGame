using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * File: MenuActions.cs
 *
 * Description:
 * Handles menu-related actions such as returning to the main menu.
 * Ensures the game state is properly reset (e.g., unpausing time)
 * before transitioning scenes with a fade effect.
 *
 * Core Responsibilities:
 * - Trigger scene transitions from UI interactions
 * - Reset time scale to ensure normal gameplay state
 * - Use SceneFader for smooth visual transitions
 *
 * Key Components:
 * - SceneFader: Handles fade-in/fade-out transitions between scenes
 * - Unity SceneManager: Loads scenes by name
 *
 * Behavior:
 * - ReturnToMenu():
 *      - Resets Time.timeScale to 1 (ensures game is not paused)
 *      - Initiates a fade transition to the "MainMenu" scene
 *
 * Dependencies:
 * - SceneFader (must implement GetOrCreate() and FadeToScene())
 * - Unity SceneManagement system
 *
 * Usage:
 * Attach this script to a UI controller object.
 * Hook ReturnToMenu() to a button’s OnClick() event.
 */

public class MenuActions : MonoBehaviour
{
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;

        SceneFader.GetOrCreate().FadeToScene("MainMenu");
    }
}