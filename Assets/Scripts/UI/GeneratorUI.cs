using System;
using BreakInfinity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorUI : MonoBehaviour
{
    [SerializeField] private GeneratorDataSO _generatorData;

    [SerializeField] private TextMeshProUGUI _CountUGUI;
    [SerializeField] private TextMeshProUGUI _PPSUGUI;
    [SerializeField] private BuyButton[] hireButtons;

    private void Start()
    {
        foreach (BuyButton wrapper in hireButtons)
        {
            wrapper.Btn.onClick.AddListener(() =>
            {
                ResourceManager.Instance.BuyGenerator(_generatorData.ID, wrapper.Amount);
            });
        }
    }

    private void Update()
    {
        UpdateTextAndButtons();
    }

    private void UpdateTextAndButtons()
    {
        Generator generator = ResourceManager.Instance.GetGenerator(_generatorData.ID);
        _CountUGUI.text = _generatorData.GeneratorName + ": " + Utils.FormatNum(generator.GetCount());
        _PPSUGUI.text = $"PPS: {Utils.FormatNum(generator.GenerationRate, true)} ({Utils.FormatNum(generator.GetSingleUnitGeneration(), true)})";

        foreach (BuyButton buyButton in hireButtons)
        {
            BigDouble cost = generator.GetCost(buyButton.Amount);
            buyButton.UpdateButton(cost);
        }
    }
}
