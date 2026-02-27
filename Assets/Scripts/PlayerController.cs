using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireCooldown = 0.2f;

    private float fireTimer;

    void Update()
    {
        fireTimer -= Time.deltaTime;

        if(Input.GetMouseButton(0))
        {
            ShootAt(Input.mousePosition);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ShootAt(Input.GetTouch(0).position);
        }
    }

    private void ShootAt(Vector2 screenPosition)
    {
        if (fireTimer > 0f)
        return;

        fireTimer = fireCooldown;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPos.z = 0f;

        Vector2 direction = (worldPos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        firePoint.rotation = Quaternion.Euler(0f, 0f, angle);

        Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
    }
}
