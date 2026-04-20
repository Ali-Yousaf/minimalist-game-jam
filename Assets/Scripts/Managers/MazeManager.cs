using System.Collections;
using TMPro;
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

    [Header("UI Elemets")]
    [SerializeField] private GameObject mazeEntryText;

    [Header("Child Elements")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject mapBoundary;
    [SerializeField] private GameObject Background;   

    private MovieScreens movieScreens;

    private bool hasTriggered = false;

    // ✅ Store original parents
    private Transform camOriginalParent;
    private Transform boundaryOriginalParent;
    private Transform bgOriginalParent;

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
        mazeGameObject.SetActive(false);
    }

    void Update()
    {
        if (!hasTriggered && PlayerController.Instance.killCounter >= thresholdForMaze)
        {
            hasTriggered = true;
            
            AudioManager.Instance.PlayMazeMusic();
            Spawner.Instance.spawningEnabled = false;
            
            StartCoroutine(TriggerMazeSequence());
        }
    }

    void LateUpdate()
    {
        if (flashlight != null && flashlight.gameObject.activeSelf)
        {
            flashlight.transform.position = player.position;
        }
    }

    private IEnumerator TriggerMazeSequence()
    {
        movieScreens.ActiveMovieScreens();
        yield return new WaitForSeconds(2.5f);

        mazeEntryText.SetActive(true);
        globalLight.intensity = 1f;

        yield return StartCoroutine(FlickerLights());

        EnableMaze();
        SetChildElements(); // ✅ Attach to player
        GridJuiceFX.Instance.DisableAllGridEffects();

        globalLight.intensity = 0f;

        yield return new WaitForSeconds(4f);

        mazeEntryText.SetActive(false);
        EnableFlashlightMode();
    }

    private void EnableMaze()
    {
        mazeGameObject.SetActive(true);
    }

    private IEnumerator FlickerLights()
    {
        for (int i = 0; i < flickerCount; i++)
        {
            globalLight.intensity = 0f;
            yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));

            globalLight.intensity = 1f;
            yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));
        }
    }

    private void EnableFlashlightMode()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.flashLightOnSFX);
        flashlight.gameObject.SetActive(true);
    }

    // =========================
    // CHILD ELEMENT HANDLING
    // =========================

    private void SetChildElements()
    {
        // Store original parents
        camOriginalParent = mainCamera.transform.parent;
        boundaryOriginalParent = mapBoundary.transform.parent;
        bgOriginalParent = Background.transform.parent;

        // Parent to player (preserve world position)
        mainCamera.transform.SetParent(player, true);
        mapBoundary.transform.SetParent(player, true);
        Background.transform.SetParent(player, true);
    }

    private void RemoveChildElements()
    {
        mainCamera.transform.SetParent(camOriginalParent, true);
        mapBoundary.transform.SetParent(boundaryOriginalParent, true);
        Background.transform.SetParent(bgOriginalParent, true);
    }
}