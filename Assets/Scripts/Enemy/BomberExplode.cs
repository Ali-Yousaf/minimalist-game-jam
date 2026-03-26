using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BomberExplode : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private ParticleSystem explosionParticle;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip slowTick;
    [SerializeField] private AudioClip fastTick;

    [Header("Audio Settings")]
    [SerializeField] private float fastTickDistanceThreshold = 5f; 
    [SerializeField] private float maxVolume = 0.1f;
    [SerializeField] private Canvas healthBarCanvas;

    private AudioSource slowSource;
    private AudioSource fastSource;

    private Transform player;
    private CircleCollider2D explodeRadius;
    private bool hasExploded = false;

    private void Awake()
    {
        slowSource = gameObject.AddComponent<AudioSource>();
        fastSource = gameObject.AddComponent<AudioSource>();

        SetupSource(slowSource, slowTick);
        SetupSource(fastSource, fastTick);
    }

    private void SetupSource(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.loop = true;
        source.playOnAwake = false;
        source.spatialBlend = 0f;
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        healthBarCanvas = GetComponentInChildren<Canvas>();

        if (playerObj != null)
        {
            player = playerObj.transform;
            explodeRadius = playerObj.GetComponentInChildren<CircleCollider2D>();
        }

        if (slowTick != null) slowSource.Play();
        if (fastTick != null) fastSource.Play();
    }

    private void Update()
    {
        HandleAudio();
    }

    private void HandleAudio()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= fastTickDistanceThreshold)
        {
            fastSource.volume = maxVolume;
            slowSource.volume = 0f;
        }
        else
        {
            slowSource.volume = maxVolume;
            fastSource.volume = 0f;
        }
    }

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

        slowSource.Stop();
        fastSource.Stop();

        GetComponent<SpriteRenderer>().enabled = false;
        healthBarCanvas.enabled = false;
        CameraShake.Instance.Shake(0.2f, 0.2f);
        GridJuiceFX.Instance.Flash();

        if (explosionParticle != null)
            explosionParticle.Play();

        AudioManager.Instance.PlaySFX(AudioManager.Instance.explosionSFX);

        Destroy(gameObject, 1.5f);
    }
}