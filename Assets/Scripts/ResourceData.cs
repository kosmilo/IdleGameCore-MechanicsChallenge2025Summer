using System;
using BreakInfinity;

public static class ResourceData
{
    public static BigDouble PrimaryResource { get; private set; }

    public static Action<BigDouble> OnPrimaryResourceAmountChanged;

    public static void IncreasePrimaryResource(BigDouble amount)
    {
        PrimaryResource += amount;
        OnPrimaryResourceAmountChanged?.Invoke(PrimaryResource);
    }
}
