using System;
using BreakInfinity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ButtonWrapper
{
    public Button Btn;
    public TextMeshProUGUI CostUGUI;
    public int Amount;
}

public class GeneratorUI : MonoBehaviour
{
    [SerializeField] private GeneratorDataSO _generatorData;

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _ratsCountUGUI;
    [SerializeField] private TextMeshProUGUI _ratsPPSUGUI;
    [SerializeField] private ButtonWrapper[] hireButtons;

    private void Start()
    {
        foreach (ButtonWrapper wrapper in hireButtons)
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
        _ratsCountUGUI.text = _generatorData.GeneratorName + ": " + generator.GetCount().ToString("F0");
        _ratsPPSUGUI.text = $"PPS: {generator.GenerationRate.ToString("F2")} ({generator.GetSingleUnitGeneration().ToString("F2")})";

        foreach (ButtonWrapper wrapper in hireButtons)
        {
            BigDouble cost = generator.GetCost(wrapper.Amount);
            string costString = cost < 10000 ? cost.ToString("F0") : cost.ToString("G4");
            wrapper.CostUGUI.text = costString;
            wrapper.Btn.interactable = cost <= ResourceManager.Instance.Profit;
        }
    }
}
