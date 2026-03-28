using UnityEngine;
using UnityEngine.UI;

public class TankHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float lerpSpeed = 5f;

    private float targetValue;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        targetValue = health;
    }

    public void SetHealth(float health)
    {
        targetValue = health;
    }

    private void Update()
    {
        if (slider.value != targetValue)
        {
            slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * lerpSpeed);

            if (Mathf.Abs(slider.value - targetValue) < 0.01f)
                slider.value = targetValue;
        }
    }
}