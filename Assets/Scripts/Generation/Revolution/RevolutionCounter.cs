using BreakInfinity;

public class RevolutionCounter
{
    public RevolutionCounterDataSO Data { get; private set; }
    public BigDouble Count { get; private set; } = 0;

    public BigDouble GetReturnRate() => Data.ReturnRate * Count;
    public BigDouble GetBaseReturnRate() => Data.ReturnRate;

    public RevolutionCounter(RevolutionCounterDataSO data)
    {
        Data = data;
    }

    public void SetValues(BigDouble count) => Count = count;

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
