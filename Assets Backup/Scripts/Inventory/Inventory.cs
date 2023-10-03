using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> invList = new List<InventoryItem>();
    public Dictionary<string, InventoryItem> invDictionary = new Dictionary<string, InventoryItem>();

    public int Count {get; set;}

    public enum InventoryTypes{
        Standard,
        Catalogue

    } 

    public InventoryTypes InventoryType;

    [SerializeField] private CatalogueScriptableObject catalogueTemplate;

    void Awake()
    {
        if(InventoryType == InventoryTypes.Catalogue)
        {

            var instance = ScriptableObject.Instantiate(catalogueTemplate);
            invList = instance.Catalogue;
        }
    }

    public void SetupDictionary(List<InventoryItem> inventory)
    {
        invDictionary.Clear();
        foreach(InventoryItem item in inventory)
        {
            invDictionary.Add(item.ItemID,item);
        }

    }


    public void AddToInventory(string itemID)
    {
        if(invDictionary.TryGetValue(itemID, out InventoryItem invItem))
        {
            invItem.StackSize++;
        }
        else
        {
        InventoryItem newInvItem = new InventoryItem(itemID);
        invList.Add(newInvItem);
        invDictionary.Add(itemID, newInvItem);

        }

        Count++;
    }

    public void RemoveFromInventory(ItemScriptableObject itemData, int count) 
    {
        for(int i = 0; i < count; i++)
        {
            if (invDictionary.TryGetValue(itemData.ItemID, out InventoryItem invItem))
            {
                invItem.StackSize--;
                if (InventoryType == InventoryTypes.Standard && invItem.StackSize == 0)
                {
                    invList.Remove(invItem);
                    invDictionary.Remove(itemData.ItemID);
                }
            }
            Count--;
        }
    }

    public int FindItemCount(ItemScriptableObject itemData)
    {
        if (invDictionary.TryGetValue(itemData.ItemID, out InventoryItem invItem))
        {
            return (int)invItem.StackSize;
        }
        return 0;
    }

    public void Clear()
    {
        invList.Clear();
        invDictionary.Clear();
        Count = 0;
    }

}