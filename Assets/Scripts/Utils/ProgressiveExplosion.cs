using UnityEngine;
using System.Collections;

public class ProgressiveExplosion : MonoBehaviour
{
    public float maxRadius = 5f;
    public float expansionTime = 1f;
    public int damage = 50;
    public LayerMask enemyLayer;

    private SphereCollider sphere;

    private void Awake()
    {
        sphere = gameObject.AddComponent<SphereCollider>();
        sphere.isTrigger = true;
        sphere.radius = 0f;
    }

    private void Start()
    {
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        float timer = 0f;
        while (timer < expansionTime)
        {
            timer += Time.deltaTime;
            sphere.radius = Mathf.Lerp(0f, maxRadius, timer / expansionTime);

            // Check for enemies every frame
            Collider[] enemies = Physics.OverlapSphere(transform.position, sphere.radius, enemyLayer);
            foreach (Collider enemy in enemies)
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
            }

            yield return null;
        }

        // Optional: Destroy explosion object after finished
        Destroy(gameObject, 0.5f);
    }
}