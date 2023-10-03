using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeCostItem : MonoBehaviour
{

    [SerializeField] public Image icon;
    [SerializeField] public TextMeshProUGUI count;
    public bool CanAfford {get; set;}

    private int playerCount;

    public void CheckAfford(UpgradeCost cost)
    {
        playerCount = Player.Instance.Inventory.FindItemCount(cost.item);

        if(playerCount >= cost.count)
        {
            CanAfford = true;
        }
        else
        {
            CanAfford = false;
        }

    }

    public void Setup(UpgradeCost cost, bool purchased, bool unlocked)
    {

        CheckAfford(cost); 

        icon.sprite = cost.item.ItemSprite;

        if (CanAfford)
        {
            count.text = NumberManager.DisplayNumber(cost.count);
            count.color = Color.white;
        }
        else
        {
            count.text = NumberManager.DisplayNumber(cost.count) + "("+ NumberManager.DisplayNumber(playerCount) +")";
            count.color = GameManager.Instance.colourRed;
        }

        if(!unlocked)
        {
            count.color = GameManager.Instance.colourRed;
        }

        if(purchased)
        {
            count.color = GameManager.Instance.colourGreen;
        }


        gameObject.SetActive(true);
    }

}
