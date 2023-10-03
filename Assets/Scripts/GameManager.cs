using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    [ClearOnReload(assignNewTypeInstance:true)]
    public static List<ItemObject> Items = new List<ItemObject>();

    public ItemNotificationManager ItemNotifications;

    public Color colourRed;
    public Color colourLightRed;
    public Color colourGreen;
    public Color colourGray;
    public Color colourDarkGray;

    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
            return;
        } 
        else 
        { 
            Instance = this; 
        } 

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {

        // Spawns all ready items
        if(ItemSpawn.itemSpawnBuffer.Count > 0)
        {
            ItemSpawn.Spawn();
        }

    }

}
