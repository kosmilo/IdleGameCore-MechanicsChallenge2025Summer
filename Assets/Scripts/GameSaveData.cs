using System;
using System.Collections.Generic;
using BreakInfinity;
using UnityEngine;

[Serializable]
public struct GeneratorSave
{
    public string ID;
    public BigDouble Count;
    public int MultiplierIndex;
}

[Serializable]
public struct BoosterSave
{
    public string ID;
    public BigDouble Count;
}

[Serializable]
public struct RevolutionCounterSave
{
    public string ID;
    public BigDouble Count;
}

public class GameSaveData
{
    public BigDouble CurrentProfit;
    public BigDouble LifetimeProfit;
    public BigDouble PrestiegeGenerationBoost;
    public BigDouble RatsOnStrike;
    public EventStatus RevolutionStatus;
    public int RevolutionPhase;

    public List<GeneratorSave> Generators = new(); // ID lookup
    public List<BoosterSave> Boosters = new();
    public List<RevolutionCounterSave> RevolutionCounters = new();

    public GameSaveData(
        Dictionary<string, Generator> generators,
        Dictionary<string, Booster> boosters,
        Dictionary<string, RevolutionCounter> revolutionCounters,
        BigDouble profit,
        BigDouble lifetimeProfit,
        BigDouble prestiegeGenerationBoost,
        BigDouble ratsOnStrike,
        EventStatus revolutionStatus,
        int revolutionPhase
    )
    {
        foreach ((string id, Generator gen) in generators)
        {
            Generators.Add(new()
            {
                ID = id,
                Count = gen.GetCount(),
                MultiplierIndex = gen.MultiplierIndex
            });
        }

        foreach ((string id, Booster booster) in boosters)
        {
            Boosters.Add(new()
            {
                ID = id,
                Count = booster.Count
            });
        }

        foreach ((string id, RevolutionCounter revolutionCounter) in revolutionCounters)
        {
            RevolutionCounters.Add(new()
            {
                ID = id,
                Count = revolutionCounter.Count
            });
        }
        CurrentProfit = profit;
        LifetimeProfit = lifetimeProfit;
        PrestiegeGenerationBoost = prestiegeGenerationBoost;
        RatsOnStrike = ratsOnStrike;
        RevolutionStatus = revolutionStatus;
        RevolutionPhase = revolutionPhase;
    }

    public string GetJson()
    {
        return JsonUtility.ToJson(this);
    }
}
