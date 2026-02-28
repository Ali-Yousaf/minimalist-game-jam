using UnityEngine;
using System.Collections;

public class GridJuiceFX : MonoBehaviour
{
    public static GridJuiceFX Instance;

    [Header("===== EFFECT TOGGLES =====")]
    public bool enableWiggle = true;
    public bool enableRotation = true;
    public bool enablePulse = true;
    public bool enableReactiveBurst = true;
    public bool enableColorShift = false;

    [Header("===== FLASH SETTINGS =====")]
    public Color flashColor = Color.red;   // 👈 Change in Inspector
    [SerializeField] private float defaultFlashDuration = 0.1f;

    [Header("===== WIGGLE SETTINGS =====")]
    [SerializeField] private float wiggleAmount = 0.05f;
    [SerializeField] private float wiggleSpeed = 0.5f;

    [Header("===== ROTATION SETTINGS =====")]
    [SerializeField] private float rotationSpeed = 2f;

    [Header("===== PULSE SETTINGS =====")]
    [SerializeField] private float pulseSpeed = 1f;
    [SerializeField] private float pulseAmount = 0.03f;

    [Header("===== COLOR SHIFT SETTINGS =====")]
    [SerializeField] private float colorShiftSpeed = 1f;

    [Header("===== KILL SCALING SETTINGS =====")]
    [SerializeField] private int killsPerMilestone = 10;
    [SerializeField] private float maxWiggleAmount = 0.15f;
    [SerializeField] private float maxRotationSpeed = 10f;
    [SerializeField] private float maxPulseAmount = 0.1f;
    [SerializeField] private float wiggleIncreaseStep = 0.01f;
    [SerializeField] private float rotationIncreaseStep = 1f;
    [SerializeField] private float pulseIncreaseStep = 0.02f;
    [SerializeField] private float transitionSpeed = 2f;

    private Vector3 startPos;
    private Vector3 originalScale;
    private Color originalColor;
    private SpriteRenderer sr;

    private Coroutine flashRoutine;
    private bool isFlashing = false;

    private int currentMilestone = 0;

    private float targetWiggle;
    private float targetRotation;
    private float targetPulse;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startPos = transform.position;
        originalScale = transform.localScale;

        if (sr != null)
            originalColor = sr.color;

        targetWiggle = wiggleAmount;
        targetRotation = rotationSpeed;
        targetPulse = pulseAmount;
    }

    private void Update()
    {
        // Smoothly lerp current values toward targets
        wiggleAmount = Mathf.Lerp(wiggleAmount, targetWiggle, Time.deltaTime * transitionSpeed);
        rotationSpeed = Mathf.Lerp(rotationSpeed, targetRotation, Time.deltaTime * transitionSpeed);
        pulseAmount = Mathf.Lerp(pulseAmount, targetPulse, Time.deltaTime * transitionSpeed);

        HandleWiggle();
        HandleRotation();
        HandlePulse();
        HandleColorShift();

        // Optional: automatically check kills each frame
        CheckKillMilestone(PlayerController.Instance.killCounter);
    }

    private void HandleWiggle()
    {
        if (!enableWiggle) return;

        float x = Mathf.Sin(Time.time * wiggleSpeed) * wiggleAmount;
        float y = Mathf.Cos(Time.time * wiggleSpeed * 0.7f) * wiggleAmount;

        transform.position = startPos + new Vector3(x, y, 0f);
    }

    private void HandleRotation()
    {
        if (!enableRotation) return;

        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    private void HandlePulse()
    {
        if (!enablePulse) return;

        float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = originalScale * scale;
    }

    private void HandleColorShift()
    {
        if (!enableColorShift || sr == null || isFlashing) return;

        float hue = Mathf.PingPong(Time.time * colorShiftSpeed, 1f);
        sr.color = Color.HSVToRGB(hue, 1f, 1f);
    }

    public void TriggerBurst()
    {
        if (!enableReactiveBurst) return;

        StopCoroutine(nameof(BurstPulse));
        StartCoroutine(BurstPulse());
    }

    private IEnumerator BurstPulse()
    {
        float timer = 0f;
        float duration = 0.2f;

        while (timer < duration)
        {
            float scale = 1 + Mathf.Sin(timer * 50f) * 0.05f;
            transform.localScale = originalScale * scale;
            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
    }

    public void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine(defaultFlashDuration));
    }

    private IEnumerator FlashRoutine(float duration)
    {
        if (sr == null) yield break;

        isFlashing = true;

        sr.color = flashColor;

        yield return new WaitForSeconds(duration);

        isFlashing = false;

        if (!enableColorShift)
            sr.color = originalColor;
    }

    // Call this to check milestone and increase intensity
    public void CheckKillMilestone(int totalKills)
    {
        int milestone = totalKills / killsPerMilestone;

        if (milestone > currentMilestone)
        {
            currentMilestone = milestone;
            IncreaseIntensity();
        }
    }

    private void IncreaseIntensity()
    {
        targetWiggle += wiggleIncreaseStep;
        targetRotation += rotationIncreaseStep;
        targetPulse += pulseIncreaseStep;

        targetWiggle = Mathf.Min(targetWiggle, maxWiggleAmount);
        targetRotation = Mathf.Min(targetRotation, maxRotationSpeed);
        targetPulse = Mathf.Min(targetPulse, maxPulseAmount);
    }
}