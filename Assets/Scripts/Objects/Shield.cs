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

    private void Start()
    {
        playerShield.SetActive(false);
        StartCoroutine(InitialFill());
    }

    public void ActivateShield()
    {
        if (fill == null) return;

        if (fill.IsFull())
        {
            fill.ResetFill();
            StartCoroutine(ShieldRoutine());
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
        fill.StartFill();
    }
}