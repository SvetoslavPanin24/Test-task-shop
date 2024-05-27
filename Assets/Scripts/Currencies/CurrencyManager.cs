using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [field: SerializeField] private List<Currency> currencies;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        LoadCurrencies();
    }
    private void LoadCurrencies()
    {
        DataManager.LoadCurrencies(currencies);
    }

    private void OnApplicationQuit()
    {
        DataManager.SaveCurrencies(currencies);
    }

    public Currency GetCurrency(string name)
    {
        Currency currency = currencies.Find(c => c.Name == name);
        return currency;
    }
}
