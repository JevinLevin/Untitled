using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MachineUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI iconCount;
    [SerializeField] private Image iconSprite;
    public float Count {get; set;}

    public void ChangeCount(float value)
    {
        Count = value;
        iconCount.text = NumberManager.DisplayNumber(value);
    }

    public void Setup(string itemID, float newCount)
    {
        SavingManager.ItemDict.TryGetValue(itemID, out ItemScriptableObject item);
        iconSprite.sprite = item.ItemSprite;
        iconCount.text = NumberManager.DisplayNumber(newCount);
    }

}
