using UnityEngine;
using System.Collections;

public class MenuBGJuice : MonoBehaviour
{
    [Header("==== EFFECT TOGGLES ====")]
    public bool enablePulse = true;
    public bool enableFloat = true;
    public bool enableBounce = false;
    public bool enableRotation = false;
    public bool enableTilt = false;

    [Header("==== PULSE SETTINGS ====")]
    [SerializeField] private float pulseSpeed = 1f;
    [SerializeField] private float pulseAmount = 0.05f;

    [Header("==== FLOAT SETTINGS ====")]
    [SerializeField] private float floatSpeed = 0.5f;
    [SerializeField] private float floatAmount = 10f;

    [Header("==== BOUNCE SETTINGS ====")]
    [SerializeField] private float bounceSpeed = 2f;
    [SerializeField] private float bounceAmount = 15f;

    [Header("==== ROTATION SETTINGS ====")]
    [SerializeField] private float rotationSpeed = 5f;

    [Header("==== TILT SETTINGS ====")]
    [SerializeField] private float tiltAmount = 5f;
    [SerializeField] private float tiltSpeed = 1f;

    private RectTransform rect;
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Vector3 originalRotation;

    private void Start()
    {
        rect = GetComponent<RectTransform>();

        originalScale = rect.localScale;
        originalPosition = rect.localPosition;
        originalRotation = rect.localEulerAngles;
    }

    private void Update()
    {
        HandlePulse();
        HandleFloat();
        HandleBounce();
        HandleRotation();
        HandleTilt();
    }

    private void HandlePulse()
    {
        if (!enablePulse) return;

        float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        rect.localScale = originalScale * scale;
    }

    private void HandleFloat()
    {
        if (!enableFloat) return;

        float x = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        float y = Mathf.Cos(Time.time * floatSpeed * 0.7f) * floatAmount;

        rect.localPosition = originalPosition + new Vector3(x, y, 0f);
    }

    private void HandleBounce()
    {
        if (!enableBounce) return;

        float y = Mathf.Abs(Mathf.Sin(Time.time * bounceSpeed)) * bounceAmount;
        rect.localPosition = originalPosition + new Vector3(0f, y, 0f);
    }

    private void HandleRotation()
    {
        if (!enableRotation) return;

        rect.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    private void HandleTilt()
    {
        if (!enableTilt) return;

        float tilt = Mathf.Sin(Time.time * tiltSpeed) * tiltAmount;
        rect.localEulerAngles = new Vector3(tilt, 0f, originalRotation.z);
    }

    public void TriggerBurst()
    {
        StopAllCoroutines();
        StartCoroutine(Burst());
    }

    private IEnumerator Burst()
    {
        float timer = 0f;
        float duration = 0.2f;

        while (timer < duration)
        {
            float scale = 1 + Mathf.Sin(timer * 50f) * 0.1f;
            rect.localScale = originalScale * scale;
            timer += Time.deltaTime;
            yield return null;
        }

        rect.localScale = originalScale;
    }
}