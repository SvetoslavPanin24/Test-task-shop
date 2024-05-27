using TMPro;
using TNRD;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseButton : MonoBehaviour
{    
    [Header("Initialize item to purshase")]
    [SerializeField] private int itemId;  
    private IItem item;

    [Header("Initialize purshase method")]
    [SerializeField] private SerializableInterface<IPurchaseStrategy> method;

    private void Start()
    {
        item = ShopManager.Instance.GetItem(itemId);
    }

    public void OnClick()
    {
        if (item.IsUnlocked)
        {
            Debug.Log($"{item.Name} already unlocked");
        }

        if (method.Value.Purchase(item))
        {
            // Update item status and UI
            Debug.Log($"{item.Name} purchased with {method.Value.MethodName}");
        }
        else
        {
            Debug.Log($"Failed to purchase {item.Name} with {method.Value.MethodName}");
        }
    }
}

