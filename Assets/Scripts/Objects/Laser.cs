using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private int damage = 25;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float duration = 3f;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyHealth>()?.TakeDamage(damage);

            // Apply knockback
            EnemyKnockback knockback = collision.GetComponent<EnemyKnockback>();

            if (knockback != null)
            {
                Vector2 direction = (collision.transform.position - transform.position).normalized;
                knockback.ApplyKnockback(direction);
            }

            Destroy(gameObject);
        }
    }
}