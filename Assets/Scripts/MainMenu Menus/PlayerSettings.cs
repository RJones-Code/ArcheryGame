/*
 * File: PlayerSettings.cs
 *
 * Description:
 * Stores global player configuration settings used across the project.
 * Currently used to define player handedness for VR interaction setup.
 *
 * Core Responsibilities:
 * - Provide global access to player preferences and settings
 * - Store configuration that affects input and interaction behavior
 *
 * Key Components:
 * - IsLeftHanded:
 *      - Determines whether the player uses left-handed or right-handed controls
 *      - Used to select appropriate VR prefabs and interaction setups
 *
 * Behavior:
 * - Static class ensures values are globally accessible without instantiation
 * - Values persist only for the current session unless externally saved
 *
 * Dependencies:
 * - Used by systems such as WeaponRack for spawning correct hand-specific objects
 *
 * Usage:
 * Access directly via PlayerSettings.IsLeftHanded
 * Example:
 *     if (PlayerSettings.IsLeftHanded) { ... }
 */

public static class PlayerSettings
{
    public static bool IsLeftHanded = false;
}