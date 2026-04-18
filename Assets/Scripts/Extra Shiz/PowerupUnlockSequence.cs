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
        sequence?.Kill();
        
        // if (Spawner.Instance != null)
        //     Spawner.Instance.enableSpawning = false;

        container.SetActive(true);

        powerupIcon.sprite = icon;
        powerupNameText.text = title;
        powerupDescText.text = description;

        powerupIcon.transform.localScale = Vector3.zero;
        powerupIcon.color = new Color(1,1,1,0);

        powerupNameText.alpha = 0;
        powerupDescText.alpha = 0;

        if (background != null)
            background.color = new Color(0,0,0,0);

        sequence = DOTween.Sequence();

        if (background != null)
            sequence.Append(background.DOFade(0.6f, 0.2f));

        sequence.Append(powerupIcon.DOFade(1f, 0.2f));
        sequence.Join(
            powerupIcon.transform.DOScale(1.2f, animationDuration)
            .SetEase(Ease.OutBack)
        );

        sequence.Append(powerupIcon.transform.DOScale(1f, 0.2f));

        sequence.Append(powerupNameText.DOFade(1f, 0.3f));
        sequence.Join(
            powerupNameText.transform.DOLocalMoveY(
                powerupNameText.transform.localPosition.y + 20f, 
                0.3f
            ).From()
        );

        sequence.Append(powerupDescText.DOFade(1f, 0.3f));
        sequence.Join(
            powerupDescText.transform.DOLocalMoveY(
                powerupDescText.transform.localPosition.y + 15f, 
                0.3f
            ).From()
        );

        // Stay visible for 3 seconds
        sequence.AppendInterval(3f);

        // Exit 
        sequence.Append(powerupIcon.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack));
        sequence.Join(powerupIcon.DOFade(0f, 0.2f));

        sequence.Join(powerupNameText.DOFade(0f, 0.2f));
        sequence.Join(powerupDescText.DOFade(0f, 0.2f));

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