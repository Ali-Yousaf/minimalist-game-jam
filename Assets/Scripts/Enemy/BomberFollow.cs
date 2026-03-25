using UnityEngine;

public class BomberFollow : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool rotateTowardsPlayer = true;

    [Header("Blink Settings")]
    [SerializeField] private float baseBlinkSpeed = 3f;
    [SerializeField] private float maxBlinkSpeed = 12f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private Color blinkColor = new Color(1f, 0.4f, 0f); // Dark Orange

    [Header("Glow Settings")]
    [SerializeField] private float baseEmission = 0.5f;
    [SerializeField] private float maxEmission = 3f;

    private Rigidbody2D rb;
    private Transform player;
    private EnemyKnockback knockback;
    private SpriteRenderer spriteRenderer;

    private Color originalColor;
    private Material materialInstance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knockback = GetComponent<EnemyKnockback>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;

            // Create material instance so we don't overwrite shared material
            materialInstance = spriteRenderer.material;
        }
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        if (knockback != null && knockback.IsStunned)
            return;

        MoveTowardsPlayer();
    }

    private void Update()
    {
        HandleBlinkingAndGlow();
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (rotateTowardsPlayer)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }
    }

    private void HandleBlinkingAndGlow()
    {
        if (spriteRenderer == null || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        float proximity = 1f - Mathf.Clamp01(distance / maxDistance);

        float blinkSpeed = Mathf.Lerp(baseBlinkSpeed, maxBlinkSpeed, proximity);

        float t = Mathf.PingPong(Time.time * blinkSpeed, 1f);

        spriteRenderer.color = Color.Lerp(originalColor, blinkColor, t);

        if (materialInstance != null && materialInstance.HasProperty("_EmissionColor"))
        {
            float emissionStrength = Mathf.Lerp(baseEmission, maxEmission, proximity * t);
            Color emissionColor = blinkColor * emissionStrength;

            materialInstance.SetColor("_EmissionColor", emissionColor);
        }
    }
}