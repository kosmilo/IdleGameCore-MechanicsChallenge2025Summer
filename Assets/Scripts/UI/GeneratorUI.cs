using BreakInfinity;
using TMPro;
using UnityEngine;

public class GeneratorUI : MonoBehaviour
{
    [SerializeField] private GeneratorDataSO _generatorData;
    [SerializeField] private GeneratorDataSO _previousGeneratorData; // For checking visibility
    [SerializeField] private TextMeshProUGUI _countUGUI;
    [SerializeField] private TextMeshProUGUI _PPSUGUI;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private BuyButton[] _hireButtons;
    [SerializeField] private string _productionTitle = "PPS";
    private bool _isHidden;

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
        if (_previousGeneratorData != null)
        {
            HideUI();
        }
        ResourceManager.Instance.OnGameSoftReset += () => { if (_previousGeneratorData != null) HideUI(); };
    }

    private void Update()
    {
        if (_isHidden)
        {
            Generator generator = ResourceManager.Instance.GetGenerator(_generatorData.ID);
            Generator previousGenerator = ResourceManager.Instance.GetGenerator(_previousGeneratorData.ID);
            // Check if there is enough to buy the previous generator
            if (
                ((generator.Type == GeneratorType.Profit || generator.Type == GeneratorType.Generator) && previousGenerator.GetCost() <= ResourceManager.Instance.Profit) ||
                ((generator.Type == GeneratorType.Booster || generator.Type == GeneratorType.RevolutionCounter) && previousGenerator.GetCost() <= ResourceManager.Instance.GetRats()))
            {
                ShowUI();
            }
        }
        else
        {
            UpdateTextAndButtons();
        }
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

    private void ShowUI()
    {
        _isHidden = false;
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
    }

    private void HideUI()
    {
        _isHidden = true;
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
    }
}
