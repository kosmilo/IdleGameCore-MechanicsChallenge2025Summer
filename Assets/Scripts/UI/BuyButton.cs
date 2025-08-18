using BreakInfinity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    [field: SerializeField] public Button Btn { get; private set; }
    [field: SerializeField] public int Amount { get; private set; }
    [SerializeField] private TextMeshProUGUI _costUGUI;
    [SerializeField] private TextMeshProUGUI _amountUGUI;

    private void Start()
    {
        _amountUGUI.text = Utils.FormatNum(Amount);
    }

    public void UpdateButton(BigDouble cost)
    {
        _costUGUI.text = Utils.FormatNum(cost);
        Btn.interactable = cost <= ResourceManager.Instance.Profit;
    }
}
