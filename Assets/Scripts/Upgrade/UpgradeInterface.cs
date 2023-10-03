using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeInterface : MonoBehaviour
{

    [SerializeField] protected UpgradeInfo info;
    [SerializeField] private List<UpgradeNode> nodeList;
    [SerializeField] protected MachineTile machine;



    public void Activate()
    {
        gameObject.SetActive(true);
        info.StartInfo();
    }

    public void BuyUpgrade()
    {
        // Marks the node as purchased
        info.SelectedNode.Purchase();
        info.UnlockedInfo();

        foreach(UpgradeCost cost in info.SelectedNode.upgrade.upgradeCost) 
        {
            Player.Instance.Inventory.RemoveFromInventory(cost.item, cost.count);
        }


    }

}
