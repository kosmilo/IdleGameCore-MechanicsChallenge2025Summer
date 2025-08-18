using UnityEngine;

[CreateAssetMenu(fileName = "BoosterGeneratorDataSO", menuName = "Scriptable Objects/BoosterGeneratorDataSO")]
public class BoosterGeneratorDataSO : GeneratorDataSO
{
    [field: SerializeField] public BoosterDataSO GenerationTarget { get; private set; }
}
