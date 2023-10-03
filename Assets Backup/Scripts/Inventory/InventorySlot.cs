using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{

    public InventoryItem Item {get; set;}

    [SerializeField] private GameObject slotObject;
    [SerializeField] private TextMeshProUGUI stack;
    private Image slotIcon;

    void Awake()
    {
        slotIcon = slotObject.GetComponent<Image>();
    }

    public void AddItem(InventoryItem item)
    {
        SavingManager.ItemDict.TryGetValue(item.ItemID, out ItemScriptableObject itemType);
        slotIcon.enabled = true;
        slotIcon.sprite = itemType.ItemSprite;
        stack.text = item.StackSize.ToString();
    }

    public void ClearItem()
    {
        slotIcon.enabled = false;
        slotIcon.sprite = null;

    }

}
