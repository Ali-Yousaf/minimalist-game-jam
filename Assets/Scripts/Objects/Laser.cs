using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 5f;
    public float duration = 3f;
    private int damage;

    void Start()
    {
        // Get damage from PlayerController
        damage = PlayerController.Instance.laserDamage;
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

        if(collision.CompareTag("Tank"))
        {
            var health = collision.GetComponent<TankHealth>();
            if(health != null)
            {
                health.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}