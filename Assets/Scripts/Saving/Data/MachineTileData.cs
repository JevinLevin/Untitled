using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MachineTileData
{
    public List<InventoryItem> machineInventory;
    public int inventoryCount;
    public float generateTime;

    public MachineTileData()
    {
        machineInventory = new List<InventoryItem>();
        inventoryCount = 0;

        generateTime = 0;
    }

    public void SetMachineTileData(List<InventoryItem> machineInventory, int inventoryCount, float generateTime)
    {
        this.machineInventory = machineInventory;
        this.inventoryCount = inventoryCount;

        this.generateTime = generateTime;
    }
}