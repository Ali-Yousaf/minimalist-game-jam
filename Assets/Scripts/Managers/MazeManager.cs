using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal; // For Light2D

public class MazeManager : MonoBehaviour
{
    public static MazeManager Instance;

    [Header("Maze Settings")]
    [SerializeField] private int thresholdForMaze = 501;
    [SerializeField] private GameObject mazeGameObject;

    [Header("Lighting")]
    [SerializeField] private CanvasGroup blackScreen; // Fullscreen UI (black image)
    [SerializeField] private Light2D flashlight;      // URP 2D Light
    [SerializeField] private Transform player;

    [Header("Flicker Settings")]
    [SerializeField] private int flickerCount = 6;
    [SerializeField] private float minFlickerTime = 0.05f;
    [SerializeField] private float maxFlickerTime = 0.2f;

    private bool hasTriggered = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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
        // Make flashlight follow player
        if (flashlight != null && flashlight.gameObject.activeSelf)
        {
            flashlight.transform.position = player.position;

            RotateFlashlightToMouse();
        }
    }

    private IEnumerator TriggerMazeSequence()
    {
        yield return StartCoroutine(PlayLightBlinkingAnim());

        EnableMaze();

        yield return new WaitForSeconds(0.2f); // small delay for effect

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
            blackScreen.alpha = 1f;
            yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));

            blackScreen.alpha = 0f;
            yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));
        }

        // Final blackout
        blackScreen.alpha = 1f;
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