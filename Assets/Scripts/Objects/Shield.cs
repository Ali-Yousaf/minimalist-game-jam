using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class Shield : MonoBehaviour
{
    [SerializeField] private CircularFill fill; 
    [SerializeField] private Button shieldButton;
    [SerializeField] private GameObject playerShield;
    public float shieldDuration = 5f;

    [SerializeField] private GameObject lockedIcon;
    [SerializeField] private bool isLocked = true; // default locked

    private void Start()
    {
        playerShield.SetActive(false);

        UpdateLockState();

        if (!isLocked)
        {
            StartCoroutine(InitialFill());
        }
    }

    public void ActivateShield()
    {
        if (isLocked)
        {
            PlayErrorAnimation();
            return;
        }

        if (fill == null) return;

        if (fill.IsFull())
        {
            fill.ResetFill();
            StartCoroutine(ShieldRoutine());
        }
        else
        {
            PlayErrorAnimation();
        }
    }

    private IEnumerator InitialFill()
    {
        yield return new WaitForSeconds(2f);
        fill.StartFill();
    }

    private IEnumerator ShieldRoutine()
    {
        playerShield.SetActive(true);

        yield return new WaitForSeconds(shieldDuration);

        playerShield.SetActive(false);

        yield return new WaitForSeconds(2f);

        if (!isLocked) // safety check
            fill.StartFill();
    }

    private void PlayErrorAnimation()
    {
        shieldButton.transform.DOKill();
        shieldButton.transform
            .DOShakeScale(0.5f, 0.15f, 10)
            .SetEase(Ease.InOutQuad);

        fill.FlashRed();
    }

    public void Unlock()
    {
        if (!isLocked) return;

        isLocked = false;
        UpdateLockState();

        StartCoroutine(InitialFill());
    }

    private void UpdateLockState()
    {
        lockedIcon.SetActive(isLocked);
        shieldButton.interactable = !isLocked;
    }
}