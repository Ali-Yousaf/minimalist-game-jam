using UnityEngine;

public class TankHealth : MonoBehaviour
{
    [SerializeField] public float maxHealth = 1000f;
    [SerializeField] private TankHealthBar healthBar;

    public float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);
    }

    // =============================
    // TAKE DAMAGE
    // =============================
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // =============================
    // DIE
    // =============================
    private void Die()
    {
        print("Tank Exploded");
        BossFightManager.Instance.BossDied();
        Destroy(gameObject, 1f);
    }
}