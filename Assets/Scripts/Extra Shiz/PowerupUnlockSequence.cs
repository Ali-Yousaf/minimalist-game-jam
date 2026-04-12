using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PowerupUnlockSequence : MonoBehaviour
{
    public static PowerupUnlockSequence Instance;

    [Header("UI References")]
    [SerializeField] private GameObject container; // parent panel
    [SerializeField] private Image powerupIcon;
    [SerializeField] private TextMeshProUGUI powerupNameText;
    [SerializeField] private TextMeshProUGUI powerupDescText;
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
        // Kill previous animation if running
        sequence?.Kill();

        // Activate UI
        container.SetActive(true);

        // Set data
        powerupIcon.sprite = icon;
        powerupNameText.text = title;
        powerupDescText.text = description;

        // Reset states
        powerupIcon.transform.localScale = Vector3.zero;
        powerupIcon.color = new Color(1,1,1,0);

        powerupNameText.alpha = 0;
        powerupDescText.alpha = 0;

        if (background != null)
            background.color = new Color(0,0,0,0);

        // Create sequence
        sequence = DOTween.Sequence();

        // 🔹 Background fade
        if (background != null)
        {
            sequence.Append(background.DOFade(0.6f, 0.2f));
        }

        // 🔹 Icon pop-in (scale + fade)
        sequence.Append(powerupIcon.DOFade(1f, 0.2f));
        sequence.Join(
            powerupIcon.transform.DOScale(1.2f, animationDuration)
            .SetEase(Ease.OutBack)
        );

        // 🔹 Settle scale
        sequence.Append(
            powerupIcon.transform.DOScale(1f, 0.2f)
        );

        // 🔹 Text fade + slight move
        sequence.Append(
            powerupNameText.DOFade(1f, 0.3f)
        );
        sequence.Join(
            powerupNameText.transform.DOLocalMoveY(
                powerupNameText.transform.localPosition.y + 20f, 
                0.3f
            ).From()
        );

        sequence.Append(
            powerupDescText.DOFade(1f, 0.3f)
        );
        sequence.Join(
            powerupDescText.transform.DOLocalMoveY(
                powerupDescText.transform.localPosition.y + 15f, 
                0.3f
            ).From()
        );

        // 🔹 Wait
        sequence.AppendInterval(displayTime);

        // 🔻 Exit animation
        sequence.Append(powerupIcon.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack));
        sequence.Join(powerupIcon.DOFade(0f, 0.2f));

        sequence.Join(powerupNameText.DOFade(0f, 0.2f));
        sequence.Join(powerupDescText.DOFade(0f, 0.2f));

        if (background != null)
            sequence.Join(background.DOFade(0f, 0.3f));

        // 🔹 Disable after finish
        sequence.OnComplete(() =>
        {
            container.SetActive(false);
        });
    }
}