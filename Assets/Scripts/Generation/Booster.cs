using BreakInfinity;

public class Booster
{
    public BoosterDataSO Data { get; private set; }
    public BigDouble Count { get; private set; } = 0;

    public BigDouble GetBoost() => Count * Data.BaseBoost;
    public BigDouble GetBaseBoost() => Data.BaseBoost;
    public BigDouble GetConsumptioRate() => Data.BaseConsumptionRate;

    public Booster(BoosterDataSO data)
    {
        Data = data;
    }

    public void Add(BigDouble amount)
    {
        Count += amount;
    }

    public void Remove(BigDouble amount)
    {
        if (amount > Count)
        {
            Count = 0;
            return;
        }
        Count -= amount;
    }
}
