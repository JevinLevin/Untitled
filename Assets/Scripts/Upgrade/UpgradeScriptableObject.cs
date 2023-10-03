using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "MachineUpgrade", menuName = "ScriptableObjects/Upgrades")]
public class UpgradeScriptableObject : ScriptableObject
{

    [PreviewField(60, ObjectFieldAlignment.Left), HideLabel]
    [HorizontalGroup("Split", 60)]
    public Sprite upgradeIcon;
    [VerticalGroup("Split/Right"),LabelWidth(100),LabelText("ID")]
    public string upgradeID;
    
    [VerticalGroup("Split/Right"),LabelWidth(100),LabelText("Name")]
    public string upgradeName;

    [HorizontalGroup("Split/Right/Values"),LabelWidth(100),LabelText("Old")]
    public string oldValue;
    [HorizontalGroup("Split/Right/Values"),LabelWidth(100),LabelText("New")]
    public string newValue;

    [LabelWidth(80),Multiline(3),LabelText("Description")]
    public string upgradeDescription;

    [LabelText("Cost")]    
    public List<UpgradeCost> upgradeCost;

} 

