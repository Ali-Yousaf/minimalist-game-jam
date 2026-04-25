using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class RewardUnlockSequence : MonoBehaviour
{
    public static RewardUnlockSequence Instance;

    [Header("UI References")]
    [SerializeField] private GameObject container;
    [SerializeField] private Image rewardIcon;
    [SerializeField] private TextMeshProUGUI rewardNameText;
    [SerializeField] private TextMeshProUGUI rewardDescText;
    [SerializeField] private Image background;

    [Header("Settings")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float displayTime = 1.5f;

    private Sequence sequence;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayUnlock(Sprite icon, string title, string description)
    {
        sequence?.Kill();
        
        // if (Spawner.Instance != null)
        //     Spawner.Instance.enableSpawning = false;

        container.SetActive(true);

        rewardIcon.sprite = icon;
        rewardNameText.text = title;
        rewardDescText.text = description;

        rewardIcon.transform.localScale = Vector3.zero;
        rewardIcon.color = new Color(1,1,1,0);

        rewardNameText.alpha = 0;
        rewardDescText.alpha = 0;

        if (background != null)
            background.color = new Color(0,0,0,0);

        sequence = DOTween.Sequence();

        if (background != null)
            sequence.Append(background.DOFade(0.6f, 0.2f));

        sequence.Append(rewardIcon.DOFade(1f, 0.2f));
        sequence.Join(
            rewardIcon.transform.DOScale(1.2f, animationDuration)
            .SetEase(Ease.OutBack)
        );

        sequence.Append(rewardIcon.transform.DOScale(1f, 0.2f));

        sequence.Append(rewardNameText.DOFade(1f, 0.3f));
        sequence.Join(
            rewardNameText.transform.DOLocalMoveY(
                rewardNameText.transform.localPosition.y + 20f, 
                0.3f
            ).From()
        );

        sequence.Append(rewardDescText.DOFade(1f, 0.3f));
        sequence.Join(
            rewardDescText.transform.DOLocalMoveY(
                rewardDescText.transform.localPosition.y + 15f, 
                0.3f
            ).From()
        );

        // Stay visible for 3 seconds
        sequence.AppendInterval(3f);

        // Exit 
        sequence.Append(rewardIcon.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack));
        sequence.Join(rewardIcon.DOFade(0f, 0.2f));

        sequence.Join(rewardNameText.DOFade(0f, 0.2f));
        sequence.Join(rewardDescText.DOFade(0f, 0.2f));

        if (background != null)
            sequence.Join(background.DOFade(0f, 0.3f));

        sequence.OnComplete(() =>
        {
            container.SetActive(false);

            // if (Spawner.Instance != null)
            //     Spawner.Instance.enableSpawning = true;
        });
    }
}