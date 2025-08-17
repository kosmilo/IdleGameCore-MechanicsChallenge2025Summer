using System;
using BreakInfinity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RatsUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _ratsCountUGUI;
    [SerializeField] private TextMeshProUGUI _ratsPPSUGUI;
    [Serializable]
    public struct ButtonWrapper
    {
        public Button Btn;
        public TextMeshProUGUI CostUGUI;
        public int Amount;
    }
    [SerializeField] private ButtonWrapper[] hireButtons;

    private bool isHidden = true;

    private void Start()
    {
        // Hide until required profits are earned
        HidePanel();

        foreach (ButtonWrapper wrapper in hireButtons)
        {
            wrapper.Btn.onClick.AddListener(() =>
            {
                GameManager.Instance.BuyRat(wrapper.Amount);
                UpdateButtonsCost();
            });
        }
        UpdateButtonsCost();
    }

    private void Update()
    {
        if (isHidden)
        {
            if (ResourceManager.Instance.Profit >= 8)
            {
                ShowPanel();
            }
            else return;
        }
        UpdateTextAndButtons();
    }

    private void ShowPanel()
    {
        isHidden = false;
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
    }

    private void HidePanel()
    {
        isHidden = true;
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
    }

    private void UpdateTextAndButtons()
    {
        _ratsCountUGUI.text = "Rats: " + ResourceManager.Instance.Rats.Count.ToString("F0");
        _ratsPPSUGUI.text = $"PPS: {ResourceManager.Instance.Rats.GenerationRate.ToString("F2")} ({ResourceManager.Instance.Rats.SingleUnitGeneration.ToString("F2")})";

        foreach (ButtonWrapper wrapper in hireButtons)
        {
            BigDouble cost = ResourceManager.Instance.Rats.GetCost(wrapper.Amount);
            wrapper.Btn.interactable = cost <= ResourceManager.Instance.Profit;
        }
    }

    private void UpdateButtonsCost()
    {
        foreach (ButtonWrapper wrapper in hireButtons)
        {
            BigDouble cost = ResourceManager.Instance.Rats.GetCost(wrapper.Amount);
            string costString = cost < 10000 ? cost.ToString("F0") : cost.ToString("G4");
            wrapper.CostUGUI.text = costString;
        }
    }
}
