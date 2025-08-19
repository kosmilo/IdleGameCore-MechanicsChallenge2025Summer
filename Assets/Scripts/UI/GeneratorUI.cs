using BreakInfinity;
using TMPro;
using UnityEngine;

public class GeneratorUI : MonoBehaviour
{
    [SerializeField] private GeneratorDataSO _generatorData;
    [SerializeField] private TextMeshProUGUI _countUGUI;
    [SerializeField] private TextMeshProUGUI _PPSUGUI;
    [SerializeField] private BuyButton[] _hireButtons;
    [SerializeField] private string _productionTitle = "PPS";

    private void Start()
    {
        foreach (BuyButton wrapper in _hireButtons)
        {
            wrapper.Btn.onClick.AddListener(() =>
            {
                ResourceManager.Instance.BuyGenerator(_generatorData.ID, wrapper.Amount);
                SoundManager.Instance.PlayButtonSound();
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
        _countUGUI.text = _generatorData.GeneratorName + ": " + Utils.FormatNum(generator.GetCount());
        _PPSUGUI.text = $"{_productionTitle}: {Utils.FormatNum(generator.GenerationRate, true)} ({Utils.FormatNum(generator.GetSingleUnitGeneration(), true)})";

        foreach (BuyButton buyButton in _hireButtons)
        {
            BigDouble cost = generator.GetCost(buyButton.Amount);
            buyButton.UpdateButton(cost, _generatorData.ID);
        }
    }
}
