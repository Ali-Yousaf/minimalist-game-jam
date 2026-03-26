using UnityEngine;

public class ExplosiveBulletObject : MonoBehaviour
{
    public float speed = 5f;
    public float duration = 3f;
    [SerializeField] private int damage = 1000;

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
            var health = collision.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}