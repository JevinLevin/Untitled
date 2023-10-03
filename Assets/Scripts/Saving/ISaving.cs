using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaving
{

    void LoadData(PlayerGameData playerData, WorldGameData worldData);
    void SaveData (PlayerGameData playerData, WorldGameData worldData);


}
