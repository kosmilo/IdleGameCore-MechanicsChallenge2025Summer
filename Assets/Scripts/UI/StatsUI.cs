using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _profitUGUI;
    [SerializeField] private TextMeshProUGUI _boosterMultiplierUGUI;
    [SerializeField] private TextMeshProUGUI _rawRPSUGUI;
    [SerializeField] private TextMeshProUGUI _totalPPSUGUI;
    [SerializeField] private TextMeshProUGUI _totalRPSUGUI;
    [SerializeField] private Button _makeProfitBtn;

    private void Start()
    {
        _makeProfitBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonSound();
            ResourceManager.Instance.CreateClickResource();
        });
    }

    private void Update()
    {
        UpdateResourceText();
    }

    private void UpdateResourceText()
    {
        _profitUGUI.text = Utils.FormatNum(ResourceManager.Instance.Profit, false);
        _rawRPSUGUI.text = "Raw RPS: " + Utils.FormatNum(ResourceManager.Instance.UnboostedRPS, true);
        _boosterMultiplierUGUI.text = "Booster multiplier: " + Utils.FormatNum(ResourceManager.Instance.BoosterMultiplier, true);
        _totalPPSUGUI.text = "PPS: " + Utils.FormatNum(ResourceManager.Instance.PPS, true);
        _totalRPSUGUI.text = "RPS: " + Utils.FormatNum(ResourceManager.Instance.RPS, true);
    }
}
