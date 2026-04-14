using UnityEngine;
using UnityEngine.UI;

public class TimeTrialHud : MonoBehaviour
{
    const float PlaneDistance = 0.35f;
    const int FontSize = 44;

    static readonly Color TimerColor = new(1f, 0.92f, 0.16f, 1f);
    static readonly Color ScoreColor = new(0.25f, 0.95f, 0.35f, 1f);

    Canvas _canvas;
    Text _timer;
    Text _score;

    void Start()
    {
        ScoreManager.Instance?.ResetScore();
        GameTimer.Instance?.ResetTimer();
    }

    void LateUpdate()
    {
        if (_canvas == null)
            BuildIfPossible();

        if (_canvas != null && Camera.main != null && _canvas.worldCamera != Camera.main)
            _canvas.worldCamera = Camera.main;

        if (_timer != null && GameTimer.Instance != null)
            _timer.text = GameTimer.Instance.GetFormattedTime();

        if (_score != null && ScoreManager.Instance != null)
            _score.text = $"Score: {ScoreManager.Instance.Score}";
    }

    void BuildIfPossible()
    {
        var cam = Camera.main;
        if (cam == null || !cam.isActiveAndEnabled)
            return;

        var root = new GameObject("HudCanvas");
        root.transform.SetParent(transform, false);

        _canvas = root.AddComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = cam;
        _canvas.planeDistance = PlaneDistance;
        _canvas.sortingOrder = 1000;

        var scaler = root.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);

        root.AddComponent<GraphicRaycaster>();

        var rt = root.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;

        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        _timer = AddLabel(root.transform, "00:00", upperLeft: true, TimerColor, font);
        _score = AddLabel(root.transform, "Score: 0", upperLeft: false, ScoreColor, font);
    }

    static Text AddLabel(Transform parent, string text, bool upperLeft, Color color, Font font)
    {
        var go = new GameObject(upperLeft ? "Timer" : "Score");
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(parent, false);

        if (upperLeft)
        {
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = new Vector2(48f, -48f);
        }
        else
        {
            rt.anchorMin = rt.anchorMax = new Vector2(1f, 1f);
            rt.pivot = new Vector2(1f, 1f);
            rt.anchoredPosition = new Vector2(-48f, -48f);
        }

        rt.sizeDelta = new Vector2(520f, 100f);

        var label = go.AddComponent<Text>();
        label.text = text;
        label.fontSize = FontSize;
        label.color = color;
        label.fontStyle = FontStyle.Bold;
        label.alignment = upperLeft ? TextAnchor.UpperLeft : TextAnchor.UpperRight;
        label.horizontalOverflow = HorizontalWrapMode.Overflow;
        label.verticalOverflow = VerticalWrapMode.Overflow;
        label.raycastTarget = false;
        if (font != null)
            label.font = font;

        return label;
    }
}
