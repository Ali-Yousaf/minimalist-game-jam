using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MazeManager : MonoBehaviour
{
    public static MazeManager Instance;

    [Header("Maze Settings")]
    [SerializeField] private int thresholdForMaze = 501;
    [SerializeField] private GameObject mazeGameObject;

    [Header("Lighting")]
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Light2D flashlight;
    [SerializeField] private Transform player;

    [Header("Flicker UI")]
    [SerializeField] private CanvasGroup blackScreen;

    [Header("Flicker Settings")]
    [SerializeField] private int flickerCount = 6;
    [SerializeField] private float minFlickerTime = 0.05f;
    [SerializeField] private float maxFlickerTime = 0.15f;

    private bool hasTriggered = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Normal state
        globalLight.intensity = 1f;
        flashlight.gameObject.SetActive(false);

        if (blackScreen != null)
            blackScreen.alpha = 0f;
    }

    void Update()
    {
        if (!hasTriggered && PlayerController.Instance.killCounter >= thresholdForMaze)
        {
            hasTriggered = true;
            StartCoroutine(TriggerMazeSequence());
        }
    }

    void LateUpdate()
    {
        // Flashlight follow + rotate
        if (flashlight != null && flashlight.gameObject.activeSelf)
        {
            flashlight.transform.position = player.position;
            RotateFlashlightToMouse();
        }
    }

    private IEnumerator TriggerMazeSequence()
    {
        // Flicker effect
        yield return StartCoroutine(PlayLightBlinkingAnim());

        // Enable maze gameplay
        EnableMaze();

        // Fade to darkness
        yield return StartCoroutine(FadeGlobalLight(0f, 0.4f));

        // Small delay for suspense
        yield return new WaitForSeconds(0.2f);

        // Turn on flashlight
        EnableFlashlightMode();
    }

    private void EnableMaze()
    {
        mazeGameObject.SetActive(true);
        Spawner.Instance.spawningEnabled = false;
    }

    private IEnumerator PlayLightBlinkingAnim()
    {
        for (int i = 0; i < flickerCount; i++)
        {
            // Lights OFF
            if (blackScreen != null)
                blackScreen.alpha = 1f;

            globalLight.intensity = 0f;

            yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));

            // Lights ON
            if (blackScreen != null)
                blackScreen.alpha = 0f;

            globalLight.intensity = 1f;

            yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));
        }

        // Final blackout
        if (blackScreen != null)
            blackScreen.alpha = 1f;

        globalLight.intensity = 0f;
    }

    private IEnumerator FadeGlobalLight(float target, float duration)
    {
        float start = globalLight.intensity;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            globalLight.intensity = Mathf.Lerp(start, target, time / duration);
            yield return null;
        }

        globalLight.intensity = target;
    }

    private void EnableFlashlightMode()
    {
        if (flashlight != null)
            flashlight.gameObject.SetActive(true);
    }

    private void RotateFlashlightToMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - flashlight.transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        flashlight.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}