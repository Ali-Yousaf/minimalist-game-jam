using UnityEngine;
using System.Collections;

public class TankBoss : MonoBehaviour
{
    public enum TankState { Enter, Attack, Retreat }
    private TankState currentState;

    [Header("References")]
    [SerializeField] private Transform gun;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private SpriteRenderer bodyRenderer;

    private Transform player;
    private Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float retreatSpeed = 2f;
    [SerializeField] private float enterDistance = 3f;

    [Header("Attack")]
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private int burstCount = 20;

    [Header("Visuals")]
    [SerializeField] private float gunRotationOffset = -90f;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    [Header("Recoil Effect")]
    [SerializeField] private float recoilForce = 2f;
    [SerializeField] private float recoilDuration = 0.05f;
    [SerializeField] private float scaleMultiplier = 1.15f;
    [SerializeField] private float scaleReturnSpeed = 20f;

    private Vector2 targetPosition;
    private int currentSpawnIndex = -1;
    private bool isAttacking = false;

    private Color originalColor;
    private Vector3 originalScale;

    private Coroutine recoilRoutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (bodyRenderer != null)
            originalColor = bodyRenderer.color;

        originalScale = transform.localScale;

        PickNewSpawn();
        currentState = TankState.Enter;
    }

    private void Update()
    {
        if (player == null) return;

        RotateGunToPlayer();

        switch (currentState)
        {
            case TankState.Enter:
                HandleEnter();
                break;

            case TankState.Attack:
                break;

            case TankState.Retreat:
                HandleRetreat();
                break;
        }
    }

    // =============================
    // ENTER
    // =============================
    private void HandleEnter()
    {
        MoveTowards(targetPosition, moveSpeed);

        if (Vector2.Distance(transform.position, targetPosition) < 0.2f && !isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            StartCoroutine(AttackRoutine());
        }
    }

    // =============================
    // ATTACK
    // =============================
    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        currentState = TankState.Attack;

        // 🔥 Telegraph (flash before attack)
        yield return StartCoroutine(FlashEffect());

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < burstCount; i++)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }

        // 💥 BIG pushback after burst
        yield return StartCoroutine(PushBack());

        currentState = TankState.Retreat;
        PickNewSpawn();

        isAttacking = false;
    }

    // =============================
    // RETREAT
    // =============================
    private void HandleRetreat()
    {
        Transform spawn = spawnPoints[currentSpawnIndex];

        MoveTowards(spawn.position, retreatSpeed);

        if (Vector2.Distance(transform.position, spawn.position) < 0.2f)
        {
            PickNewSpawn();
            currentState = TankState.Enter;
        }
    }

    // =============================
    // MOVEMENT
    // =============================
    private void MoveTowards(Vector2 target, float speed)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * speed;

        if (dir != Vector2.zero)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }
    }

    // =============================
    // GUN ROTATION
    // =============================
    private void RotateGunToPlayer()
    {
        if (player == null || gun == null) return;

        Vector2 dir = (player.position - gun.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        gun.rotation = Quaternion.Euler(0, 0, angle + gunRotationOffset);
    }

    // =============================
    // SHOOT
    // =============================
    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // 🎥 Camera shake
        CameraShake.Instance?.Shake(0.05f, 0.1f);

        // 💥 Recoil punch (no overlap)
        if (recoilRoutine != null)
            StopCoroutine(recoilRoutine);

        recoilRoutine = StartCoroutine(RecoilEffect());
    }

    // =============================
    // RECOIL EFFECT (NEW 🔥)
    // =============================
    private IEnumerator RecoilEffect()
    {
        float timer = 0f;

        Vector2 dirAway = (transform.position - player.position).normalized;

        // instant scale up
        transform.localScale = originalScale * scaleMultiplier;

        while (timer < recoilDuration)
        {
            rb.linearVelocity = dirAway * recoilForce;

            timer += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;

        // smooth snap back
        float t = 0f;
        while (Vector3.Distance(transform.localScale, originalScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, t);
            t += Time.deltaTime * scaleReturnSpeed;
            yield return null;
        }

        transform.localScale = originalScale;
    }

    // =============================
    // FLASH EFFECT
    // =============================
    private IEnumerator FlashEffect()
    {
        if (bodyRenderer == null) yield break;

        bodyRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        bodyRenderer.color = originalColor;
    }

    // =============================
    // SPAWN
    // =============================
    private void PickNewSpawn()
    {
        int newIndex;

        do
        {
            newIndex = Random.Range(0, spawnPoints.Length);
        
        } while (newIndex == currentSpawnIndex);

        currentSpawnIndex = newIndex;

        Transform spawn = spawnPoints[currentSpawnIndex];
        transform.position = spawn.position;

        Vector2 dirToPlayer = (player.position - transform.position).normalized;
        targetPosition = (Vector2)spawn.position + dirToPlayer * enterDistance;
    }

    // =============================
    // PUSHBACK (BIG ONE)
    // =============================
    private IEnumerator PushBack()
    {
        float pushTime = 0.2f;
        float timer = 0f;

        Vector2 dirAway = (transform.position - player.position).normalized;

        CameraShake.Instance?.Shake(0.25f, 0.25f);

        while (timer < pushTime)
        {
            rb.linearVelocity = dirAway * (retreatSpeed * 2f);
            timer += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
    }
}