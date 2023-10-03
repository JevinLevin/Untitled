using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDrillUpgradeInterface : UpgradeInterface
{

    private bool SpeedGenerate1;
    private bool SpeedGenerate2;
    private bool SpeedGenerate3;
    private bool Capacity1;
    private bool ChromeDiamond1;

    [SerializeField] private LootTableScriptableObject defaultLootTable;
    [SerializeField] private LootTableScriptableObject chromeLootTable1;

    public override void BuyUpgrade()
    {
        base.BuyUpgrade();

        if(info.SelectedNode.upgrade.upgradeID == "speed1")
        {
            BuySpeedGenerate1();
        }
        if(info.SelectedNode.upgrade.upgradeID == "speed2")
        {
            BuySpeedGenerate2();
        }
        if(info.SelectedNode.upgrade.upgradeID == "speed3")
        {
            BuySpeedGenerate3();
        }
        if(info.SelectedNode.upgrade.upgradeID == "capacity1")
        {
            BuyCapacity1();
        }
        if(info.SelectedNode.upgrade.upgradeID == "chromediamond1")
        {
            BuyChromeDiamond1();
        }
    }

    private void BuySpeedGenerate1()
    {
        machine.generateTimeMultiplyer = 5 / 3.5f;
    }

    private void BuySpeedGenerate2()
    {
        machine.generateTimeMultiplyer = 5 / 2.25f;
    }

    private void BuySpeedGenerate3()
    {
        machine.generateTimeMultiplyer = 5;
    }

    private void BuyCapacity1()
    {
        machine.capacityMultiplyer = 10;
    }

    private void BuyChromeDiamond1()
    {
        machine.lootTable = chromeLootTable1;
    }

}
