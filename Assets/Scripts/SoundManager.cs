using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _buttonSound;
    [SerializeField] private AudioClip _tabSound;

    [Header("Component references")]
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource;

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

    internal void ToggleSFX(bool on)
    {
        _sfxSource.volume = on ? 1 : 0;
    }

    internal void ToggleMusic(bool on)
    {
        _musicSource.volume = on ? 0.05f : 0;
    }
}
