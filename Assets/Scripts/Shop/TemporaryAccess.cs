using Game.Utils;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.PurchaseMethods
{
    public class TemporaryAccess : MonoBehaviour, IPurchaseStrategy
    {
        [SerializeField] private int duraion;
        public string MethodName => "Temporary Access";

        [SerializeField] private TMP_Text purchaseText;
        private void Start()
        {

            purchaseText.text = ($"{duraion} seconds");
        }
        public bool Purchase(IItem item)
        {
            if (item.IsUnlocked) return false;

            item.TemporarilyUnlock(duraion);
            DataManager.SaveItemStatus(new List<IItem> { item });
            return true;
        }
    }
}