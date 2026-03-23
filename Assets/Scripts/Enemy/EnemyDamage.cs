using UnityEngine;

public class EnemyDamage : MonoBehaviour
{ 
    [SerializeField] private int damage = 10;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }

        if(collision.CompareTag("Shield"))
        {
            GetComponent<EnemyHealth>().Die();
        }
    }
}
