using UnityEngine;

public class BomberExplode : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private ParticleSystem explosionParticle; 

    private bool hasExploded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasExploded) return;

        if (collision.CompareTag("ExplodeRadius"))
        {
            Explode();
            PlayerController.Instance.killCounter -= 5;
        }

        if (collision.CompareTag("Shield"))
        {
            Explode();

            GetComponent<EnemyHealth>().TakeDamage(1000);
        }
    }

    private void Explode()
    {
        hasExploded = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        explosionParticle.Play();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.explosionSFX);

        Destroy(gameObject, 1.5f);
    }
}