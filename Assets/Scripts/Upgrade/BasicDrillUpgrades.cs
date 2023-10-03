//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class BasicDrillUpgrades : MonoBehaviour
//{
//
//    public struct UpgradeStruct
//    {
//        bool purchased;
//        UpgradeScriptableObject upgrade;
//    }
//
//    public List<UpgradeStruct> upgradeList;
//
//    [SerializeField] private LootTableScriptableObject defaultLootTable;
//    [SerializeField] private LootTableScriptableObject chromeLootTable1;
//
//    public void BuyUpgrade() 
//    {
//        if(info.SelectedNode.upgrade.upgradeID == "speed1")
//        {
//            BuySpeedGenerate1();
//        }
//        if(info.SelectedNode.upgrade.upgradeID == "speed2")
//        {
//            BuySpeedGenerate2();
//        }
//        if(info.SelectedNode.upgrade.upgradeID == "speed3")
//        {
//            BuySpeedGenerate3();
//        }
//        if(info.SelectedNode.upgrade.upgradeID == "capacity1")
//        {
//            BuyCapacity1();
//        }
//        if(info.SelectedNode.upgrade.upgradeID == "chromediamond1")
//        {
//            BuyChromeDiamond1();
//        }
//    }
//
//    private void BuySpeedGenerate1()
//    {
//        machine.generateTimeMultiplyer = 5 / 3.5f;
//        SpeedGenerate1 = true;
//    }
//
//    private void BuySpeedGenerate2()
//    {
//        machine.generateTimeMultiplyer = 5 / 2.25f;
//        SpeedGenerate2 = true;
//    }
//
//    private void BuySpeedGenerate3()
//    {
//        machine.generateTimeMultiplyer = 5;
//        SpeedGenerate3 = true;
//    }
//
//    private void BuyCapacity1()
//    {
//        machine.capacityMultiplyer = 10;
//        Capacity1 = true;
//    }
//
//    private void BuyChromeDiamond1()
//    {
//        machine.lootTable = chromeLootTable1;
//        ChromeDiamond1 = true;
//    }
//
//}
