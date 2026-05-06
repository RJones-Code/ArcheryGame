using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * File: MainMenu.cs
 *
 * Description:
 * Controls the main menu system, including navigation between sections
 * (Main, Options, About, Leaderboard), scene transitions, and dynamic
 * generation of the leaderboard UI. Also handles menu state persistence
 * between scene loads.
 *
 * Core Responsibilities:
 * - Manage main menu navigation (Main / Options / About / Leaderboard)
 * - Handle scene transitions (Play, Quit, return to menu)
 * - Dynamically construct and display leaderboard UI
 * - Configure scrollable leaderboard view and UI layout at runtime
 * - Control background interaction blocking for UI states
 *
 * Key Components:
 * - _main / _options / _about / _leaderboard:
 *   References to menu sections in the scene
 *
 * - Leaderboard system:
 *   Dynamically clones and builds UI including:
 *   - Scroll view
 *   - Scrollbar
 *   - Back button
 *   - Styled score text display
 *
 * - SceneFader:
 *   Handles smooth transitions between scenes
 *
 * Behavior Overview:
 * - Awake():
 *      - Locates menu sections in the hierarchy
 *      - Initializes references to UI elements
 *      - Optionally opens leaderboard on scene load
 *
 * - OpenLeaderboard():
 *      - Builds leaderboard UI if needed
 *      - Hides other menu sections
 *      - Shows leaderboard panel
 *      - Disables background raycast blocking
 *
 * - CloseLeaderboard():
 *      - Restores main menu visibility
 *      - Hides leaderboard
 *
 * - PlayGame():
 *      - Resets time scale
 *      - Loads GameScene via SceneFader
 *
 * - GoToLeaderboard():
 *      - Sets flag to open leaderboard after scene load
 *      - Loads MainMenu scene
 *
 * - QuitGame():
 *      - Exits application
 *
 * Dependencies:
 * - TextMeshPro (TMP)
 * - Unity UI system (Image, Button, Scrollbar)
 * - SceneFader (custom transition system)
 * - LeaderboardMenuPanel (runtime UI controller)
 *
 * Notes:
 * - This script performs significant runtime UI construction.
 * - Consider refactoring leaderboard generation into a separate UI builder
 *   for improved maintainability.
 */

public class MainMenu : MonoBehaviour
{

    static readonly Color LeaderboardTextColor = new Color(0.06f, 0.07f, 0.09f, 1f);
    static readonly Vector2 LeaderboardScrollPositionOffset = new Vector2(0.25f, -0.15f);
    const float LeaderboardTitleY = 2.2f;

    public static bool OpenLeaderboardOnLoad = false;

    GameObject _main;
    GameObject _options;
    GameObject _about;
    GameObject _leaderboard;
    Image _menuBackgroundImage;

    void Awake()
    {
        var startMenu = GameObject.Find("StartMenu")?.transform;
        if (startMenu != null)
        {
            _main = startMenu.Find("Main Section")?.gameObject;
            _options = startMenu.Find("Options Section")?.gameObject;
            _about = startMenu.Find("About Section")?.gameObject;
            _leaderboard = startMenu.Find("Leaderboard Section")?.gameObject;
        }

        if (_main == null)
            _main = GameObject.Find("Main Section");
        if (_leaderboard == null)
            _leaderboard = GameObject.Find("Leaderboard Section");

        var bgGo = startMenu != null ? startMenu.Find("Background")?.gameObject : GameObject.Find("Background");
        if (bgGo != null)
            _menuBackgroundImage = bgGo.GetComponent<Image>();

        if (OpenLeaderboardOnLoad)
        {
            OpenLeaderboardOnLoad = false;
            OpenLeaderboard();
        }
    }

    void EnsureLeaderboardPage()
    {
        if (_leaderboard != null || _about == null)
            return;

        _leaderboard = Instantiate(_about, _about.transform.parent);
        _leaderboard.name = "Leaderboard Section";

        foreach (var t in _leaderboard.GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            if (t.gameObject.name.Contains("Title"))
            {
                t.text = "Leaderboard";
                var rt = t.rectTransform;
                rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, LeaderboardTitleY);
            }
        }

        var panel = _leaderboard.AddComponent<LeaderboardMenuPanel>();
        foreach (var t in _leaderboard.GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            if (t.gameObject.name == "About Text")
            {
                SetupLeaderboardScrollView(_leaderboard.transform, panel, t);
                break;
            }
        }

        var clonedBack = _leaderboard.transform.Find("BackButton");
        CreateLeaderboardBackButton(_leaderboard.transform, panel, clonedBack as RectTransform);

        if (clonedBack != null)
            Destroy(clonedBack.gameObject);

        StyleLeaderboardText(_leaderboard);

        _leaderboard.SetActive(false);
    }

    static void SetupLeaderboardScrollView(Transform leaderboardRoot, LeaderboardMenuPanel panel, TextMeshProUGUI scoresText)
    {
        var scoresRt = scoresText.rectTransform;
        var oldAnchorMin = scoresRt.anchorMin;
        var oldAnchorMax = scoresRt.anchorMax;
        var oldPivot = scoresRt.pivot;
        var oldAnchoredPos = scoresRt.anchoredPosition;
        var oldLocalPos = scoresRt.localPosition;
        var oldRotation = scoresRt.localRotation;
        var oldScale = scoresRt.localScale;
        var oldSibling = scoresRt.GetSiblingIndex();

        var scrollRoot = new GameObject("LeaderboardScrollView");
        scrollRoot.layer = 5;
        var scrollRt = scrollRoot.AddComponent<RectTransform>();
        scrollRoot.transform.SetParent(leaderboardRoot, false);
        scrollRt.anchorMin = oldAnchorMin;
        scrollRt.anchorMax = oldAnchorMax;
        scrollRt.pivot = oldPivot;
        scrollRt.anchoredPosition = new Vector2(
            oldAnchoredPos.x + LeaderboardScrollPositionOffset.x,
            1.35f + LeaderboardScrollPositionOffset.y);
        scrollRt.sizeDelta = new Vector2(420f, 180f);
        scrollRt.localPosition = oldLocalPos + new Vector3(LeaderboardScrollPositionOffset.x, LeaderboardScrollPositionOffset.y, 0f);
        scrollRt.localRotation = oldRotation;
        scrollRt.localScale = oldScale;
        scrollRt.SetSiblingIndex(oldSibling);

        // Width of the scrollbar strip (viewport uses the rest). Reduced 15% with text inset for tighter layout.
        const float scrollbarWidth = 16f * 0.85f;

        var viewportGo = new GameObject("Viewport");
        viewportGo.layer = 5;
        var viewportRt = viewportGo.AddComponent<RectTransform>();
        viewportGo.transform.SetParent(scrollRt, false);
        viewportRt.anchorMin = Vector2.zero;
        viewportRt.anchorMax = Vector2.one;
        viewportRt.offsetMin = Vector2.zero;
        viewportRt.offsetMax = new Vector2(-scrollbarWidth, 0f);
        var viewportImage = viewportGo.AddComponent<Image>();
        viewportImage.sprite = CreateWhiteSprite();
        viewportImage.type = Image.Type.Simple;
        viewportImage.color = new Color(1f, 1f, 1f, 0.001f);
        viewportImage.raycastTarget = true;

        var scrollbarGo = new GameObject("Scrollbar Vertical");
        scrollbarGo.layer = 5;
        var scrollbarRt = scrollbarGo.AddComponent<RectTransform>();
        scrollbarGo.transform.SetParent(scrollRt, false);
        scrollbarRt.anchorMin = new Vector2(1f, 0f);
        scrollbarRt.anchorMax = new Vector2(1f, 1f);
        scrollbarRt.pivot = new Vector2(1f, 1f);
        scrollbarRt.offsetMin = new Vector2(-scrollbarWidth, 0f);
        scrollbarRt.offsetMax = Vector2.zero;

        var scrollbarBg = scrollbarGo.AddComponent<Image>();
        scrollbarBg.type = Image.Type.Simple;
        scrollbarBg.color = new Color(0f, 0f, 0f, 0.2f);
        var scrollbar = scrollbarGo.AddComponent<Scrollbar>();
        scrollbar.direction = Scrollbar.Direction.BottomToTop;
        scrollbar.numberOfSteps = 0;

        var slidingAreaGo = new GameObject("Sliding Area");
        slidingAreaGo.layer = 5;
        var slidingRt = slidingAreaGo.AddComponent<RectTransform>();
        slidingAreaGo.transform.SetParent(scrollbarRt, false);
        slidingRt.anchorMin = Vector2.zero;
        slidingRt.anchorMax = Vector2.one;
        slidingRt.offsetMin = new Vector2(2f * 0.85f, 3f);
        slidingRt.offsetMax = new Vector2(-2f * 0.85f, -3f);

        var handleGo = new GameObject("Handle");
        handleGo.layer = 5;
        var handleRt = handleGo.AddComponent<RectTransform>();
        handleGo.transform.SetParent(slidingRt, false);
        handleRt.anchorMin = Vector2.zero;
        handleRt.anchorMax = Vector2.one;
        handleRt.offsetMin = Vector2.zero;
        handleRt.offsetMax = Vector2.zero;
        var handleImage = handleGo.AddComponent<Image>();
        handleImage.type = Image.Type.Simple;
        handleImage.color = new Color(1f, 1f, 1f, 0.85f);

        scrollbar.handleRect = handleRt;
        scrollbar.targetGraphic = handleImage;
        scrollbar.value = 1f;

        scoresRt.SetParent(viewportRt, false);
        scoresRt.anchorMin = new Vector2(0f, 1f);
        scoresRt.anchorMax = new Vector2(1f, 1f);
        scoresRt.pivot = new Vector2(0.5f, 1f);
        scoresRt.anchoredPosition = Vector2.zero;
        scoresRt.localPosition = Vector3.zero;
        scoresRt.localRotation = Quaternion.identity;
        scoresRt.localScale = Vector3.one;
        scoresRt.sizeDelta = new Vector2(0f, 200f);

        var oldFitter = scoresText.GetComponent<ContentSizeFitter>();
        if (oldFitter != null)
            Object.DestroyImmediate(oldFitter);

        scoresText.raycastTarget = false;
        scoresText.enableWordWrapping = false;

        panel.BindScoresText(scoresText);
        panel.BindScrollControls(scrollbar, viewportRt);
        Canvas.ForceUpdateCanvases();
    }

    static Sprite CreateWhiteSprite()
    {
        var tex = Texture2D.whiteTexture;
        return Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
    }

    void CreateLeaderboardBackButton(Transform leaderboardRoot, LeaderboardMenuPanel panel, RectTransform clonedAboutBack)
    {
        RectTransform templateRt = clonedAboutBack;
        if (templateRt == null && _options != null)
            templateRt = _options.transform.Find("BackButton") as RectTransform;
        if (templateRt == null)
        {
            var main = GameObject.Find("StartMenu")?.transform.Find("Main Section")
                ?? GameObject.Find("Main Section")?.transform;
            if (main != null)
                templateRt = main.Find("LeaderboardButton") as RectTransform;
        }

        var go = new GameObject("LeaderboardBackButton");
        go.layer = 5;
        var rt = go.AddComponent<RectTransform>();
        go.transform.SetParent(leaderboardRoot, false);

        if (templateRt != null)
        {
            rt.anchorMin = templateRt.anchorMin;
            rt.anchorMax = templateRt.anchorMax;
            rt.pivot = templateRt.pivot;
            rt.anchoredPosition = templateRt.anchoredPosition;
            rt.sizeDelta = templateRt.sizeDelta;
            rt.localRotation = templateRt.localRotation;
            rt.localScale = templateRt.localScale;
            rt.localPosition = templateRt.localPosition;
        }
        else
        {
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = new Vector2(-6.661f, 0.773f);
            rt.sizeDelta = new Vector2(160f, 30f);
        }

        var img = go.AddComponent<Image>();
        var templateImg = templateRt != null ? templateRt.GetComponent<Image>() : null;
        if (templateImg != null && templateImg.sprite != null)
        {
            img.sprite = templateImg.sprite;
            img.type = templateImg.type;
            img.color = templateImg.color;
        }
        else
        {
            img.type = Image.Type.Simple;
            img.color = Color.white;
        }

        if (img.sprite == null)
        {
            var t = Texture2D.whiteTexture;
            img.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f), 100f);
            img.type = Image.Type.Simple;
        }

        img.raycastTarget = true;

        var btn = go.AddComponent<Button>();
        var templateBtn = templateRt != null ? templateRt.GetComponent<Button>() : null;
        if (templateBtn != null)
        {
            btn.transition = templateBtn.transition;
            btn.colors = templateBtn.colors;
        }

        btn.targetGraphic = img;
        btn.onClick.AddListener(panel.BackToMainMenu);

        var textGo = new GameObject("Text (TMP)");
        textGo.layer = 5;
        var textRt = textGo.AddComponent<RectTransform>();
        textGo.transform.SetParent(go.transform, false);
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = Vector2.zero;
        textRt.offsetMax = Vector2.zero;

        var tmp = textGo.AddComponent<TextMeshProUGUI>();
        var templateTmp = templateRt != null ? templateRt.GetComponentInChildren<TextMeshProUGUI>(true) : null;
        if (templateTmp != null)
        {
            tmp.font = templateTmp.font;
            tmp.fontSharedMaterial = templateTmp.fontSharedMaterial;
            tmp.fontSize = templateTmp.fontSize;
            tmp.color = templateTmp.color;
            tmp.alignment = templateTmp.alignment;
        }
        else
        {
            var fontSrc = leaderboardRoot.GetComponentInChildren<TextMeshProUGUI>(true);
            if (fontSrc != null)
            {
                tmp.font = fontSrc.font;
                tmp.fontSharedMaterial = fontSrc.fontSharedMaterial;
            }
        }

        tmp.text = "Back";
        tmp.raycastTarget = false;

        rt.SetAsLastSibling();
        var lp = rt.localPosition;
        rt.localPosition = new Vector3(lp.x, lp.y, lp.z + 0.02f);
    }

    static void StyleLeaderboardText(GameObject root)
    {
        foreach (var tmp in root.GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            if (tmp.transform.parent != null && tmp.transform.parent.name == "LeaderboardBackButton")
                continue;

            tmp.color = LeaderboardTextColor;
            if (tmp.fontSize < 26f)
                tmp.fontSize = 26f;
        }
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;

        SceneFader.GetOrCreate().FadeToScene("GameScene");
    }

    public void OpenLeaderboard()
    {
        EnsureLeaderboardPage();
        if (_options != null)
            _options.SetActive(false);
        if (_about != null)
            _about.SetActive(false);
        if (_main != null)
            _main.SetActive(false);
        if (_leaderboard != null)
        {
            _leaderboard.SetActive(true);
            _leaderboard.transform.SetAsLastSibling();
        }

        if (_menuBackgroundImage != null)
            _menuBackgroundImage.raycastTarget = false;
    }

    public void CloseLeaderboard()
    {
        if (_leaderboard != null)
            _leaderboard.SetActive(false);
        if (_main != null)
            _main.SetActive(true);

        if (_menuBackgroundImage != null)
            _menuBackgroundImage.raycastTarget = true;
    }

    public void GoToLeaderboard()
    {
        MainMenu.OpenLeaderboardOnLoad = true;
        SceneFader.GetOrCreate().FadeToScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
