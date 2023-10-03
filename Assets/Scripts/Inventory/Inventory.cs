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
            if(invDictionary.ContainsKey(item.ItemID))
            {
                invDictionary.Remove(item.ItemID);
            }
            invDictionary.Add(item.ItemID,item);
        }

    }


    // Adds the number of items to the inventory dictionary
    public void AddToInventory(string itemID, int count)
    {
        if(invDictionary.TryGetValue(itemID, out InventoryItem invItem))
        {
            invItem.StackSize += count;
        }
        else
        {
        InventoryItem newInvItem = new InventoryItem(itemID);

        newInvItem.StackSize = count;

        invList.Add(newInvItem);
        invDictionary.Add(itemID, newInvItem);

        }

        Count += count;
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