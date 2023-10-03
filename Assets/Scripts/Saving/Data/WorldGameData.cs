using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldGameData
{

    public long lastUpdated;

    public  Dictionary<string, MachineTileData> machineTiles;

    public WorldGameData()
    {
        machineTiles = new Dictionary<string, MachineTileData>();
    }

}
