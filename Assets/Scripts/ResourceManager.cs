using System;
using System.Collections.Generic;
using BreakInfinity;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Generators")]
    [SerializeField] private GeneratorDataSO _profitGeneratorData;
    [SerializeField] private List<BoosterGeneratorDataSO> _boosterGeneratorDatas;
    [SerializeField] private List<GeneratorGeneratorDataSO> _generatorGeneratorDatas;

    [Header("Boosters")]
    [SerializeField] private List<BoosterDataSO> _boosterDatas;

    public static ResourceManager Instance { get; private set; }
    public BigDouble Profit { get; private set; }
    public BigDouble BoosterMultiplier { get; private set; }
    public BigDouble PPS { get; private set; } // Profit per second
    public BigDouble RPS { get; private set; } // Rats per second
    public BigDouble UnboostedRPS { get; private set; }

    private Dictionary<string, Generator> _generators = new(); // ID lookup
    private Dictionary<string, GenerationBooster> _boosters = new();

    public Generator GetGenerator(string ID)
    {
        if (_generators.TryGetValue(ID, out Generator generator))
        {
            return generator;
        }
        Debug.LogError("Invalid generator ID");
        return null;
    }
    public GenerationBooster GetBooster(string ID)
    {
        if (_boosters.TryGetValue(ID, out GenerationBooster booster))
        {
            return booster;
        }
        Debug.LogError("Invalid booster ID");
        return null;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Create boosters
        foreach (BoosterDataSO boosterData in _boosterDatas)
        {
            GenerationBooster newBooster = new GenerationBooster(boosterData);
            _boosters.Add(boosterData.ID, newBooster);
        }

        // Create generators
        Generator profitGen = new(_profitGeneratorData);
        _generators.Add(_profitGeneratorData.ID, profitGen);

        foreach (BoosterGeneratorDataSO generatorData in _boosterGeneratorDatas)
        {
            Generator newGen = new(generatorData, GeneratorType.Booster, generatorData.GenerationTarget.ID);
            _generators.Add(generatorData.ID, newGen);
        }
        foreach (GeneratorGeneratorDataSO generatorData in _generatorGeneratorDatas)
        {
            Generator newGen = new(generatorData, GeneratorType.Generator, generatorData.GenerationTarget.ID);
            _generators.Add(generatorData.ID, newGen);
        }
    }

    public void Update()
    {
        // Calculate profit boost
        BoosterMultiplier = 0;
        foreach (GenerationBooster booster in _boosters.Values)
        {
            BoosterMultiplier += booster.GetBoost();
        }

        // Handle generation
        PPS = 0;
        RPS = 0;
        UnboostedRPS = 0;
        foreach (Generator generator in _generators.Values)
        {
            switch (generator.Type)
            {
                case GeneratorType.Profit:
                    PPS += generator.GenerationRate * (1 + BoosterMultiplier);
                    Profit += generator.GenerationRate * (1 + BoosterMultiplier) * Time.deltaTime;
                    break;
                case GeneratorType.Booster:
                    GenerationBooster targetBooster = _boosters[generator.TargetID];
                    targetBooster.Add(generator.GenerationRate * Time.deltaTime);
                    break;
                case GeneratorType.Generator:
                    UnboostedRPS += generator.GenerationRate;
                    RPS += generator.GenerationRate * (1 + BoosterMultiplier);
                    Generator targetGenerator = _generators[generator.TargetID];
                    targetGenerator.Add(generator.GenerationRate * (1 + BoosterMultiplier) * Time.deltaTime);
                    break;
            }
        }

        // Consume boosters
        BigDouble profitGeneratorCount = _generators[_profitGeneratorData.ID].GetCount();
        foreach (GenerationBooster booster in _boosters.Values)
        {
            booster.Remove(booster.GetConsumptioRate() * booster.Count * profitGeneratorCount * Time.deltaTime);
        }
    }

    public void BuyGenerator(string ID, int amount)
    {
        if (_generators.TryGetValue(ID, out Generator generator))
        {
            BigDouble cost = generator.GetCost(amount);

            if (generator.Type == GeneratorType.Profit || generator.Type == GeneratorType.Generator) // Profit gens and rat gens cost profit
            {
                if (Profit >= cost)
                {
                    Profit -= cost;
                    generator.Add(amount);
                }
                else Debug.LogError("Not enough profit to buy");
            }
            else if (generator.Type == GeneratorType.Booster) // Booster gens cost rats
            {
                Generator ratGen = _generators[_profitGeneratorData.ID];
                if (ratGen.GetCount() >= cost)
                {
                    ratGen.Remove(cost);
                    generator.Add(amount);
                }
                else Debug.LogError("Not enough profit to buy");
            }
        }
        else Debug.LogError("Invalid generator ID");
    }

    public bool CanBuy(string ID, int amount)
    {
        if (_generators.TryGetValue(ID, out Generator generator))
        {
            BigDouble cost = generator.GetCost(amount);

            if (generator.Type == GeneratorType.Profit || generator.Type == GeneratorType.Generator) // Profit gens and rat gens cost profit
            {
                return Profit >= cost;
            }
            else if (generator.Type == GeneratorType.Booster) // Booster gens cost rats
            {
                Generator ratGen = _generators[_profitGeneratorData.ID];
                return ratGen.GetCount() >= cost;
            }
        }

        Debug.LogError("Invalid generator ID");
        return false;
    }

    internal void CreateClickResource()
    {
        Profit++;
    }
}
