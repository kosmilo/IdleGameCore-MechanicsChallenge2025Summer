using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _buttonSound;
    [SerializeField] private AudioClip _tabSound;

    [Header("Component references")]
    [SerializeField] private AudioSource _sfxSource;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlayButtonSound()
    {
        if (_sfxSource.isPlaying) _sfxSource.Stop();
        _sfxSource.PlayOneShot(_buttonSound);
    }

    public void PlayTabSound()
    {
        if (_sfxSource.isPlaying) _sfxSource.Stop();
        _sfxSource.PlayOneShot(_tabSound);
    }
}
