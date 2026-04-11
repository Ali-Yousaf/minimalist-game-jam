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
        // 🔥 Switch to cinematic UI
        canvas.SetActive(false);
        movieCanvas.SetActive(true);

        // (Default animation plays automatically: bars slide down)

        // Step 1: Zoom out
        zoomOut = true;
        zoomIn = false;

        yield return new WaitForSeconds(1.5f);

        // Step 2: Hold for animation
        yield return new WaitForSeconds(waitTime);

        // Step 3: Zoom back in
        zoomOut = false;
        zoomIn = true;

        yield return new WaitForSeconds(1.5f);

        zoomIn = false;
        mainCamera.orthographicSize = originalSize;

        // 🔥 Play "go back" animation (bars slide up)
        movieAnimator.SetTrigger("goBack");

        // Wait for animation to finish
        yield return new WaitForSeconds(goBackAnimDuration);

        // 🔥 Restore HUD
        movieCanvas.SetActive(false);
        canvas.SetActive(true);
    }
}