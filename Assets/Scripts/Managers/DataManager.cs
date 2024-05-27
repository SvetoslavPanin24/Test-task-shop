using UnityEngine;
using System.Collections.Generic;
using Game.Currencies;

namespace Game.Utils
{
    public static class DataManager
    {
        private const string ItemStatusKey = "ItemStatus";
        private const string CurrencyKey = "CurrencyData";
        private const string HashSuffix = "_hash";

        public static void SaveItemStatus(List<IItem> items)
        {
            List<ItemStatus> itemStatuses = new List<ItemStatus>();

            foreach (var item in items)
            {
                float remainingTime = item.IsTemporary ? (item.UnlockTime - Time.time) : 0;
                itemStatuses.Add(new ItemStatus
                {
                    Id = item.Id,
                    IsUnlocked = item.IsUnlocked,
                    IsTemporary = item.IsTemporary,
                    UnlockTime = item.UnlockTime,
                    RemainingTime = remainingTime
                });
            }

            string json = JsonUtility.ToJson(new ItemStatusList { Items = itemStatuses });
            string encryptedJson = CryptoUtils.EncryptString(json);
            string hash = HashUtils.ComputeHash(encryptedJson);

            PlayerPrefs.SetString(ItemStatusKey, encryptedJson);
            PlayerPrefs.SetString(ItemStatusKey + HashSuffix, hash);
            PlayerPrefs.Save();
        }

        public static void LoadItemStatus(List<IItem> items)
        {
            if (!PlayerPrefs.HasKey(ItemStatusKey)) return;

            string encryptedJson = PlayerPrefs.GetString(ItemStatusKey);
            string hash = PlayerPrefs.GetString(ItemStatusKey + HashSuffix);

            if (!HashUtils.VerifyHash(encryptedJson, hash))
            {
                Debug.LogWarning("Data tampering detected!");
                return;
            }

            string json = CryptoUtils.DecryptString(encryptedJson);
            ItemStatusList itemStatusList = JsonUtility.FromJson<ItemStatusList>(json);

            foreach (var status in itemStatusList.Items)
            {
                IItem item = items.Find(i => i.Id == status.Id);
                if (item != null)
                {
                    item.IsUnlocked = status.IsUnlocked;
                    item.IsTemporary = status.IsTemporary;
                    item.UnlockTime = status.UnlockTime;

                    if (item.IsTemporary && status.RemainingTime > 0)
                    {
                        item.TemporarilyUnlock(status.RemainingTime);
                    }
                    else if (item.IsTemporary)
                    {
                        item.Lock();
                    }
                }
            }
        }

        public static void SaveCurrencies(List<Currency> currencies)
        {
            List<CurrencyData> currencyData = new List<CurrencyData>();

            foreach (var currency in currencies)
            {
                currencyData.Add(new CurrencyData
                {
                    Name = currency.Name,
                    Amount = currency.Amount
                });
            }

            string json = JsonUtility.ToJson(new CurrencyDataList { Currencies = currencyData });
            string encryptedJson = CryptoUtils.EncryptString(json);
            string hash = HashUtils.ComputeHash(encryptedJson);

            PlayerPrefs.SetString(CurrencyKey, encryptedJson);
            PlayerPrefs.SetString(CurrencyKey + HashSuffix, hash);
            PlayerPrefs.Save();
        }

        public static void LoadCurrencies(List<Currency> currencies)
        {          
            if (!PlayerPrefs.HasKey(CurrencyKey))
            {
                //logic for test
                foreach (Currency data in currencies)
                {
                    Currency currency = currencies.Find(c => c.Name == data.Name);

                    if (currency as GameCurrency)
                    {
                        currency.SetCurrencyAmount(1500);
                    }
                    if (currency as DonateCurrency)
                    {
                        currency.SetCurrencyAmount(20);
                    }
                }
                return;
            }

            string encryptedJson = PlayerPrefs.GetString(CurrencyKey);
            string hash = PlayerPrefs.GetString(CurrencyKey + HashSuffix);

            if (!HashUtils.VerifyHash(encryptedJson, hash))
            {
                Debug.LogWarning("Data tampering detected!");
                return;
            }

            string json = CryptoUtils.DecryptString(encryptedJson);
            CurrencyDataList currencyDataList = JsonUtility.FromJson<CurrencyDataList>(json);

            foreach (var data in currencyDataList.Currencies)
            {
                Currency currency = currencies.Find(c => c.Name == data.Name);
                if (currency != null)
                {
                    currency.SetCurrencyAmount(data.Amount);
                }
                else
                {
                    if (currency as GameCurrency)
                    {
                        currency.SetCurrencyAmount(1500);
                    }
                    if (currency as DonateCurrency)
                    {
                        currency.SetCurrencyAmount(20);
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class ItemStatus
    {
        public int Id;
        public bool IsUnlocked;
        public bool IsTemporary;
        public float UnlockTime;
        public float RemainingTime;
    }

    [System.Serializable]
    public class ItemStatusList
    {
        public List<ItemStatus> Items;
    }

    [System.Serializable]
    public class CurrencyData
    {
        public string Name;
        public int Amount;
    }

    [System.Serializable]
    public class CurrencyDataList
    {
        public List<CurrencyData> Currencies;
    }
}
