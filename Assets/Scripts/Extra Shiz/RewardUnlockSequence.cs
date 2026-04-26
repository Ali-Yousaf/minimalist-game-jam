using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class RewardUnlockSequence : MonoBehaviour
{
    public static RewardUnlockSequence Instance;

    [Header("References")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descText;

    [Header("Animation")]
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float displayTime = 3f;
    [SerializeField] private float startY = -600f; // off-screen
    [SerializeField] private float endY = 0f;      // center

    private Sequence sequence;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);
    }

    public void PlayUnlock(Sprite sprite, string title, string desc)
    {
        Debug.Log("Playing Sequence");
        
        sequence?.Kill();
        panel.gameObject.SetActive(true);

        // Set content
        icon.sprite = sprite;
        titleText.text = title;
        descText.text = desc;

        // Reset position
        panel.anchoredPosition = new Vector2(0, startY);

        sequence = DOTween.Sequence();

        // Slide IN
        sequence.Append(
            panel.DOAnchorPosY(endY, slideDuration)
                .SetEase(Ease.OutCubic)
        );

        // Wait
        sequence.AppendInterval(displayTime);

        // Slide OUT
        sequence.Append(
            panel.DOAnchorPosY(startY, slideDuration)
                .SetEase(Ease.InCubic)
        );

        sequence.OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}