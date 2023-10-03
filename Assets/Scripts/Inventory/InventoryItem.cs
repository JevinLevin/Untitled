using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{

    public int StackSize;

    public string ItemID;

    public InventoryItem(string itemID)
    {
        ItemID = itemID;
        StackSize++;
    }

}
