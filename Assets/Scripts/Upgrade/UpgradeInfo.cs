using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class UpgradeInfo : MonoBehaviour
{

public TextMeshProUGUI description;
public TextMeshProUGUI title;
public TextMeshProUGUI visualiser;
public Image icon;

public GameObject purchase;
public GameObject costPanel;
public GameObject unlocked;

public Button buy;
public Image buyImage;

private bool canAfford;

[SerializeField] [AssetsOnly] private GameObject costItemPrefab;
[SerializeField] private int costItemCount;
private List<UpgradeCostItem> costItems = new List<UpgradeCostItem>();
public UpgradeNode SelectedNode {get; set;}

[Header("Starting Values")]
[LabelWidth(100),LabelText("Name")]
[SerializeField] private string startTitle;
[LabelWidth(100),Multiline(3),LabelText("Description")]
[SerializeField] private string startDesc;

    private void Awake()
    {
        for(int i = 0; i < costItemCount; i++)
        {
            costItems.Add(Instantiate(costItemPrefab, transform.position, Quaternion.identity, costPanel.transform).GetComponent<UpgradeCostItem>());
            costItems[i].gameObject.SetActive(false);
        }
    }

    public void StartInfo()
    {
            title.text = startTitle;
            description.text = startDesc;

            icon.gameObject.SetActive(false);
            purchase.SetActive(false);
            visualiser.gameObject.SetActive(false);
    }

    public void SetInfo(UpgradeScriptableObject upgrade, UpgradeNode node)
    {

        if (SelectedNode!= null && SelectedNode != node) {SelectedNode.DeselectNode();}

        SelectedNode = node;


        title.text = upgrade.upgradeName;
        description.text = upgrade.upgradeDescription;
        icon.sprite = upgrade.upgradeIcon;

        visualiser.text = new string("<color=#"+ColorUtility.ToHtmlStringRGB(GameManager.Instance.colourRed)+">"+ upgrade.oldValue + 
            "<color=#"+ColorUtility.ToHtmlStringRGB(GameManager.Instance.colourGray)+">"+"  ->  " + 
            "<color=#"+ColorUtility.ToHtmlStringRGB(GameManager.Instance.colourGreen)+">"+upgrade.newValue);

        canAfford = SetupCost(upgrade.upgradeCost);

        UpdateInfo();



        if (!icon.gameObject.activeSelf){icon.gameObject.SetActive(true);}
        if (!purchase.activeSelf) {purchase.SetActive(true);}
        if (!visualiser.gameObject.activeSelf) {visualiser.gameObject.SetActive(true);}

        LayoutRebuilder.ForceRebuildLayoutImmediate(costPanel.GetComponent<RectTransform>());

    }

    public void UpdateInfo()
    {
                // If the node can be bought but hasnt already
        if(SelectedNode.Unlocked && !SelectedNode.Purchased)
        {
            buyImage.color = GameManager.Instance.colourGreen;

            // If they can afford it
            if(!canAfford)
            {
                buy.interactable = false;
            }
            else
            {
                buy.interactable = true;
            }
        }
        else
        {
            buyImage.color = GameManager.Instance.colourGray;   
            buy.interactable = false;
        }

        // If the node has already been bought
        if(SelectedNode.Purchased)
        {
            buyImage.color = GameManager.Instance.colourGreen;
            buy.interactable = false;
            unlocked.SetActive(true);
        }
        else
        {
            unlocked.SetActive(false);
        }
    }

    public void UnlockedInfo()
    {
        UpdateInfo();

        foreach(UpgradeCostItem costItem in costItems)
        {
            costItem.count.color = GameManager.Instance.colourGreen;
        }
    }

    public bool SetupCost(List<UpgradeCost> costs)
    {
        bool allCanAfford = true;
        for(int i = 0; i < costItems.Count; i++)
        {
            if(i < costs.Count)
            {
                costItems[i].Setup(costs[i], SelectedNode.Purchased, SelectedNode.Unlocked);
                if (!costItems[i].CanAfford) {allCanAfford = false;}
            }
            else
            {
                costItems[i].gameObject.SetActive(false);
            }
        }
        return allCanAfford;
    }

}
