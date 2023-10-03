using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    //private Dictionary<InventoryItem, InventorySlot> inventoryDictionary = new Dictionary<InventoryItem, InventorySlot>();
    private InventorySlot[] slots;



    private void Awake()
    {
        slots = GetComponentsInChildren<InventorySlot>();

        //foreach(var slot in slots)
        //{
        //    inventoryDictionary.Add(slot.Item, slot);
        //}
    }


    public void UpdateUI()
    {

        for(var i = 0; i < Player.Instance.Inventory.invList.Count; i++)
        {
            slots[i].AddItem(Player.Instance.Inventory.invList[i]); 
        }

    }

}
