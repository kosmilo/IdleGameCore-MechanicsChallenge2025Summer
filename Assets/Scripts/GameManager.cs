using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Update()
    {
        ResourceManager.Instance.IncreasePrimaryResource(ResourceManager.Instance.Rats.GenerationRate * Time.deltaTime);
    }

    public void CreateClickResource()
    {
        ResourceManager.Instance.IncreasePrimaryResource(1);
    }

    internal void BuyRat(int amount)
    {
        ResourceManager.Instance.BuyRat(amount);
    }
}
