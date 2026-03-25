using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem explodeParticle;

    [SerializeField] private KillCounter.EnemyType enemyType;

    private float currentHealth;
    private EnemyHealthBar healthBar;
    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;

        healthBar = GetComponentInChildren<EnemyHealthBar>();
        if (healthBar != null)
            healthBar.Initialize(maxHealth);

        if (explodeParticle != null)
            explodeParticle.Stop();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        CameraShake.Instance.Shake(0.1f, 0.1f);

        healthBar?.UpdateHealth(currentHealth);

        if (currentHealth <= 0f)
            Die();
    }

    public void Die()
    {
        isDead = true;
        DisableVisualsAndPlayAudio();

        if (explodeParticle != null)
        {
            explodeParticle.transform.parent = null;
            explodeParticle.Play();

            Destroy(explodeParticle.gameObject,
                explodeParticle.main.duration +
                explodeParticle.main.startLifetime.constantMax);
        }

        Destroy(gameObject);
    }

    private void DisableVisualsAndPlayAudio()
    {
        GridJuiceFX.Instance.TriggerBurst();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = false;

        healthBar?.gameObject.SetActive(false);

        AudioManager.Instance.PlaySFX(AudioManager.Instance.explosionSFX);

        KillCounter.Instance.AddKill(enemyType);
        PlayerController.Instance.killCounter++;
        ScoreManager.SaveHighScore();
        StatsUI stats = FindFirstObjectByType<StatsUI>();
        stats?.UpdateStatsUI();
    }
}