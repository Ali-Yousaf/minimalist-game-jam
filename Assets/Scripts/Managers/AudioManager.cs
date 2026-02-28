using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    public AudioSource musicSource;
    public AudioSource SFXsource;

    [Header("Audio Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip mainMenuMusic2;
    public AudioClip laserShootSFX;
    public AudioClip explosionSFX;


    [Header("Music Settings")]
    [SerializeField] private float fadeInDuration = 2f;
    [SerializeField] private float targetVolume = 1f;

    private float defaultPitch = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        defaultPitch = musicSource.pitch;
        StartCoroutine(FadeInMusic());
    }

    private IEnumerator FadeInMusic()
    {
        musicSource.clip = mainMenuMusic;
        musicSource.volume = 0f;
        musicSource.Play();

        float timer = 0f;

        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, targetVolume, timer / fadeInDuration);
            yield return null;
        }

        musicSource.volume = targetVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXsource.PlayOneShot(clip);
    }
}
