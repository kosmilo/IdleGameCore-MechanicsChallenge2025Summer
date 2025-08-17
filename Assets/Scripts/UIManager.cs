using System;
using BreakInfinity;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resourceUGUI;

    private void Start()
    {
        _resourceUGUI.text = "0";
        ResourceData.OnPrimaryResourceAmountChanged += UpdateResourceText;
    }

    private void UpdateResourceText(BigDouble resourceAmount)
    {
        _resourceUGUI.text = resourceAmount.ToString();
    }
}
