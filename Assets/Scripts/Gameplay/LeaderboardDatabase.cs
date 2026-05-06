using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/*
 * File: LeaderboardDatabase.cs
 *
 * Description:
 * Provides a lightweight local leaderboard system using JSON file storage.
 * Handles saving, loading, sorting, and retrieving high score entries,
 * as well as storing the player's display name using PlayerPrefs.
 *
 * Core Responsibilities:
 * - Persist leaderboard data to disk (JSON format)
 * - Add new score entries with timestamps
 * - Sort and maintain a capped list of top scores
 * - Retrieve top N leaderboard entries
 * - Store and retrieve the player's name
 *
 * Key Components:
 * - Entry: Represents a single leaderboard record (name, score, timestamp)
 * - Table: Container for all leaderboard entries
 *
 * Data Storage:
 * - File: Stored at Application.persistentDataPath/leaderboard_db.json
 * - Format: JSON (Unity JsonUtility)
 * - Player name stored via PlayerPrefs
 *
 * Behavior:
 * - AddEntry():
 *      - Validates player name
 *      - Adds a new score entry with current UTC timestamp
 *      - Sorts entries in descending score order
 *      - Trims list to MaxEntries
 *      - Saves updated data to disk
 *
 * - GetTop(int count):
 *      - Returns the top N scores in descending order
 *
 * - LoadTable():
 *      - Reads leaderboard data from file
 *      - Handles missing/corrupt files safely
 *
 * - SaveTable():
 *      - Serializes leaderboard data to JSON and writes to disk
 *
 * Properties:
 * - PlayerName:
 *      - Gets/sets the player’s display name using PlayerPrefs
 *
 * Configuration Notes:
 * - MaxEntries limits the number of stored scores (default: 64)
 * - Entries include Unix timestamps for potential future use (sorting, UI, etc.)
 *
 * Dependencies:
 * - UnityEngine (Application, PlayerPrefs, Debug)
 * - System.IO (file operations)
 * - System.Linq (sorting and querying)
 *
 * Usage:
 * - Call AddEntry(name, score) after a game ends
 * - Call GetTop(n) to populate leaderboard UI
 * - Access PlayerName to store/retrieve the current player’s name
 */

public static class LeaderboardDatabase
{
    const string FileName = "leaderboard_db.json";
    const int MaxEntries = 64;
    const string PlayerNameKey = "LeaderboardPlayerName";

    static string FilePath => Path.Combine(Application.persistentDataPath, FileName);

    [Serializable]
    public class Entry
    {
        public string playerName;
        public int score;
        public long recordedUnix;
    }

    [Serializable]
    class Table
    {
        public Entry[] entries = Array.Empty<Entry>();
    }

    public static string PlayerName
    {
        get => PlayerPrefs.GetString(PlayerNameKey, "Archer");
        set
        {
            PlayerPrefs.SetString(PlayerNameKey, value);
            PlayerPrefs.Save();
        }
    }

    public static void AddEntry(string playerName, int score)
    {
        if (string.IsNullOrWhiteSpace(playerName))
            playerName = "Archer";

        var table = LoadTable();
        var list = new List<Entry>(table.entries ?? Array.Empty<Entry>())
        {
            new Entry
            {
                playerName = playerName.Trim(),
                score = score,
                recordedUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            }
        };

        list.Sort((a, b) => b.score.CompareTo(a.score));
        if (list.Count > MaxEntries)
            list.RemoveRange(MaxEntries, list.Count - MaxEntries);

        table.entries = list.ToArray();
        SaveTable(table);
    }

    public static IReadOnlyList<Entry> GetTop(int count)
    {
        var table = LoadTable();
        var arr = table.entries ?? Array.Empty<Entry>();
        return arr.OrderByDescending(e => e.score).Take(Math.Max(0, count)).ToArray();
    }

    static Table LoadTable()
    {
        try
        {
            if (!File.Exists(FilePath))
                return new Table();

            var json = File.ReadAllText(FilePath);
            if (string.IsNullOrWhiteSpace(json))
                return new Table();

            var t = JsonUtility.FromJson<Table>(json);
            return t ?? new Table();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"LeaderboardDatabase load failed: {e.Message}");
            return new Table();
        }
    }

    static void SaveTable(Table table)
    {
        try
        {
            var json = JsonUtility.ToJson(table, true);
            File.WriteAllText(FilePath, json);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"LeaderboardDatabase save failed: {e.Message}");
        }
    }
}
