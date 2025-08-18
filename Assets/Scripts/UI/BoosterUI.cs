using TMPro;
using UnityEngine;

public class BoosterUI : MonoBehaviour
{
    [SerializeField] private BoosterDataSO _boosterData;

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _countUGUI;
    [SerializeField] private TextMeshProUGUI _boostUGUI;

    private void Update()
    {
        UpdateTextAndButtons();
    }

    private void UpdateTextAndButtons()
    {
        GenerationBooster booster = ResourceManager.Instance.GetBooster(_boosterData.ID);
        _countUGUI.text = _boosterData.BoosterName + ": " + booster.Count.ToString("F0");
        _boostUGUI.text = $"Boost: {booster.GetBoost().ToString("F2")} ({booster.GetBaseBoost().ToString("F2")})";
    }
}
