using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource musicSource; // For background music
    public AudioSource sfxSource;   // For sound effects (explosion, projectile, hit)

    // Audio clips for each sound effect
    public AudioClip explosionClip;
    public AudioClip projectileClip;
    public AudioClip hitClip;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Load saved volumes
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 1f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 1f));

        // Debug to confirm setup
        if (musicSource == null) Debug.LogWarning("MusicSource not assigned in SoundManager!");
        if (sfxSource == null) Debug.LogWarning("SFXSource not assigned in SoundManager!");
        if (explosionClip == null) Debug.LogWarning("ExplosionClip not assigned in SoundManager!");
        if (projectileClip == null) Debug.LogWarning("ProjectileClip not assigned in SoundManager!");
        if (hitClip == null) Debug.LogWarning("HitClip not assigned in SoundManager!");
    }

    public void PlayExplosionSound(Vector3 position)
    {
        if (sfxSource != null && explosionClip != null)
        {
            sfxSource.PlayOneShot(explosionClip);
            Debug.Log($"Playing explosion sound at position: {position}");
        }
        else
        {
            Debug.LogWarning("Cannot play explosion sound: SFXSource or ExplosionClip is null!");
        }
    }

    public void PlayProjectileSound(Vector3 position)
    {
        if (sfxSource != null && projectileClip != null)
        {
            sfxSource.PlayOneShot(projectileClip);
            Debug.Log($"Playing projectile sound at position: {position}");
        }
        else
        {
            Debug.LogWarning("Cannot play projectile sound: SFXSource or ProjectileClip is null!");
        }
    }

    public void PlayHitSound(Vector3 position)
    {
        if (sfxSource != null && hitClip != null)
        {
            sfxSource.PlayOneShot(hitClip);
            Debug.Log($"Playing hit sound at position: {position}");
        }
        else
        {
            Debug.LogWarning("Cannot play hit sound: SFXSource or HitClip is null!");
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
            Debug.Log($"Music volume set to: {volume}");
        }
        else
        {
            Debug.LogWarning("Cannot set music volume: MusicSource is null!");
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = volume;
            Debug.Log($"SFX volume set to: {volume}");
        }
        else
        {
            Debug.LogWarning("Cannot set SFX volume: SFXSource is null!");
        }
    }
}