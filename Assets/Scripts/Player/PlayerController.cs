using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    [Header("Boundary Settings")]
    [SerializeField] private PolygonCollider2D mapBoundary;

    [Header("Shooting")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireCooldown = 0.2f;

    [Header("Player Stats")]
    public int killCounter = 0;
    public int laserDamage = 25;
    public int maxBulletSpawners = 10;
    public int currentBulletSpawners = 1;

    private float fireTimer;
    [SerializeField] private bool canMove = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        fireTimer -= Time.deltaTime;

        HandleMovementInput();

        if (Input.GetMouseButton(0))
            ShootAt(Input.mousePosition);

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            ShootAt(Input.GetTouch(0).position);
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        newPosition = ClampInsideBoundary(newPosition);

        rb.MovePosition(newPosition);
    }

    private void HandleMovementInput()
    {
        if (!canMove)
        {
            movement = Vector2.zero;
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveX, moveY).normalized;
    }

    // =============================
    // PERFECT BOUNDARY SYSTEM
    // =============================

    private Vector2 ClampInsideBoundary(Vector2 targetPos)
    {
        if (mapBoundary == null)
            return targetPos;

        Bounds bounds = mapBoundary.bounds;

        float clampedX = Mathf.Clamp(targetPos.x, bounds.min.x, bounds.max.x);
        float clampedY = Mathf.Clamp(targetPos.y, bounds.min.y, bounds.max.y);

        return new Vector2(clampedX, clampedY);
    }

    // =============================
    // Shooting
    // =============================

    private void ShootAt(Vector2 screenPosition)
    {
        if (fireTimer > 0f) return;
        fireTimer = fireCooldown;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.laserShootSFX);

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPos.z = 0f;

        Vector2 direction = (worldPos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        firePoint.rotation = Quaternion.Euler(0f, 0f, angle);

        switch (currentBulletSpawners)
        {
            case 1:
                Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
                break;

            case 2:
                Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
                Instantiate(laserPrefab, firePoint.position,
                    firePoint.rotation * Quaternion.Euler(0, 0, 180f));
                break;

            case 4:
                Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
                Instantiate(laserPrefab, firePoint.position,
                    firePoint.rotation * Quaternion.Euler(0, 0, 180f));
                Instantiate(laserPrefab, firePoint.position,
                    firePoint.rotation * Quaternion.Euler(0, 0, 90f));
                Instantiate(laserPrefab, firePoint.position,
                    firePoint.rotation * Quaternion.Euler(0, 0, -90f));
                break;

            default:
                for (int i = 0; i < currentBulletSpawners; i++)
                {
                    float spread = 360f / currentBulletSpawners * i;
                    Instantiate(laserPrefab, firePoint.position,
                        firePoint.rotation * Quaternion.Euler(0, 0, spread));
                }
                break;
        }
    }

    // =============================
    // Upgrade Functions
    // =============================

    public void ReduceFireCooldown(float amount)
    {
        fireCooldown = Mathf.Max(0.05f, amount);
        Debug.Log("New fire cooldown: " + fireCooldown);
    }

    public void AddBulletSpawner()
    {
        if (currentBulletSpawners < maxBulletSpawners)
        {
            currentBulletSpawners++;
            Debug.Log("Bullet Spawners: " + currentBulletSpawners);
        }
    }

    public void IncreaseLaserDamage(int amount)
    {
        laserDamage += amount;
        Debug.Log("Laser damage: " + laserDamage);
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void IncreaseMovementSpeed(float amount)
    {
        moveSpeed += amount;
    }

    public void IncreaseShieldDuration()
    {
        var shield = FindFirstObjectByType<Shield>();
        shield.shieldDuration += 5;
    }
}