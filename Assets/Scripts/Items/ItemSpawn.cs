using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSpawn
{

    [ClearOnReload(assignNewTypeInstance:true)]
    public static List<ItemSpawn> itemSpawnBuffer = new List<ItemSpawn>();

    private string ItemID;
    private Vector3 itemPosition;
    private int itemCount;

    public ItemSpawn(string itemID, Vector3 position)
    {
        ItemID = itemID;
        itemPosition = position;
        itemCount = 1;

    }

    public static void NewItem(string itemID, Vector3 position)
    {
        bool newClass = true;
        foreach(ItemSpawn item in itemSpawnBuffer)
        {
            if (item.ItemID == itemID && item.itemPosition == position)
            {
                item.itemCount++;
                newClass = false;
            }
        }
        if (newClass)
        {
            itemSpawnBuffer.Add(new ItemSpawn(itemID, position));
        }
    }

    public static void Spawn()
    {
        
        foreach(ItemSpawn item in itemSpawnBuffer)
        {
            SavingManager.ItemDict.TryGetValue(item.ItemID, out ItemScriptableObject itemType);
            itemType.Drop(item.itemPosition, item.itemCount);
        }

        

        itemSpawnBuffer = new List<ItemSpawn>();
    }

}
