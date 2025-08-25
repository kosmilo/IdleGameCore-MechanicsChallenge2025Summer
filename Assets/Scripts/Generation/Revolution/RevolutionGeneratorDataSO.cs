using UnityEngine;

[CreateAssetMenu(fileName = "RevolutionGeneratorDataSO", menuName = "Scriptable Objects/RevolutionGeneratorDataSO")]
public class RevolutionGeneratorDataSO : GeneratorDataSO
{
    [field: SerializeField] public RevolutionCounterDataSO GenerationTarget { get; private set; }
}
