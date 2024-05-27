using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }
    [SerializeField] private List<Item> itemList;
 
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
        LoadItems();        
    }

    private void LoadItems()
    {
        DataManager.LoadItemStatus(itemList.ConvertAll(item => (IItem)item));
    }
 
    private void OnApplicationQuit()
    {
        DataManager.SaveItemStatus(itemList.ConvertAll(item => (IItem)item));
    }

    public IItem GetItem(int id)
    {
        return itemList.Find(item => item.Id == id);
    }
}
