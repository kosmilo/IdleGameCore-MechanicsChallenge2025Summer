using UnityEngine;

[CreateAssetMenu(fileName = "GeneratorGeneratorDataSO", menuName = "Scriptable Objects/GeneratorGeneratorDataSO")]
public class GeneratorGeneratorDataSO : GeneratorDataSO
{
    [field: SerializeField] public GeneratorDataSO GenerationTarget { get; private set; }
}
