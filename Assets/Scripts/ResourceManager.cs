using BreakInfinity;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    public BigDouble Profit { get; private set; }
    public Generator Rats { get; private set; }

    [SerializeField] private GeneratorDataSO RatGeneratorData;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Rats = new(RatGeneratorData);
    }

    public void IncreasePrimaryResource(BigDouble amount)
    {
        Profit += amount;
    }

    public void BuyRat(int amount)
    {
        BigDouble cost = Rats.GetCost(amount);

        if (Profit >= cost)
        {
            Profit -= cost;
            Rats.Add(amount);
        }
    }
}
