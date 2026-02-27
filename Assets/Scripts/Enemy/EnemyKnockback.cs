using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyKnockback : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 6f;
    [SerializeField] private float stunDuration = 0.2f;

    private Rigidbody2D rb;
    private float stunTimer;

    public bool IsStunned => stunTimer > 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (stunTimer > 0f)
            stunTimer -= Time.deltaTime;
    }

    public void ApplyKnockback(Vector2 hitDirection)
    {
        stunTimer = stunDuration;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(hitDirection * knockbackForce, ForceMode2D.Impulse);
    }
}