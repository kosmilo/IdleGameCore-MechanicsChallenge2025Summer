using System.Linq;
using BreakInfinity;

public class Generator
{
    private int _baseCost;
    private BigDouble _singleUnitRawGeneration;

    private float _costIncreaseExponential;
    private float _costIncreaseLinear;

    private Multiplier[] _availableMultipliers;
    private int _multiplierIndex; // Keep index of the next multiplier so we don't check each multiplier for unlock

    public BigDouble Count { get; private set; }
    public BigDouble GenerationRate { get; private set; } // Per second
    public BigDouble GenerationMultiplier { get; private set; } = 1;

    public Generator(GeneratorDataSO data)
    {
        _baseCost = data.BaseCost;
        _singleUnitRawGeneration = data.SingleUnitGeneration;
        _costIncreaseExponential = data.CostIncreaseExponential;
        _costIncreaseLinear = data.CostIncreaseLinear;
        _availableMultipliers = data.Multipliers;
    }

    public BigDouble GetSingleUnitGeneration() => _singleUnitRawGeneration * GenerationMultiplier;

    /// <summary>
    /// Calculate cost of single purchase.
    /// </summary>
    public BigDouble GetCost()
    {
        return _baseCost * BigDouble.Pow(_costIncreaseExponential, Count) + _costIncreaseLinear * Count;
    }

    /// <summary>
    /// Calculate cost of n purchase(s).
    /// </summary>
    public BigDouble GetCost(int amount)
    {
        // See https://blog.kongregate.com/the-math-of-idle-games-part-i/ for formula
        BigDouble exponentialIncrease = _baseCost * (BigDouble.Pow(_costIncreaseExponential, Count)
            * (BigDouble.Pow(_costIncreaseExponential, amount) - 1)) / (_costIncreaseExponential - 1);
        BigDouble linearIncrease = _costIncreaseLinear * (amount * Count + (amount * (amount - 1) / 2));
        return (linearIncrease + exponentialIncrease).Round();
    }

    public void Add(int amount)
    {
        Count += amount;
        AddMultipliers();
        CalculateGenerationRate();
    }

    public void Remove(int amount)
    {
        if (amount > Count)
        {
            // We should probably throw an error here...
            Count = 0;
            CalculateGenerationRate();
            return;
        }
        Count -= amount;
        CalculateGenerationRate();
    }

    private void CalculateGenerationRate()
    {
        GenerationRate = _singleUnitRawGeneration * Count * GenerationMultiplier;
    }

    private void AddMultipliers()
    {
        if (_availableMultipliers == null || _multiplierIndex >= _availableMultipliers.Count()) return;

        if (Count >= _availableMultipliers[_multiplierIndex].RequiredCount)
        {
            GenerationMultiplier *= _availableMultipliers[_multiplierIndex].MultiplierAmount;
            _multiplierIndex++;
        }
    }
}
