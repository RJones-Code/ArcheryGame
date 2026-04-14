using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public SceneFader fader;

    static readonly Color LeaderboardTextColor = new Color(0.06f, 0.07f, 0.09f, 1f);

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
                t.text = "Leaderboard";
        }

        var panel = _leaderboard.AddComponent<LeaderboardMenuPanel>();
        foreach (var t in _leaderboard.GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            if (t.gameObject.name == "About Text")
            {
                panel.BindScoresText(t);
                var scoresRt = t.rectTransform;
                scoresRt.sizeDelta = new Vector2(420f, 180f);
                scoresRt.anchoredPosition = new Vector2(scoresRt.anchoredPosition.x, 1.35f);
                t.raycastTarget = false;
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
            img.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd");
            img.type = Image.Type.Sliced;
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
        fader.FadeToScene("GameScene");
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

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
