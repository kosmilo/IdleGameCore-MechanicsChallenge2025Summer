using BreakInfinity;

public class Generator
{
    public int BaseCost { get; private set; }
    public BigDouble SingleUnitGeneration { get; private set; }

    public float CostIncreaseExponential { get; private set; }
    public float CostIncreaseLinear { get; private set; }

    public BigDouble Count { get; private set; }
    public BigDouble GenerationRate { get; private set; } // Per second

    public Generator(GeneratorDataSO data)
    {
        BaseCost = data.BaseCost;
        SingleUnitGeneration = data.SingleUnitGeneration;
        CostIncreaseExponential = data.CostIncreaseExponential;
        CostIncreaseLinear = data.CostIncreaseLinear;
    }

    /// <summary>
    /// Calculate cost of single purchase.
    /// </summary>
    public BigDouble GetCost()
    {
        return BaseCost * BigDouble.Pow(CostIncreaseExponential, Count) + CostIncreaseLinear * Count;
    }

    /// <summary>
    /// Calculate cost of n purchase(s).
    /// </summary>
    public BigDouble GetCost(int amount)
    {
        // See https://blog.kongregate.com/the-math-of-idle-games-part-i/ for formula
        BigDouble exponentialIncrease = BaseCost * (BigDouble.Pow(CostIncreaseExponential, Count)
            * (BigDouble.Pow(CostIncreaseExponential, amount) - 1)) / (CostIncreaseExponential - 1);
        BigDouble linearIncrease = CostIncreaseLinear * (amount * Count + (amount * (amount - 1) / 2));
        return (linearIncrease + exponentialIncrease).Round();
    }

    public void Add(int amount)
    {
        Count += amount;
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
        GenerationRate = SingleUnitGeneration * Count;
    }
}
