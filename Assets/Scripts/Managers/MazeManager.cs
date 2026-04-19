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

    [Header("Flicker Settings")]
    [SerializeField] private int flickerCount = 8;
    [SerializeField] private float minFlickerTime = 0.05f;
    [SerializeField] private float maxFlickerTime = 0.15f;

    private MovieScreens movieScreens;

    private bool hasTriggered = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    
        movieScreens = FindAnyObjectByType<MovieScreens>();
    }

    void Start()
    {
        globalLight.intensity = 1f;
        flashlight.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!hasTriggered && PlayerController.Instance.killCounter >= thresholdForMaze)
        {
            hasTriggered = true;
            AudioManager.Instance.PlayMazeMusic();
            StartCoroutine(TriggerMazeSequence());
        }
    }

    void LateUpdate()
    {
        if (flashlight != null && flashlight.gameObject.activeSelf)
        {
            flashlight.transform.position = player.position;
            RotateFlashlightToMouse();
        }
    }

    private IEnumerator TriggerMazeSequence()
    {
        movieScreens.ActiveMovieScreens();
        yield return new WaitForSeconds(2);

        yield return StartCoroutine(FlickerLights());

        EnableMaze();
        GridJuiceFX.Instance.DisableAllGridEffects();

        // FULL DARK AFTER FLICKER
        globalLight.intensity = 0f;

        // Wait 3 seconds in darkness
        yield return new WaitForSeconds(3f);

        EnableFlashlightMode();
    }

    private void EnableMaze()
    {
        mazeGameObject.SetActive(true);
        Spawner.Instance.spawningEnabled = false;
    }

    private IEnumerator FlickerLights()
    {
        for (int i = 0; i < flickerCount; i++)
        {
            // Lights OFF
            globalLight.intensity = 0f;
            yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));

            // Lights ON
            globalLight.intensity = 1f;
            yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));
        }
    }

    private void EnableFlashlightMode()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.flashLightOnSFX);
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