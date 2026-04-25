using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class RewardUnlockSequence : MonoBehaviour
{
    public static RewardUnlockSequence Instance;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float displayTime = 3f;

    // Runtime-created references
    private GameObject container;
    private Image rewardIconImage;
    private TextMeshProUGUI rewardNameText;
    private TextMeshProUGUI rewardDescText;
    private Image background;

    private Sequence sequence;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            BuildUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // =============================================
    // BUILD ENTIRE UI HIERARCHY AT RUNTIME
    // =============================================
    private void BuildUI()
    {
        // --- Canvas ---
        GameObject canvasGO = new GameObject("RewardCanvas");
        DontDestroyOnLoad(canvasGO);

        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        canvasGO.AddComponent<CanvasScaler>().uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasGO.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
        canvasGO.AddComponent<GraphicRaycaster>();

        // --- Container (full-screen anchor) ---
        container = new GameObject("Container");
        container.transform.SetParent(canvasGO.transform, false);

        RectTransform containerRect = container.AddComponent<RectTransform>();
        containerRect.anchorMin = Vector2.zero;
        containerRect.anchorMax = Vector2.one;
        containerRect.offsetMin = Vector2.zero;
        containerRect.offsetMax = Vector2.zero;

        // --- Background panel ---
        GameObject bgGO = new GameObject("Background");
        bgGO.transform.SetParent(container.transform, false);

        RectTransform bgRect = bgGO.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        background = bgGO.AddComponent<Image>();
        background.color = new Color(0, 0, 0, 0);

        // --- Center panel (card) ---
        GameObject panel = new GameObject("Panel");
        panel.transform.SetParent(container.transform, false);

        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(420, 320);
        panelRect.anchoredPosition = Vector2.zero;

        Image panelImg = panel.AddComponent<Image>();
        panelImg.color = new Color(0.1f, 0.1f, 0.15f, 0.92f);

        // --- Reward icon ---
        GameObject iconGO = new GameObject("RewardIcon");
        iconGO.transform.SetParent(panel.transform, false);

        RectTransform iconRect = iconGO.AddComponent<RectTransform>();
        iconRect.anchorMin = new Vector2(0.5f, 0.5f);
        iconRect.anchorMax = new Vector2(0.5f, 0.5f);
        iconRect.sizeDelta = new Vector2(140, 140);
        iconRect.anchoredPosition = new Vector2(0, 60);

        rewardIconImage = iconGO.AddComponent<Image>();
        rewardIconImage.preserveAspect = true;

        // --- Title text ---
        GameObject titleGO = new GameObject("TitleText");
        titleGO.transform.SetParent(panel.transform, false);

        RectTransform titleRect = titleGO.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.5f);
        titleRect.anchorMax = new Vector2(0.5f, 0.5f);
        titleRect.sizeDelta = new Vector2(380, 50);
        titleRect.anchoredPosition = new Vector2(0, -45);

        rewardNameText = titleGO.AddComponent<TextMeshProUGUI>();
        rewardNameText.fontSize = 28;
        rewardNameText.fontStyle = FontStyles.Bold;
        rewardNameText.alignment = TextAlignmentOptions.Center;
        rewardNameText.color = Color.white;

        // --- Description text ---
        GameObject descGO = new GameObject("DescText");
        descGO.transform.SetParent(panel.transform, false);

        RectTransform descRect = descGO.AddComponent<RectTransform>();
        descRect.anchorMin = new Vector2(0.5f, 0.5f);
        descRect.anchorMax = new Vector2(0.5f, 0.5f);
        descRect.sizeDelta = new Vector2(360, 60);
        descRect.anchoredPosition = new Vector2(0, -100);

        rewardDescText = descGO.AddComponent<TextMeshProUGUI>();
        rewardDescText.fontSize = 18;
        rewardDescText.alignment = TextAlignmentOptions.Center;
        rewardDescText.color = new Color(0.85f, 0.85f, 0.85f, 1f);
        rewardDescText.textWrappingMode = TextWrappingModes.Normal;

        // Start hidden
        container.SetActive(false);
    }

    // =============================================
    // PUBLIC API — called from TankHealth
    // =============================================
    public void PlayUnlock(Sprite icon, string title, string description)
    {
        sequence?.Kill();

        container.SetActive(true);

        rewardIconImage.sprite = icon;
        rewardNameText.text = title;
        rewardDescText.text = description;

        SetupInitialState();
        PlayAnimation();
    }

    private void SetupInitialState()
    {
        rewardIconImage.transform.localScale = Vector3.zero;
        rewardIconImage.color = new Color(1, 1, 1, 0);

        rewardNameText.alpha = 0;
        rewardDescText.alpha = 0;

        background.color = new Color(0, 0, 0, 0);
    }

    private void PlayAnimation()
    {
        sequence = DOTween.Sequence();

        sequence.Append(background.DOFade(0.6f, 0.2f));

        // Icon pop
        sequence.Append(rewardIconImage.DOFade(1f, 0.2f));
        sequence.Join(
            rewardIconImage.transform.DOScale(1.2f, animationDuration)
                .SetEase(Ease.OutBack)
        );
        sequence.Append(rewardIconImage.transform.DOScale(1f, 0.2f));

        // Title
        sequence.Append(rewardNameText.DOFade(1f, 0.3f));
        sequence.Join(
            rewardNameText.transform.DOLocalMoveY(
                rewardNameText.transform.localPosition.y + 20f, 0.3f
            ).From()
        );

        // Description
        sequence.Append(rewardDescText.DOFade(1f, 0.3f));
        sequence.Join(
            rewardDescText.transform.DOLocalMoveY(
                rewardDescText.transform.localPosition.y + 15f, 0.3f
            ).From()
        );

        sequence.AppendInterval(displayTime);

        // Exit
        sequence.Append(rewardIconImage.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack));
        sequence.Join(rewardIconImage.DOFade(0f, 0.2f));
        sequence.Join(rewardNameText.DOFade(0f, 0.2f));
        sequence.Join(rewardDescText.DOFade(0f, 0.2f));
        sequence.Join(background.DOFade(0f, 0.3f));

        sequence.OnComplete(() => container.SetActive(false));
    }
}