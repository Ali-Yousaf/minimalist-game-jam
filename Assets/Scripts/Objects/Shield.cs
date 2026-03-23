using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class Shield : MonoBehaviour
{
    [SerializeField] private CircularFill fill; 
    [SerializeField] private Button shieldButton;
    [SerializeField] private GameObject playerShield;
    [SerializeField] private float shieldDuration = 5f;

    private void Awake()
    {
        if (shieldButton != null)
        {
            StartCoroutine(StartCircularFill());
        }

        playerShield.SetActive(false);
    }

    public void ActivateShield()
    {
        if (fill == null) return;

        if (fill.IsFull())
        {
            ActivePlayerShield();  
            fill.ResetFill(); 
            StartCoroutine(StartCircularFill());
        }
        else
        {
            shieldButton.transform.DOKill();
            shieldButton.transform
                .DOShakeScale(0.5f, 0.15f, 10)
                .SetEase(Ease.InOutQuad);

            fill.FlashRed(); 
        }
    }

    IEnumerator StartCircularFill()
    {
        yield return new WaitForSeconds(2f);
        fill.StartFill();
    }

    private void ActivePlayerShield()
    {
        StartCoroutine(ShieldRoutine());
    }

    private IEnumerator ShieldRoutine()
    {
        playerShield.SetActive(true);
        yield return new WaitForSeconds(shieldDuration);
        playerShield.SetActive(false);
    }
}
            