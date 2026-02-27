using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float damage = 25f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float duration = 3f;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            print("Enemy Hit");
        }
    }
}
