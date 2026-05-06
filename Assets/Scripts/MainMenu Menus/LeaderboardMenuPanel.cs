using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardMenuPanel : MonoBehaviour
{
    /// <summary> Horizontal inset so score text does not sit flush against the scrollbar; controls most of the visible gap. </summary>
    const float ScoreTextToScrollbarInset = 4f * 0.85f;
    // Player-name column padding in the score lines (controls whitespace between name and score).
    // Reduced 15% from the original 14-character column.
    const int PlayerNameColumnWidth = 12;
    const string HeaderName = "Name";
    const string HeaderScore = "Score";

    [SerializeField] TextMeshProUGUI scoresText;
    [SerializeField] Scrollbar scoresScrollbar;
    [SerializeField] RectTransform scoresViewport;

    readonly List<string> _lines = new List<string>();

    public void BindScoresText(TextMeshProUGUI text)
    {
        scoresText = text;
    }

    public void BindScrollControls(Scrollbar scrollbar, RectTransform viewport)
    {
        scoresScrollbar = scrollbar;
        scoresViewport = viewport;
    }

    void OnEnable()
    {
        if (scoresScrollbar != null)
            scoresScrollbar.onValueChanged.AddListener(OnScrollbarChanged);
        RefreshScores();
    }

    void OnDisable()
    {
        if (scoresScrollbar != null)
            scoresScrollbar.onValueChanged.RemoveListener(OnScrollbarChanged);
    }

    void OnScrollbarChanged(float _)
    {
        UpdateVisibleWindow();
    }

    public void RefreshScores()
    {
        if (scoresText == null)
            scoresText = GetComponentInChildren<TextMeshProUGUI>(true);

        if (scoresText == null || scoresViewport == null || scoresScrollbar == null)
            return;

        var top = LeaderboardDatabase.GetTop(64);
        _lines.Clear();
        if (top.Count == 0)
            _lines.Add("No scores yet. Play a round to record one.");
        else
        {
            for (var i = 0; i < top.Count; i++)
            {
                var e = top[i];
                _lines.Add($"{i + 1}. {e.playerName,-PlayerNameColumnWidth} {e.score}");
            }
        }

        scoresText.enableWordWrapping = false;
        scoresText.overflowMode = TextOverflowModes.Overflow;
        scoresText.maskable = false;

        Canvas.ForceUpdateCanvases();

        scoresScrollbar.SetValueWithoutNotify(1f);
        UpdateVisibleWindow();
    }

    void UpdateVisibleWindow()
    {
        if (scoresText == null || scoresViewport == null || scoresScrollbar == null || _lines.Count == 0)
            return;

        var width = Mathf.Max(1f, scoresViewport.rect.width - ScoreTextToScrollbarInset);
        var viewportHeight = scoresViewport.rect.height;
        if (viewportHeight < 1f)
            viewportHeight = 160f;

        var lineHeight = scoresText.GetPreferredValues("X", width, 0f).y;
        if (lineHeight < 1f)
            lineHeight = scoresText.fontSize * 1.15f;

        var visibleCount = Mathf.Max(1, Mathf.FloorToInt(viewportHeight / lineHeight));
        var headerLines = 1; // labels only
        var bodyVisibleCount = Mathf.Max(0, visibleCount - headerLines);
        var total = _lines.Count;
        var needsScroll = total > bodyVisibleCount;

        scoresScrollbar.interactable = needsScroll;
        scoresScrollbar.size = needsScroll
            ? Mathf.Clamp01(bodyVisibleCount / (float)total)
            : 1f;

        var maxFirst = Mathf.Max(0, total - bodyVisibleCount);
        var fromTop = scoresScrollbar.direction == Scrollbar.Direction.BottomToTop
            ? scoresScrollbar.value
            : 1f - scoresScrollbar.value;
        var first = needsScroll
            ? Mathf.Clamp(Mathf.FloorToInt((1f - fromTop) * maxFirst + 0.0001f), 0, maxFirst)
            : 0;

        var sb = new StringBuilder();
        sb.Append($"{HeaderName,5} {HeaderScore,10}");

        var end = Mathf.Min(total, first + bodyVisibleCount);
        for (var i = first; i < end; i++)
        {
            sb.Append('\n');
            sb.Append(_lines[i]);
        }

        scoresText.text = sb.ToString();

        var textRect = scoresText.rectTransform;
        textRect.anchorMin = new Vector2(0f, 1f);
        textRect.anchorMax = new Vector2(1f, 1f);
        textRect.pivot = new Vector2(0.5f, 1f);
        textRect.anchoredPosition = Vector2.zero;
        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        var windowHeight = lineHeight * (headerLines + (end - first));
        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(lineHeight, windowHeight));
    }

    public void BackToMainMenu()
    {
        var menu = Object.FindFirstObjectByType<MainMenu>(FindObjectsInactive.Include);
        if (menu != null)
            menu.CloseLeaderboard();
    }
}
