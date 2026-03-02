using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CircularFill : MonoBehaviour
{
    public Image fillImage;   
    public Image powerupImage;   
    public float fillDuration = 5f;
    private float fillAmount = 0f;
    private bool isFilling = false;

    private Color originalColor;

    void Start()
    {
        originalColor = powerupImage.color;
        powerupImage.color = Color.gray;
    }

    void Update()
    {
        if (isFilling)
        {
            fillAmount += Time.deltaTime / fillDuration;
            fillImage.fillAmount = Mathf.Clamp01(fillAmount);

            if (fillAmount >= 1f)
            {
                isFilling = false;
                powerupImage.color = originalColor; // Restore color when full
            }
        }
    }

    public void FlashRed()
    {
        powerupImage.DOKill();
        powerupImage.color = Color.red;
        powerupImage.DOColor(Color.gray, 0.3f); // Fade back to gray
    }

    public void StartFill()
    {
        fillAmount = 0f;
        fillImage.fillAmount = 0f;
        isFilling = true;
    }

    public void ResetFill()
    {
        fillAmount = 0f;
        fillImage.fillAmount = 0f;
        isFilling = false;
        powerupImage.DOKill();
        powerupImage.color = Color.gray; // ← Reset to gray on reset
    }

    public bool IsFull()
    {
        return fillAmount >= 1f;
    }
}