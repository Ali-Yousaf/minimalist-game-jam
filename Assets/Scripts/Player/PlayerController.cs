using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

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

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        fireTimer -= Time.deltaTime;

        if (Input.GetMouseButton(0))
            ShootAt(Input.mousePosition);

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            ShootAt(Input.GetTouch(0).position);
    }

    private void ShootAt(Vector2 screenPosition)
    {   
        if (fireTimer > 0f) return;
        fireTimer = fireCooldown;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.laserShootSFX);
        //GridJuiceFX.Instance.TriggerBurst();

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPos.z = 0f;

        Vector2 direction = (worldPos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        firePoint.rotation = Quaternion.Euler(0f, 0f, angle);

        // Shoot bullets based on the current pattern
        switch (currentBulletSpawners)
        {
            case 1:
                // Straight forward
                Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
                break;

            case 2:
                // Forward + backward
                Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
                Instantiate(laserPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, 180f));
                break;

            case 4:
                // Plus pattern
                Instantiate(laserPrefab, firePoint.position, firePoint.rotation); // forward
                Instantiate(laserPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, 180f)); // backward
                Instantiate(laserPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, 90f)); // up
                Instantiate(laserPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, -90f)); // down
                break;

            default:

                for (int i = 0; i < currentBulletSpawners; i++)
                {
                    float spread = 360f / currentBulletSpawners * i;
                    Instantiate(laserPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, spread));
                }
                break;
        }
    }

    // =============================
    // Upgrade Functions
    // =============================

    public void AddBulletSpawner()
    {
        if (currentBulletSpawners < maxBulletSpawners)
        {
            currentBulletSpawners++;
            Debug.Log("Bullet Spawners: " + currentBulletSpawners);
        }
    }

    public void ReduceFireCooldown(float amount)
    {
        fireCooldown = Mathf.Max(0.05f, fireCooldown - amount);
        Debug.Log("New fire cooldown: " + fireCooldown);
    }

    public void IncreaseLaserDamage(int amount)
    {
        laserDamage += amount;
        Debug.Log("Laser damage: " + laserDamage);
    }

    public void MaximizeBullets()
    {
        currentBulletSpawners = maxBulletSpawners;
        Debug.Log("Bullet spawners maxed!");
    }

    public void SuperFireRate()
    {
        fireCooldown = 0.05f;
        Debug.Log("Fire rate maxed!");
    }

    public void UltraLaser()
    {
        laserDamage += 50;
        Debug.Log("Laser damage greatly increased!");
    }
}