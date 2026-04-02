using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
    private static SettingsMenuUI instance;

    private Canvas targetCanvas;
    private GameObject settingsButton;
    private GameObject settingsPanel;
    private Slider musicSlider;
    private Slider sfxSlider;
    private TMP_Text musicValueText;
    private TMP_Text sfxValueText;
    private bool isPanelOpen;
    private float previousTimeScale = 1f;

    public static bool IsOpen => instance != null && instance.isPanelOpen;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        EnsureInstance();
    }

    private static void EnsureInstance()
    {
        if (instance != null) return;

        GameObject root = new GameObject("SettingsMenuUI");
        instance = root.AddComponent<SettingsMenuUI>();
        DontDestroyOnLoad(root);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        BuildUiIfNeeded();
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BuildUiIfNeeded();
    }

    private void Update()
    {
        if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame) return;
        TogglePanel();
    }

    private void BuildUiIfNeeded()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null) return;

        EnsureEventSystem();

        if (targetCanvas != canvas)
        {
            targetCanvas = canvas;
            settingsButton = null;
            settingsPanel = null;
        }

        if (settingsButton == null)
        {
            Transform existingButton = targetCanvas.transform.Find("RuntimeSettingsButton");
            settingsButton = existingButton != null ? existingButton.gameObject : CreateSettingsButton(targetCanvas.transform);
        }

        if (settingsPanel == null)
        {
            Transform existingPanel = targetCanvas.transform.Find("RuntimeSettingsPanel");
            settingsPanel = existingPanel != null ? existingPanel.gameObject : CreateSettingsPanel(targetCanvas.transform);
        }

        SyncFromAudio();
    }

    private void EnsureEventSystem()
    {
        EventSystem eventSystem = FindAnyObjectByType<EventSystem>();
        if (eventSystem == null)
        {
            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystem = eventSystemObject.AddComponent<EventSystem>();
        }

        StandaloneInputModule legacyModule = eventSystem.GetComponent<StandaloneInputModule>();
        if (legacyModule != null)
        {
            Destroy(legacyModule);
        }

        if (eventSystem.GetComponent<InputSystemUIInputModule>() == null)
        {
            eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
        }
    }

    private GameObject CreateSettingsButton(Transform parent)
    {
        GameObject buttonObject = CreateUiObject("RuntimeSettingsButton", parent);
        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(1f, 1f);
        rect.sizeDelta = new Vector2(180f, 48f);
        rect.anchoredPosition = new Vector2(-24f, -24f);

        Image image = buttonObject.AddComponent<Image>();
        image.color = new Color32(34, 45, 61, 220);

        Button button = buttonObject.AddComponent<Button>();
        button.targetGraphic = image;
        button.onClick.AddListener(TogglePanel);

        CreateLabel(buttonObject.transform, "Settings", 24, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        return buttonObject;
    }

    private GameObject CreateSettingsPanel(Transform parent)
    {
        GameObject panelObject = CreateUiObject("RuntimeSettingsPanel", parent);
        RectTransform rect = panelObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(1f, 1f);
        rect.sizeDelta = new Vector2(340f, 220f);
        rect.anchoredPosition = new Vector2(-24f, -84f);

        Image image = panelObject.AddComponent<Image>();
        image.color = new Color32(15, 23, 42, 230);

        VerticalLayoutGroup layout = panelObject.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(18, 18, 18, 18);
        layout.spacing = 14f;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childControlHeight = false;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = true;

        ContentSizeFitter fitter = panelObject.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        CreateLabel(panelObject.transform, "Audio Settings", 28, TextAlignmentOptions.Center, new Vector2(0f, 1f), new Vector2(1f, 1f), Vector2.zero, new Vector2(0f, 36f));
        CreateSliderRow(panelObject.transform, "Music", out musicSlider, out musicValueText, OnMusicChanged);
        CreateSliderRow(panelObject.transform, "SFX", out sfxSlider, out sfxValueText, OnSfxChanged);

        GameObject closeButton = CreateUiObject("CloseButton", panelObject.transform);
        LayoutElement closeLayout = closeButton.AddComponent<LayoutElement>();
        closeLayout.preferredHeight = 42f;

        Image closeImage = closeButton.AddComponent<Image>();
        closeImage.color = new Color32(71, 85, 105, 255);

        Button close = closeButton.AddComponent<Button>();
        close.targetGraphic = closeImage;
        close.onClick.AddListener(TogglePanel);

        CreateLabel(closeButton.transform, "Close", 24, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

        panelObject.SetActive(false);
        return panelObject;
    }

    private void CreateSliderRow(Transform parent, string label, out Slider slider, out TMP_Text valueText, UnityEngine.Events.UnityAction<float> onChanged)
    {
        GameObject row = CreateUiObject(label + "Row", parent);
        LayoutElement rowLayout = row.AddComponent<LayoutElement>();
        rowLayout.preferredHeight = 52f;

        HorizontalLayoutGroup rowGroup = row.AddComponent<HorizontalLayoutGroup>();
        rowGroup.spacing = 10f;
        rowGroup.childAlignment = TextAnchor.MiddleCenter;
        rowGroup.childControlHeight = true;
        rowGroup.childControlWidth = false;
        rowGroup.childForceExpandHeight = true;
        rowGroup.childForceExpandWidth = false;

        GameObject labelObject = CreateLabel(row.transform, label, 22, TextAlignmentOptions.Left, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        LayoutElement labelLayout = labelObject.AddComponent<LayoutElement>();
        labelLayout.preferredWidth = 80f;

        slider = CreateSlider(row.transform);
        slider.onValueChanged.AddListener(onChanged);

        GameObject valueObject = CreateLabel(row.transform, "100%", 20, TextAlignmentOptions.Right, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        valueText = valueObject.GetComponent<TMP_Text>();
        LayoutElement valueLayout = valueObject.AddComponent<LayoutElement>();
        valueLayout.preferredWidth = 64f;
    }

    private Slider CreateSlider(Transform parent)
    {
        GameObject sliderObject = CreateUiObject("Slider", parent);
        LayoutElement layout = sliderObject.AddComponent<LayoutElement>();
        layout.preferredWidth = 150f;

        RectTransform rect = sliderObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(150f, 24f);

        Slider slider = sliderObject.AddComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.wholeNumbers = false;

        GameObject background = CreateUiObject("Background", sliderObject.transform);
        RectTransform bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0f, 0.25f);
        bgRect.anchorMax = new Vector2(1f, 0.75f);
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = new Color32(51, 65, 85, 255);

        GameObject fillArea = CreateUiObject("Fill Area", sliderObject.transform);
        RectTransform fillAreaRect = fillArea.GetComponent<RectTransform>();
        fillAreaRect.anchorMin = new Vector2(0f, 0.25f);
        fillAreaRect.anchorMax = new Vector2(1f, 0.75f);
        fillAreaRect.offsetMin = new Vector2(10f, 0f);
        fillAreaRect.offsetMax = new Vector2(-10f, 0f);

        GameObject fill = CreateUiObject("Fill", fillArea.transform);
        RectTransform fillRect = fill.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;
        Image fillImage = fill.AddComponent<Image>();
        fillImage.color = new Color32(56, 189, 248, 255);

        GameObject handleArea = CreateUiObject("Handle Slide Area", sliderObject.transform);
        RectTransform handleAreaRect = handleArea.GetComponent<RectTransform>();
        handleAreaRect.anchorMin = Vector2.zero;
        handleAreaRect.anchorMax = Vector2.one;
        handleAreaRect.offsetMin = new Vector2(10f, 0f);
        handleAreaRect.offsetMax = new Vector2(-10f, 0f);

        GameObject handle = CreateUiObject("Handle", handleArea.transform);
        RectTransform handleRect = handle.GetComponent<RectTransform>();
        handleRect.sizeDelta = new Vector2(18f, 18f);
        Image handleImage = handle.AddComponent<Image>();
        handleImage.color = new Color32(241, 245, 249, 255);

        slider.targetGraphic = handleImage;
        slider.fillRect = fillRect;
        slider.handleRect = handleRect;
        slider.direction = Slider.Direction.LeftToRight;

        return slider;
    }

    private GameObject CreateLabel(Transform parent, string text, float fontSize, TextAlignmentOptions alignment, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta)
    {
        GameObject labelObject = CreateUiObject(text.Replace(" ", string.Empty) + "Label", parent);
        RectTransform rect = labelObject.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = sizeDelta;

        TextMeshProUGUI textComponent = labelObject.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.fontSize = fontSize;
        textComponent.alignment = alignment;
        textComponent.color = Color.white;
        return labelObject;
    }

    private static GameObject CreateUiObject(string name, Transform parent)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform));
        obj.transform.SetParent(parent, false);
        obj.layer = LayerMask.NameToLayer("UI");
        return obj;
    }

    private void TogglePanel()
    {
        if (settingsPanel == null) return;

        isPanelOpen = !isPanelOpen;
        settingsPanel.SetActive(isPanelOpen);
        settingsButton?.SetActive(!isPanelOpen);

        if (isPanelOpen)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = previousTimeScale;
        }

        SyncFromAudio();
    }

    private void SyncFromAudio()
    {
        GameAudio audio = GameAudio.Instance;
        if (musicSlider != null)
        {
            musicSlider.SetValueWithoutNotify(audio.GetMusicVolume());
            UpdateValueText(musicValueText, audio.GetMusicVolume());
        }

        if (sfxSlider != null)
        {
            sfxSlider.SetValueWithoutNotify(audio.GetSfxVolume());
            UpdateValueText(sfxValueText, audio.GetSfxVolume());
        }
    }

    private void OnMusicChanged(float value)
    {
        GameAudio.Instance.SetMusicVolume(value);
        UpdateValueText(musicValueText, value);
    }

    private void OnSfxChanged(float value)
    {
        GameAudio.Instance.SetSfxVolume(value);
        UpdateValueText(sfxValueText, value);
    }

    private void UpdateValueText(TMP_Text target, float value)
    {
        if (target == null) return;
        target.text = Mathf.RoundToInt(value * 100f) + "%";
    }
}
