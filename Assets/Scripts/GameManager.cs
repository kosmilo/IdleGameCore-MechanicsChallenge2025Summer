using System;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(CreateClickResource);
    }

    private void CreateClickResource()
    {
        ResourceData.IncreasePrimaryResource(1);
    }
}
