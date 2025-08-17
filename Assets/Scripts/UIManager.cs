using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _profitUGUI;
    [SerializeField] private Button _makeProfitBtn;

    private void Start()
    {
        _makeProfitBtn.onClick.AddListener(GameManager.Instance.CreateClickResource);
    }

    private void Update()
    {
        UpdateResourceText();
    }

    private void UpdateResourceText()
    {
        _profitUGUI.text = ResourceManager.Instance.Profit.ToString("F0");
    }
}
