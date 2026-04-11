using UnityEngine;
using System.Collections;

public class TankEntryScene : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject movieCanvas;
    [SerializeField] private Camera mainCamera;

    [Header("Animator")]
    [SerializeField] private Animator movieAnimator;
    [SerializeField] private float goBackAnimDuration = 1f; // duration of slide-up animation

    [Header("Camera Settings")]
    [SerializeField] private float targetSize = 30f;
    [SerializeField] private float originalSize = 15f;
    [SerializeField] private float smoothTime = 0.5f;
    [SerializeField] private float waitTime = 2.5f;

    private float velocity = 0f;
    private bool zoomOut = false;
    private bool zoomIn = false;

    void Start()
    {
        canvas.SetActive(true);
        movieCanvas.SetActive(false);
        mainCamera.orthographicSize = originalSize;
    }

    void Update()
    {
        if (zoomOut)
        {
            mainCamera.orthographicSize = Mathf.SmoothDamp(
                mainCamera.orthographicSize,
                targetSize,
                ref velocity,
                smoothTime
            );
        }

        if (zoomIn)
        {
            mainCamera.orthographicSize = Mathf.SmoothDamp(
                mainCamera.orthographicSize,
                originalSize,
                ref velocity,
                smoothTime
            );
        }
    }

    public void ActivateEntryScene()
    {
        StartCoroutine(EntrySequence());
    }

    private IEnumerator EntrySequence()
    {
        canvas.SetActive(false);
        movieCanvas.SetActive(true);

        zoomOut = true;
        zoomIn = false;

        yield return new WaitForSeconds(1.5f);

        yield return new WaitForSeconds(waitTime);

        zoomOut = false;
        zoomIn = true;

        yield return new WaitForSeconds(1.5f);

        zoomIn = false;
        mainCamera.orthographicSize = originalSize;

        movieAnimator.SetTrigger("goBack");

        yield return new WaitForSeconds(goBackAnimDuration);

        movieCanvas.SetActive(false);
        canvas.SetActive(true);
    }
}