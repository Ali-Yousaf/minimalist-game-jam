using UnityEngine;

public class TankEntryScene : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private Camera mainCamera;

    [Header("Camera Settings")]
    [SerializeField] private float targetSize = 30f;
    [SerializeField] private float originalSize = 15f;
    [SerializeField] private float smoothTime = 0.5f;

    private float velocity = 0f;
    private bool isActive = false;

    void Start()
    {
        canvas.SetActive(false);
        mainCamera.orthographicSize = originalSize;
    }

    void Update()
    {
        if (!isActive) return;

        mainCamera.orthographicSize = Mathf.SmoothDamp(
            mainCamera.orthographicSize,
            targetSize,
            ref velocity,
            smoothTime
        );
    }

    public void ActivateEntryScene()
    {
        canvas.SetActive(false);
        isActive = true;
    }
}