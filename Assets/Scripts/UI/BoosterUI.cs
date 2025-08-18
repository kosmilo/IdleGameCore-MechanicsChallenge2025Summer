using TMPro;
using UnityEngine;

public class BoosterUI : MonoBehaviour
{
    [SerializeField] private BoosterDataSO _boosterData;

    [SerializeField] private TextMeshProUGUI _countUGUI;
    [SerializeField] private TextMeshProUGUI _boostUGUI;

    private void Update()
    {
        UpdateTextAndButtons();
    }

    private void UpdateTextAndButtons()
    {
        GenerationBooster booster = ResourceManager.Instance.GetBooster(_boosterData.ID);
        _countUGUI.text = _boosterData.BoosterName + ": " + Utils.FormatNum(booster.Count);
        _boostUGUI.text = $"Boost: {Utils.FormatNum(booster.GetBoost(), true)} ({Utils.FormatNum(booster.GetBaseBoost(), true)})";
    }
}
