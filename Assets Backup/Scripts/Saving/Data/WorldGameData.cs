using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldGameData
{

    public long lastUpdated;

    public List<float> machineTiles;

    public WorldGameData()
    {
        machineTiles = new List<float>();
    }

}
