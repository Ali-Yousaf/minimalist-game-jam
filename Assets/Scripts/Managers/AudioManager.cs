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

        LoadVolumes();          //Load saved volumes
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

        float targetVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        float timer = 0f;

        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, targetVolume, timer / fadeInDuration);
            yield return null;
        }

        musicSource.volume = targetVolume;
    }

    // =========================
    // PLAY SFX
    // =========================

    public void PlaySFX(AudioClip clip)
    {
        SFXsource.PlayOneShot(clip);
    }

    // =========================
    // VOLUME CONTROL
    // =========================

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        SFXsource.volume = volume;
        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFXVolumeKey, 1f);
    }

    private void LoadVolumes()
    {
        float musicVol = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        float sfxVol = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);

        musicSource.volume = musicVol;
        SFXsource.volume = sfxVol;
    }
}