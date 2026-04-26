using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    [SerializeField] public float maxHealth = 1000f;
    [SerializeField] private TankHealthBar healthBar;

    [SerializeField] private ParticleSystem deathParticle1;
    [SerializeField] private ParticleSystem deathParticle2;

    [SerializeField] private SpriteRenderer Hull;
    [SerializeField] private SpriteRenderer Gun;
    [SerializeField] private Sprite rewardIcon;
    private TankMovement tankMovement;
    public float currentHealth;


    void Awake()
    {
        tankMovement = GetComponent<TankMovement>();
    }

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
            tankMovement.canShoot = false;
            Die();
        }
    }

    // =============================
    // DIE
    // =============================
    private void Die()
    {
        print("Tank Exploded");
        UnlockPowerup();

        Hull.enabled = false;
        Gun.enabled = false;


        BossFightManager.Instance.BossDied();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.explosionSFX);
        deathParticle1.Play();
        deathParticle2.Play();

        Destroy(gameObject, 2f);
    }

    private void UnlockPowerup()
    {
        if (RewardUnlockSequence.Instance == null)
        {
            Debug.LogError("RewardUnlockSequence is NULL!");
            return;
        }

        RewardUnlockSequence.Instance.PlayUnlock(
            rewardIcon,
            "Brozen Key.",
            "First step towards freedom."
        );
    }
}