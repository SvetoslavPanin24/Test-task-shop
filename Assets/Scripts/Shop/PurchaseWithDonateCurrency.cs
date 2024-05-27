using Game.Currencies;
using Game.Managers;
using Game.Utils;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.PurchaseMethods
{
    public class PurchaseWithDonateCurrency : MonoBehaviour, IPurchaseStrategy
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

        public string MethodName => "Donate Currency Purchase";

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
}
