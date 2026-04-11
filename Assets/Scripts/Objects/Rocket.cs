using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class Rocket : MonoBehaviour
{
    [SerializeField] private CircularFill fill; 
    [SerializeField] private Button rocketButton;

    [SerializeField] private GameObject lockedIcon;
    [SerializeField] private bool isLocked = true;

    private void Start()
    {
        UpdateLockState();

        if (!isLocked)
        {
            StartCoroutine(StartCircularFill());
        }
    }

    public void ThrowRocket()
    {
        if (isLocked)
        {
            PlayErrorAnimation();
            return;
        }

        if (fill == null) return;

        if (fill.IsFull())
        {
            FireRocket();  
            fill.ResetFill(); 
            StartCoroutine(StartCircularFill());
        }
        else
        {
            PlayErrorAnimation();
        }
    }

    IEnumerator StartCircularFill()
    {
        yield return new WaitForSeconds(2f);

        if (!isLocked)
            fill.StartFill();
    }

    private void FireRocket()
    {
        StartCoroutine(DestroyEnemiesSequentially());
    }

    private IEnumerator DestroyEnemiesSequentially()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy == null) continue;

            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.Die();
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    private void PlayErrorAnimation()
    {
        rocketButton.transform.DOKill();
        rocketButton.transform
            .DOShakeScale(0.5f, 0.15f, 10)
            .SetEase(Ease.InOutQuad);

        fill.FlashRed();
    }

    public void Unlock()
    {
        if (!isLocked) return;

        isLocked = false;
        UpdateLockState();

        StartCoroutine(StartCircularFill());
    }

    private void UpdateLockState()
    {
        lockedIcon.SetActive(isLocked);
        rocketButton.interactable = !isLocked;
    }
}