using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PurchaseWithDonateCurrencyPurchaseWithDonateCurrency : MonoBehaviour, IPurchaseStrategy
{
    [SerializeField] private string CurrencyName;

    private DonateCurrency currency;
    [SerializeField] private int cost;

    [SerializeField] private TMP_Text purchaseText;

    private void Start()
    {
        currency = (DonateCurrency)CurrencyManager.Instance.GetCurrency(CurrencyName);
        purchaseText.text = ($"{cost} {currency.Name}");
    }

    public string MethodName => "Game Currency Purchase";

    public bool Purchase(IItem item)
    {
        if (item.IsUnlocked) return false;

        if (currency.Amount >= cost)
        {
            currency.Spend(cost);
            item.Unlock();
            DataManager.SaveItemStatus(new List<IItem> { item });
            return true;
        }
        return false;
    }
}
