using TMPro;
using UnityEngine;

namespace Game.Currencies
{
    public abstract class Currency : MonoBehaviour
    {
        public string Name { get; protected set; }
        public int Amount { get; protected set; }

        [SerializeField] protected TMP_Text currencyText;

        public abstract bool Spend(int cost);

        public abstract void Earn(int value);

        public void SetCurrencyAmount(int value)
        {
            Amount = value;
            UpdateUI();
        }
        protected virtual void UpdateUI()
        {
            currencyText.text = Name + " " + Amount.ToString();

        }
    }
}