using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class Rocket : MonoBehaviour
{
    [SerializeField] private CircularFill fill; 
    [SerializeField] private Button rocketButton;

    [SerializeField] private GameObject powerup;
    public Sprite icon;

    private void Start()
    {
        powerup.SetActive(false);
    }

    public void Unlock()
    {
        powerup.SetActive(true);
        StartCoroutine(StartCircularFill());
    }

    public void ThrowRocket()
    {
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
}