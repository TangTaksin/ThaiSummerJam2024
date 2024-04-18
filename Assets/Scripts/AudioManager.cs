using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource uiSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] bgmClips;
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioClip startCombat;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Audio Clips Attack")]
    [SerializeField] private AudioClip sfx_NormalAttack;
    [SerializeField] private AudioClip sfx_FireAttack;
    [SerializeField] private AudioClip sfx_IceAttack;
    [SerializeField] private AudioClip sfx_WaterAttack;

    [Header("Audio Clips Combine")]
    [SerializeField] private AudioClip sfx_Combine_FireWater;
    [SerializeField] private AudioClip sfx_Combine_IceWater;
    [SerializeField] private AudioClip sfx_Combine_FireIce;

    [Header("Audio Clips UI")]
    [SerializeField] private AudioClip SFX_UI_Select;
    [SerializeField] private AudioClip SFX_UI_Confirm;
    [SerializeField] private AudioClip SFX_UI_Cancel;
    [SerializeField] private AudioClip SFX_UI_Error;



    [Header("Footstep Settings")]
    [SerializeField] private float minTimeBetweenFootsteps = 0.3f;
    [SerializeField] private float maxTimeBetweenFootsteps = 0.6f;

    private float lastFootstepTime;
    private int nextFootstepIndex = 0;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        PlayMusic(bgmClips[0]); // Play the first BGM clip by default
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1.0f)
    {
        StartCoroutine(FadeMusic(clip, fadeDuration));
    }

    private IEnumerator FadeMusic(AudioClip clip, float fadeDuration)
    {
        float startVolume = musicSource.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();

        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, startVolume, timer / fadeDuration);
            yield return null;
        }

        musicSource.volume = startVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayUISFX(AudioClip clip)
    {
        uiSource.PlayOneShot(clip);
    }

    public void PlayFootstepSFX()
    {
        if (Time.time - lastFootstepTime >= Random.Range(minTimeBetweenFootsteps, maxTimeBetweenFootsteps))
        {
            AudioClip footstepSound = footstepSounds[nextFootstepIndex];
            sfxSource.PlayOneShot(footstepSound);
            nextFootstepIndex = (nextFootstepIndex + 1) % footstepSounds.Length;
            lastFootstepTime = Time.time;
        }
    }
}
