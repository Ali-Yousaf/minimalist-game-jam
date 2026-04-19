using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource SFXsource;

    [Header("Audio Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip bossMusic;
    public AudioClip mazeMusic;
    public AudioClip laserShootSFX;
    public AudioClip explosionSFX;
    public AudioClip upgradeUnlockSFX;
    public AudioClip rocketFire;
    public AudioClip tankExplosionSound;
    public AudioClip flashLightOnSFX;

    [Header("Music Settings")]
    [SerializeField] private float fadeDuration = 2f;

    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    private Coroutine musicRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadVolumes();
    }

    private void Start()
    {
        PlayMainMenuMusic(); // ✅ Will now properly fade in
    }

    // =========================
    // MUSIC CONTROL (CLEAN)
    // =========================

    public void PlayMainMenuMusic()
    {
        PlayMusic(mainMenuMusic);
    }

    public void PlayBossMusic()
    {
        PlayMusic(bossMusic);
    }

    public void PlayMazeMusic()
    {
        PlayMusic(mazeMusic);
    }

    private void PlayMusic(AudioClip newClip)
    {
        if (musicRoutine != null)
            StopCoroutine(musicRoutine);

        musicRoutine = StartCoroutine(FadeToNewMusic(newClip));
    }

    private IEnumerator FadeToNewMusic(AudioClip newClip)
    {
        float timer = 0f;
        float startVolume = musicSource.volume;

        // FADE OUT
        if (musicSource.isPlaying)
        {
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
                yield return null;
            }
        }

        // SWITCH TRACK
        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        // FADE IN
        timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            musicSource.volume = Mathf.Lerp(0f, musicVolume, t * t);
            yield return null;
        }

        musicSource.volume = musicVolume;
        musicRoutine = null;
    }

    // =========================
    // PLAY SFX
    // =========================

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        SFXsource.PlayOneShot(clip, sfxVolume);
    }

    // =========================
    // VOLUME CONTROL
    // =========================

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
        PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        SFXsource.volume = sfxVolume;
        PlayerPrefs.SetFloat(SFXVolumeKey, sfxVolume);
        PlayerPrefs.Save();
    }

    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;

    private void LoadVolumes()
    {
        musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);

        if (musicSource != null)
            musicSource.volume = musicVolume;

        if (SFXsource != null)
            SFXsource.volume = sfxVolume;
    }
}