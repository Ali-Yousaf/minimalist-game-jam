using UnityEngine;

public class EnemyDamage : MonoBehaviour
{ 
    [SerializeField] private int damage = 10;
    public bool isBomber = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerHealth health = collision.GetComponent<PlayerHealth>();
            
            if(isBomber == true)
                health.isHitByBomber = true;

            health.TakeDamage(damage);
            Destroy(gameObject);
        }

        if(collision.CompareTag("Shield"))
        {
            GetComponent<EnemyHealth>().TakeDamage(1000);
        }
    }
}
