using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

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
