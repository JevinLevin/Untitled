using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[System.Serializable]
public class LootTableRolls
{
    public List<LootTableEntry> rolls;
    [HideInInspector] public int totalWeight;

    public int CalculateWeight()
    {
        totalWeight = 0;
        foreach(LootTableEntry entry in rolls)
        {
            totalWeight += entry.weight;
        }
        return totalWeight;
    }
}

[System.Serializable]
public class LootTableEntry
{
    [PreviewField(60, ObjectFieldAlignment.Left), HideLabel]
    [HorizontalGroup("Split", 60)]
    public ItemScriptableObject item;
    [VerticalGroup("Split/Right"),LabelWidth(100)]
    public int weight = 1;
    [VerticalGroup("Split/Right")]
    [HorizontalGroup("Split/Right/Left"),LabelWidth(50)]
    public int countMin = 1;
    [VerticalGroup("Split/Right")]
    [HorizontalGroup("Split/Right/Left"),LabelWidth(50)]
    public int countMax = 1;
    [VerticalGroup("Split/Right", 100),LabelWidth(100)]
    [Range(0f,1f)]
    public float randomChance = 1.0f;
}
