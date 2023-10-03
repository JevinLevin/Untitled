using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

public class UpgradeNode : MonoBehaviour
{

    [SerializeField] public UpgradeScriptableObject upgrade;

    [ChildGameObjectsOnly]
    [SerializeField] private Image icon;
    [SerializeField] private Image box;

    [SerializeField] private List<UpgradeNode> children;
    [SerializeField] private UpgradeInfo infoPanel;

    [SerializeField] private Sprite defaultBox;
    [SerializeField] private Sprite selectedBox;
    [SerializeField] private GameObject tick;
    [SerializeField] private bool root;

    public bool Purchased {get; set;}
    public bool Unlocked {get; set;}

    void Awake()
    {
        if(root)
        {
            Unlocked = true;
            box.color = Color.white;
            icon.color = Color.white;
        }
    }


    [Button]
    void SetComponents()
    {
        icon.sprite = upgrade.upgradeIcon;

#if UNITY_EDITOR
        PrefabUtility.RecordPrefabInstancePropertyModifications(icon);
#endif
    }

    public void DeselectNode()
    {
        box.sprite = defaultBox;
    }

    public void SelectNode()
    {
        box.sprite = selectedBox;
        infoPanel.SetInfo(upgrade, this);
    }

    public void UnlockChildren()
    {
        foreach(UpgradeNode node in children)
        {
            node.Unlocked = true;
            node.box.color = Color.white;
            node.icon.color = Color.white;
        }
    }

    public void Purchase()
    {
        Purchased = true;
        tick.SetActive(true);
        UnlockChildren();
    }




}
