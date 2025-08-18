using System;
using BreakInfinity;
using UnityEngine;

[CreateAssetMenu(fileName = "BoosterDataSO", menuName = "Scriptable Objects/BoosterDataSO")]
public class BoosterDataSO : ScriptableObject
{
    [field: SerializeField] public string ID { get; set; } = Guid.NewGuid().ToString();
    [field: SerializeField] public string BoosterName { get; set; }
    [field: SerializeField] public BigDouble BaseBoost { get; private set; } = 0.01; // How much generation is boosted per unit
    [field: SerializeField] public BigDouble BaseConsumptionRate { get; private set; } = 0.01;
}
