using System;
using BreakInfinity;
using UnityEngine;

[CreateAssetMenu(fileName = "RevolutionCounterDataSO", menuName = "Scriptable Objects/RevolutionCounterDataSO")]
public class RevolutionCounterDataSO : ScriptableObject
{
    [field: SerializeField] public string ID { get; set; } = Guid.NewGuid().ToString();
    [field: SerializeField] public string CounterName { get; set; }
    [field: SerializeField] public BigDouble ReturnRate { get; private set; } = 25; // Return rats per second

    private void OnValidate()
    {
        if (ID == "") ID = Guid.NewGuid().ToString();
    }
}
