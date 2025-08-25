using TMPro;
using UnityEngine;

public class RevolutionCounterUI : MonoBehaviour
{
    [SerializeField] private RevolutionCounterDataSO _revolutionCounterData;

    [SerializeField] private TextMeshProUGUI _countUGUI;
    [SerializeField] private TextMeshProUGUI _counterRateUGUI;

    private void Update()
    {
        UpdateTextAndButtons();
    }

    private void UpdateTextAndButtons()
    {
        RevolutionCounter counter = ResourceManager.Instance.GetRevolutionCounter(_revolutionCounterData.ID);
        _countUGUI.text = _revolutionCounterData.CounterName + ": " + Utils.FormatNum(counter.Count);
        _counterRateUGUI.text = $"Counter rate: {Utils.FormatNum(counter.GetReturnRate())} ({Utils.FormatNum(counter.GetBaseReturnRate())})";
    }
}
