using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosiveBulletObject : MonoBehaviour
{
    public float speed = 5f;
    public float duration = 3f;
    [SerializeField] private int damage = 100;
    [SerializeField] private ParticleSystem explodeParticle;
    [SerializeField] private ParticleSystem explodeParticle_x4;
    [SerializeField] private ParticleSystem explodeParticle_y4;
    [SerializeField] private ParticleSystem explodeParticle_xM4;
    [SerializeField] private ParticleSystem explodeParticle_yM4;

    [SerializeField] private float directionalExplosionDelay = 0.15f;

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
                health.TakeDamage(damage);
            

            GetComponent<SpriteRenderer>().enabled = false;
            
            explodeParticle.Play();
            StartCoroutine(DirectionalParticles());

            Destroy(gameObject, 3f);
        }

        if(collision.CompareTag("Tank"))
        {
            collision.GetComponent<TankHealth>()?.TakeDamage(damage);

            GetComponent<SpriteRenderer>().enabled = false;
            
            explodeParticle.Play();
            StartCoroutine(DirectionalParticles());

            Destroy(gameObject, 3f);
        }
    }

    private IEnumerator DirectionalParticles()
    {
        explodeParticle_x4.Play();
        yield return new WaitForSeconds(directionalExplosionDelay);

        explodeParticle_y4.Play();
        yield return new WaitForSeconds(directionalExplosionDelay);

        explodeParticle_xM4.Play();
        yield return new WaitForSeconds(directionalExplosionDelay);

        explodeParticle_yM4.Play();
        yield return new WaitForSeconds(directionalExplosionDelay);
    }
}