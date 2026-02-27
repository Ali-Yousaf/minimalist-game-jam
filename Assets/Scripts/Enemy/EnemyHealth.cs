using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;
    private EnemyHealthBar healthBar;

    private void Awake()
    {
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<EnemyHealthBar>();

        if (healthBar != null)
            healthBar.Initialize(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        CameraShake.Instance.Shake(0.1f, 0.1f);

        healthBar?.UpdateHealth(currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        PlayerController.Instance.killCounter++;
        Destroy(gameObject);
    }
}