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

    [Header("Boosters")]
    [SerializeField] private List<BoosterDataSO> _boosterDatas;

    public static ResourceManager Instance { get; private set; }
    public BigDouble Profit { get; private set; }
    public BigDouble BoosterMultiplier { get; private set; }
    private Dictionary<string, Generator> _generators = new(); // ID lookup
    private Dictionary<string, Booster> _boosters = new();

    // For UI
    public BigDouble PPS { get; private set; } // Profit per second
    public BigDouble RPS { get; private set; } // Rats per second
    public BigDouble UnboostedRPS { get; private set; }

    #region Public Methods

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

    public void CreateClickResource()
    {
        Profit += 1 + BoosterMultiplier;
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

        // Make sure boosters and generators exists before anything else
        CreateBoosters();
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
                    targetBooster.Add(generator.GenerationRate * Time.deltaTime);
                    break;
                case GeneratorType.Generator:
                    Generator targetGenerator = _generators[generator.TargetID];
                    BigDouble generatorGeneration = generator.GenerationRate * (1 + BoosterMultiplier);
                    UnboostedRPS += generator.GenerationRate;
                    RPS += generatorGeneration;
                    targetGenerator.Add(generatorGeneration * Time.deltaTime);
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
