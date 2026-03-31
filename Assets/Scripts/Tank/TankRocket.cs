using UnityEngine;

public class TankRocket : MonoBehaviour
{
    [SerializeField] private int damage = 50;
    [SerializeField] private float duration = 1f;
    [SerializeField] public float speed = 5f;
    [SerializeField] public float acceleration = 50f;


    void Start()
    {
        Destroy(gameObject, duration);
    }

    void Update()
    {
        speed += acceleration * Time.deltaTime; // increase speed over time
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            var health = collision.GetComponent<PlayerHealth>();
            
            if(health != null)
            {
                health.TakeDamage(damage);
                print("Damage: " + damage);
                Destroy(gameObject);
            }
        }
    }
}
