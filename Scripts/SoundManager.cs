using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip explosionSound;
    public AudioClip hitSound;
    public AudioClip projectileSound;
    private AudioSource audioSource; // For sounds without specific positions

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayExplosionSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(explosionSound, position);
    }

    public void PlayHitSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(hitSound, position);
    }

    public void PlayProjectileSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(projectileSound, position);
    }
}