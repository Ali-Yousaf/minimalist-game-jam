using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [Header("Flash Settings")]
    [SerializeField] private float flashDuration = 0.1f;

    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float shakeMagnitude = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Vector3 originalPosition;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        originalPosition = transform.localPosition;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Player Died!");
            return;
        }

        currentHealth -= amount;

        // Screen shake (already in your code)
        CameraShake.Instance.Shake(0.2f, 0.2f);

        // Optional: decrease kills
        if (PlayerController.Instance != null)
            PlayerController.Instance.killCounter--;

        // Flash red
        if (spriteRenderer != null)
            StartCoroutine(FlashRed());

        // Player local shake
        StartCoroutine(ShakePlayer());
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    private IEnumerator ShakePlayer()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = originalPosition + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}