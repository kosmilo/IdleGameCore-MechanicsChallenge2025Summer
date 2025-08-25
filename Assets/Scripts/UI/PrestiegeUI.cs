using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrestiegeUI : MonoBehaviour
{
    [SerializeField] private Button _prestiegeButton;
    [SerializeField] private TextMeshProUGUI _totalPrestiegeBoostUGUI;
    [SerializeField] private TextMeshProUGUI _prestiegeGainUGUI;

    private void Start()
    {
        _prestiegeButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonSound();
            ResourceManager.Instance.PrestiegeRun();
        });
    }

    private void Update()
    {
        _totalPrestiegeBoostUGUI.text = "Total prestiege boost: " + Utils.FormatNum(ResourceManager.Instance.PrestiegeGenerationBoost, true);
        _prestiegeGainUGUI.text = "Gain: " + Utils.FormatNum(ResourceManager.Instance.GetNextPrestiegeBoost(), true);
    }
}
