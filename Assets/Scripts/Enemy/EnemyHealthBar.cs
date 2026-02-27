using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float lerpSpeed = 8f;

    private float targetValue;
    private float maxHealth;

    public void Initialize(float maxHealth)
    {
        this.maxHealth = maxHealth;

        slider.minValue = 0f;
        slider.maxValue = 100f;
        slider.value = 100f;   // always starts full
        targetValue = 100f;

        slider.gameObject.SetActive(false); // hidden at start
    }

    private void Update()
    {
        slider.value = Mathf.Lerp(
            slider.value,
            targetValue,
            Time.deltaTime * lerpSpeed
        );

        if (Mathf.Abs(slider.value - targetValue) < 0.1f)
            slider.value = targetValue;
    }

    public void UpdateHealth(float currentHealth)
    {
        // normalize to 0–100 range
        targetValue = (currentHealth / maxHealth) * 100f;

        slider.gameObject.SetActive(true); // reveal on first damage
    }
}