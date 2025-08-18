using System;
using BreakInfinity;
using UnityEngine;

[Serializable]
public struct Multiplier
{
    public int RequiredCount;
    public float MultiplierAmount;
}

[CreateAssetMenu(fileName = "GeneratorDataSO", menuName = "Scriptable Objects/GeneratorDataSO")]
public class GeneratorDataSO : ScriptableObject
{
    [field: SerializeField] public string ID { get; set; } = Guid.NewGuid().ToString();
    [field: SerializeField] public string GeneratorName { get; set; }
    [field: SerializeField] public BigDouble BaseCost { get; private set; } = 8;
    [field: SerializeField] public double SingleUnitGeneration { get; private set; } = 0.2f;

    [field: SerializeField] public float CostIncreaseExponential { get; private set; } = 1.0001f;
    [field: SerializeField] public float CostIncreaseLinear { get; private set; } = 0.5f;

    [field: SerializeField] public Multiplier[] Multipliers { get; private set; }

    private void OnValidate()
    {
        if (ID == "") ID = Guid.NewGuid().ToString();
    }
}
