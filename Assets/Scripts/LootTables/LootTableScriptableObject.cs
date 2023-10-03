using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootTableScriptableObject", menuName = "ScriptableObjects/LootTable")]

public class LootTableScriptableObject : ScriptableObject
{

    [SerializeField] private List<LootTableRolls> Pools = new List<LootTableRolls>();

    public List<ItemScriptableObject> GenerateLoot()
    {
        var output = new List<ItemScriptableObject>();
        foreach (LootTableRolls roll in Pools)
        {

            int totalWeight = roll.CalculateWeight();
            int currentWeight = 0;
            int chosenWeight = Random.Range(0,totalWeight);
            
            foreach(LootTableEntry entry in roll.rolls)
            {
                currentWeight += entry.weight;
                if (chosenWeight < currentWeight)
                {
                    if (Random.value < entry.randomChance)
                    {
                        for (int i = 0; i <= Random.Range(entry.countMin-1,entry.countMax); i++)
                        {
                        output.Add(entry.item);
                        }
                    }
                break;
                }
            }
        }   
        return output;
    }

    public void SpawnLoot(GameObject spawner)
    {
        List<ItemScriptableObject> drops;
        drops = GenerateLoot();

        foreach(ItemScriptableObject item in drops)
        {
            //item.Drop(spawner.transform.position);

            ItemSpawn.NewItem(item.ItemID, spawner.transform.position);
        }
    }

}
