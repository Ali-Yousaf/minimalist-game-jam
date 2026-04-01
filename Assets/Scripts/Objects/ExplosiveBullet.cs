using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class ExplosiveBullet : MonoBehaviour
{
    [SerializeField] private CircularFill fill; 
    [SerializeField] private Button button;
    public float bulletsDuration = 5f;
    
    [SerializeField] private GameObject lockedIcon;
    [SerializeField] private bool isLocked = true;

    private void Start()
    {
        UpdateLockState();

        if (!isLocked)
        {
            StartCoroutine(InitialFill());
        }
    }

    public void EnableExplosiveBullets()
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
            StartCoroutine(BulletsRoutine());
        }
        else
        {
            PlayErrorAnimation();
        }
    }

    private IEnumerator InitialFill()
    {
        yield return new WaitForSeconds(2f);

        if (!isLocked)
            fill.StartFill();
    }
    
    private IEnumerator BulletsRoutine()
    {
        PlayerController.Instance.explosiveBulletsEnabled = true;

        yield return new WaitForSeconds(bulletsDuration);
        
        PlayerController.Instance.explosiveBulletsEnabled = false;

        yield return new WaitForSeconds(2f);

        if (!isLocked)
            fill.StartFill();
    }

    private void PlayErrorAnimation()
    {
        button.transform.DOKill();
        button.transform
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
        button.interactable = !isLocked;
    }
}