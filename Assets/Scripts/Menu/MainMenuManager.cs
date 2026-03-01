using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject rulesPanel;
    [SerializeField] private float animationDuration = 0.35f;

    private CanvasGroup rulesCanvasGroup;
    private RectTransform rulesRect;

    private EnemiesMovement menuSpawner;

    private void Awake()
    {   
        menuSpawner = FindFirstObjectByType<EnemiesMovement>();

        rulesCanvasGroup = rulesPanel.GetComponent<CanvasGroup>();
        rulesRect = rulesPanel.GetComponent<RectTransform>();

        // Ensure starting state
        rulesPanel.SetActive(false);
        rulesCanvasGroup.alpha = 0f;
        rulesRect.localScale = Vector3.one * 0.8f;
    }

    public void OpenRulesPanel()
    {
        menuSpawner.spawningEnabled = false;
        rulesPanel.SetActive(true);

        rulesCanvasGroup.alpha = 0f;
        rulesRect.localScale = Vector3.one * 0.8f;

        Sequence seq = DOTween.Sequence();

        seq.Append(rulesCanvasGroup.DOFade(1f, animationDuration));
        seq.Join(rulesRect.DOScale(1f, animationDuration).SetEase(Ease.OutBack));
    }

    public void CloseRulesPanel()
    {
        menuSpawner.spawningEnabled = true;
        Sequence seq = DOTween.Sequence();

        seq.Append(rulesCanvasGroup.DOFade(0f, animationDuration));
        seq.Join(rulesRect.DOScale(0.8f, animationDuration).SetEase(Ease.InBack));

        seq.OnComplete(() =>
        {
            rulesPanel.SetActive(false);
        });
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}