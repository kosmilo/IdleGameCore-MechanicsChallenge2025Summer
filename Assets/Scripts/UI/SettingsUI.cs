using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Button _restartButton;

    private void Start()
    {
        _restartButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonSound();
            ResourceManager.Instance.GiveUpRun();
        });
    }
}
