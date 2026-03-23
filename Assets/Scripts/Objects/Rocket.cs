using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class Rocket : MonoBehaviour
{
    [SerializeField] private CircularFill fill; 
    [SerializeField] private Button rocketButton;

    private void Awake()
    {
        if (rocketButton != null)
        {
            StartCoroutine(StartCircularFill());
        }
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
            // Shake + flash red while still filling
            rocketButton.transform.DOKill();
            rocketButton.transform
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

            yield return new WaitForSeconds(0.3f); // delay between each enemy
        }
    }
}
            