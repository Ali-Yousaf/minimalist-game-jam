using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ZigZagEnemyFollow : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float zigzagAmplitude = 2f;
    [SerializeField] private float zigzagFrequency = 2f;

    private Rigidbody2D rb;
    private Transform player;
    private float zigzagTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("Player not found! Assign tag 'Player'");
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = ((Vector2)player.position - rb.position).normalized;

        // Perpendicular axis to the forward direction
        Vector2 perp = new Vector2(-direction.y, direction.x);

        zigzagTimer += Time.fixedDeltaTime * zigzagFrequency;

        // zigzagOffset is now a velocity contribution, not a position delta
        Vector2 zigzagOffset = perp * Mathf.Sin(zigzagTimer) * zigzagAmplitude;

        rb.linearVelocity = direction * moveSpeed + zigzagOffset;

        // Rotate to face actual movement direction
        float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90f; // subtract 90 if your sprite faces up, remove if it faces right
    }
}