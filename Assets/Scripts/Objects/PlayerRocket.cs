// using System.Data;
// using UnityEngine;

// public class PlayerRocket : MonoBehaviour
// {
//     [SerializeField] private float speed = 5f;
//     [SerializeField] private float duration = 5f;
//     [SerializeField] private float damage = 1000f;

//     void Start()
//     {
//         Destroy(gameObject, duration);
//     }

//     void Update()
//     {
        
//     }

//     void OnTriggerEnter2D(Collider2D collision)
//     {
//         if(collision.gameObject.CompareTag("Enemy"))
//         {
//             var health = collision.GetComponent<EnemyHealth>();
//             if(health != null)
//             {
//                 health.TakeDamage(damage);
//             }

//             Destroy(gameObject);
//         }
//     }
// }
