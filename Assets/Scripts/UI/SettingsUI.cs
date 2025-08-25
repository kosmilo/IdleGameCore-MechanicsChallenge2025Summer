using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Button _musicToggle;
    [SerializeField] private Button _sfxToggle;
    [SerializeField] private Button _restartButton;
    [SerializeField] private TextMeshProUGUI _musicToggleUGUI;
    [SerializeField] private TextMeshProUGUI _sfxToggleUGUI;

    private bool _musicOn = true;
    private bool _sfxOn = true;

    private void Start()
    {
        _sfxToggle.onClick.AddListener(() =>
        {
            _sfxOn = !_sfxOn;
            SoundManager.Instance.ToggleSFX(_sfxOn);
            _sfxToggleUGUI.text = _sfxOn ? "On" : "Off";
        });
        _musicToggle.onClick.AddListener(() =>
        {
            _musicOn = !_musicOn;
            SoundManager.Instance.ToggleMusic(_musicOn);
            _musicToggleUGUI.text = _musicOn ? "On" : "Off";
        });
        _restartButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonSound();
            ResourceManager.Instance.GiveUpRun();
        });
    }
}
