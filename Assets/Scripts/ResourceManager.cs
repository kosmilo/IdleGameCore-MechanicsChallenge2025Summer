using System;
using System.Collections.Generic;
using BreakInfinity;
using UnityEngine;

/// <summary>
/// Handle boosters, generators and profit. 
/// </summary>
public class ResourceManager : MonoBehaviour
{
    [Header("Generators")]
    [SerializeField] private GeneratorDataSO _profitGeneratorData;
    [SerializeField] private List<BoosterGeneratorDataSO> _boosterGeneratorDatas;
    [SerializeField] private List<GeneratorGeneratorDataSO> _generatorGeneratorDatas;
    [SerializeField] private List<RevolutionGeneratorDataSO> _revolutionCounterGeneratorDatas;

    [Header("Boosters and Counters")]
    [SerializeField] private List<BoosterDataSO> _boosterDatas;
    [SerializeField] private List<RevolutionCounterDataSO> _revolutionCounterDatas;

    public static ResourceManager Instance { get; private set; }
    public BigDouble Profit { get; private set; }
    public BigDouble BoosterMultiplier { get; private set; }
    public BigDouble PrestiegeGenerationBoost { get; private set; }
    public float QuitRate { get; private set; } = 0;
    private BigDouble _ratsOnStrike;
    private BigDouble _ratReturnRate;

    private Dictionary<string, Generator> _generators = new(); // ID lookup
    private Dictionary<string, Booster> _boosters = new();
    private Dictionary<string, RevolutionCounter> _revolutionCounters = new();

    // For UI
    public BigDouble PPS { get; private set; } // Profit per second
    public BigDouble RPS { get; private set; } // Rats per second
    public BigDouble UnboostedRPS { get; private set; }

    #region Public Methods

    public BigDouble GetRatsOnStrike() => _ratsOnStrike.Round();
    public BigDouble GetNextPrestiegeBoost() => BigDouble.Max(0.1 * (_generators[_profitGeneratorData.ID].GetCount() - 10000) / 100000, 0);

    public Generator GetGenerator(string ID)
    {
        if (_generators.TryGetValue(ID, out Generator generator))
        {
            return generator;
        }
        Debug.LogError("Invalid generator ID");
        return null;
    }

    public Booster GetBooster(string ID)
    {
        if (_boosters.TryGetValue(ID, out Booster booster))
        {
            return booster;
        }
        Debug.LogError("Invalid booster ID");
        return null;
    }

    public RevolutionCounter GetRevolutionCounter(string ID)
    {
        if (_revolutionCounters.TryGetValue(ID, out RevolutionCounter counter))
        {
            return counter;
        }
        Debug.LogError("Invalid counter ID");
        return null;
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
                else Debug.LogError("Not enough profit gens to buy");
            }
            else if (generator.Type == GeneratorType.Booster || generator.Type == GeneratorType.RevolutionCounter) // Booster gens cost rats
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
            else if (generator.Type == GeneratorType.Booster || generator.Type == GeneratorType.RevolutionCounter) // Booster gens cost rats
            {
                Generator ratGen = _generators[_profitGeneratorData.ID];
                return ratGen.GetCount() >= cost;
            }
        }
        Debug.LogError("Invalid generator ID");
        return false;
    }

    public void CreateClickResource()
    {
        Profit += 1 + BoosterMultiplier;
    }

    public void SetQuitRate(float newRate)
    {
        QuitRate = newRate;
    }

    public void PrestiegeRun()
    {
        ScreenFade.Instance.StartScreenFaidOut(1, () =>
        {
            PrestiegeGenerationBoost += GetNextPrestiegeBoost();
            ResetGameResources();
            GameEventManager.Instance.ResetEventData();
            ScreenFade.Instance.StartScreenFaidIn(1);
        });
    }

    #endregion
    #region Initialization & Reset

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Make sure boosters, counters and generators exists before anything else
        CreateBoosters();
        CreateCounters();
        CreateGenerators();
    }

    private void CreateBoosters()
    {
        _boosters.Clear();
        foreach (BoosterDataSO boosterData in _boosterDatas)
        {
            Booster newBooster = new Booster(boosterData);
            _boosters.Add(boosterData.ID, newBooster);
        }
    }

    private void CreateCounters()
    {
        _revolutionCounters.Clear();
        foreach (RevolutionCounterDataSO counterData in _revolutionCounterDatas)
        {
            RevolutionCounter newcounter = new RevolutionCounter(counterData);
            _revolutionCounters.Add(counterData.ID, newcounter);
        }
    }

    private void CreateGenerators()
    {
        _generators.Clear();
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
        foreach (RevolutionGeneratorDataSO generatorData in _revolutionCounterGeneratorDatas)
        {
            Generator newGen = new(generatorData, GeneratorType.RevolutionCounter, generatorData.GenerationTarget.ID);
            _generators.Add(generatorData.ID, newGen);
        }
    }

    public void GiveUpRun()
    {
        ScreenFade.Instance.StartScreenFaidOut(1, () =>
        {
            ResetGameResources();
            GameEventManager.Instance.ResetEventData();
            ScreenFade.Instance.StartScreenFaidIn(1);
        });
    }

    private void ResetGameResources()
    {
        Profit = 0;
        BoosterMultiplier = 0;

        CreateBoosters();
        CreateGenerators();
    }

    #endregion
    #region Game Update

    private void Update()
    {
        CalculateBoosterMultiplier();
        HandleGenerators();
        ConsumeBoosters();
        UpdateRevolution();
    }

    private void UpdateRevolution()
    {
        Generator ratGenerator = _generators[_profitGeneratorData.ID];

        // Rats entering strike
        BigDouble ratsEnteringStrike = ratGenerator.GetCount() * QuitRate * Time.deltaTime;
        _ratsOnStrike += ratsEnteringStrike;
        ratGenerator.Remove(ratsEnteringStrike);

        // Rats returning
        BigDouble ratsReturned = _ratReturnRate * Time.deltaTime < _ratsOnStrike ? _ratReturnRate * Time.deltaTime : _ratsOnStrike;
        _ratsOnStrike -= ratsReturned;
        ratGenerator.Add(ratsReturned);
    }

    private void CalculateBoosterMultiplier()
    {
        BoosterMultiplier = 0;
        foreach (Booster booster in _boosters.Values)
        {
            BoosterMultiplier += booster.GetBoost();
        }
    }

    private void HandleGenerators()
    {
        // Reset so we can update the values
        PPS = 0;
        RPS = 0;
        UnboostedRPS = 0;
        _ratReturnRate = 0;

        foreach (Generator generator in _generators.Values)
        {
            switch (generator.Type)
            {
                case GeneratorType.Profit:
                    BigDouble profitGeneration = generator.GenerationRate * (1 + BoosterMultiplier);
                    PPS += profitGeneration;
                    Profit += profitGeneration * Time.deltaTime;
                    break;
                case GeneratorType.Booster:
                    Booster targetBooster = _boosters[generator.TargetID];
                    BigDouble boosterGeneration = generator.GenerationRate * Time.deltaTime;
                    BigDouble maxBoosterCount = generator.GetCount() * 10;
                    if (targetBooster.Count + boosterGeneration >= maxBoosterCount)
                    {
                        targetBooster.Add(maxBoosterCount - targetBooster.Count);
                    }
                    else
                    {
                        targetBooster.Add(generator.GenerationRate * Time.deltaTime);
                    }
                    break;
                case GeneratorType.Generator:
                    Generator targetGenerator = _generators[generator.TargetID];
                    BigDouble generatorGeneration = generator.GenerationRate * (1 + BoosterMultiplier);
                    UnboostedRPS += generator.GenerationRate;
                    RPS += generatorGeneration;
                    targetGenerator.Add(generatorGeneration * Time.deltaTime);
                    break;
                case GeneratorType.RevolutionCounter:
                    RevolutionCounter targetCounter = _revolutionCounters[generator.TargetID];
                    BigDouble counterGeneration = generator.GenerationRate * Time.deltaTime;
                    BigDouble maxCounterCount = generator.GetCount() * 10;
                    if (targetCounter.Count + counterGeneration >= maxCounterCount)
                    {
                        targetCounter.Add(maxCounterCount - targetCounter.Count);
                    }
                    else
                    {
                        targetCounter.Add(generator.GenerationRate * Time.deltaTime);
                    }
                    _ratReturnRate += targetCounter.GetReturnRate();
                    break;
            }
        }
    }

    private void ConsumeBoosters()
    {
        BigDouble profitGeneratorCount = _generators[_profitGeneratorData.ID].GetCount();
        foreach (Booster booster in _boosters.Values)
        {
            BigDouble boosterConsumption = booster.GetConsumptioRate() * booster.Count * profitGeneratorCount * Time.deltaTime;
            booster.Remove(boosterConsumption);
        }
    }

    #endregion
}
