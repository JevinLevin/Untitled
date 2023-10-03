using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerGameData 
{

    public long lastUpdated;

    public List<InventoryItem> playerInventory;
    public Vector3 playerPosition;

    public PlayerGameData()
    {
        playerInventory = new List<InventoryItem>();
        playerPosition = Vector3.zero;
    }

}
