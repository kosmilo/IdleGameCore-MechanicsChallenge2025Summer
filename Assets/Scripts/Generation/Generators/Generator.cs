using System.Linq;
using BreakInfinity;

public enum GeneratorType
{
    Profit,
    Booster,
    Generator,
    RevolutionCounter
}

public class Generator
{
    public GeneratorDataSO Data { get; private set; }
    public GeneratorType Type { get; private set; }
    public string TargetID { get; private set; }

    public BigDouble GenerationRate { get; private set; } // Per second
    public BigDouble GenerationMultiplier { get; private set; } = 1;
    public int MultiplierIndex { get; private set; } // Keep index of the next multiplier so we don't check each multiplier for unlock

    private BigDouble _count;

    public Generator(GeneratorDataSO data, GeneratorType type = GeneratorType.Profit, string target = "")
    {
        Data = data;
        Type = type;
        TargetID = target;
    }

    public void SetValues(BigDouble count, int multiplierIndex)
    {
        _count = count;
        for (int i = 0; i < multiplierIndex; i++) AddMultipliers();
        CalculateGenerationRate();
    }

    public BigDouble GetCount() => _count.Round();

    public BigDouble GetSingleUnitGeneration() => (Data.SingleUnitGeneration + ResourceManager.Instance.PrestiegeGenerationBoost) * GenerationMultiplier;

    /// <summary>
    /// Calculate cost of single purchase.
    /// </summary>
    public BigDouble GetCost()
    {
        return Data.BaseCost * BigDouble.Pow(Data.CostIncreaseExponential, _count) + Data.CostIncreaseLinear * _count;
    }

    /// <summary>
    /// Calculate cost of n purchase(s).
    /// </summary>
    public BigDouble GetCost(int amount)
    {
        // See https://blog.kongregate.com/the-math-of-idle-games-part-i/ for formula
        BigDouble exponentialIncrease = Data.BaseCost * (BigDouble.Pow(Data.CostIncreaseExponential, _count)
            * (BigDouble.Pow(Data.CostIncreaseExponential, amount) - 1)) / (Data.CostIncreaseExponential - 1);
        BigDouble linearIncrease = Data.CostIncreaseLinear * (amount * _count + (amount * (amount - 1) / 2));
        return (linearIncrease + exponentialIncrease).Round();
    }

    public void Add(BigDouble amount)
    {
        _count += amount;
        AddMultipliers();
        CalculateGenerationRate();
    }

    public void Remove(BigDouble amount)
    {
        if (amount > _count)
        {
            _count = 0;
            CalculateGenerationRate();
            return;
        }
        _count -= amount;
        CalculateGenerationRate();
    }

    private void CalculateGenerationRate()
    {
        GenerationRate = (Data.SingleUnitGeneration + ResourceManager.Instance.PrestiegeGenerationBoost) * _count.Round() * GenerationMultiplier;
    }

    private void AddMultipliers()
    {
        if (Data.Multipliers == null || MultiplierIndex >= Data.Multipliers.Count()) return;

        if (_count >= Data.Multipliers[MultiplierIndex].RequiredCount)
        {
            GenerationMultiplier *= Data.Multipliers[MultiplierIndex].MultiplierAmount;
            MultiplierIndex++;
        }
    }
}
