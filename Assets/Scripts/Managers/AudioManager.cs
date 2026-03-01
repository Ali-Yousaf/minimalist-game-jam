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
    public AudioClip mainMenuMusic2;
    public AudioClip laserShootSFX;
    public AudioClip explosionSFX;
    public AudioClip upgradeUnlockSFX;

    [Header("Music Settings")]
    [SerializeField] private float fadeInDuration = 2f;

    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private float defaultPitch = 1f;
    private float musicVolume = 1f;
    private float sfxVolume = 1f;

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
        defaultPitch = musicSource.pitch;
        StartCoroutine(FadeInMusic());
    }

    // =========================
    // MUSIC FADE IN
    // =========================
    private IEnumerator FadeInMusic()
    {
        musicSource.clip = mainMenuMusic;
        musicSource.volume = 0f;
        musicSource.Play();

        float timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, musicVolume, timer / fadeInDuration);
            yield return null;
        }

        musicSource.volume = musicVolume;
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