using UnityEngine;

public class DestroyEnemiesOnMenu : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }
}
