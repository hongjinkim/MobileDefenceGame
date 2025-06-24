using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : BasicSingleton<DataManager>
{
    private PlayerData _playerData = new();
    private StageData stageData;

    public static PlayerData GetPlayerData()
	{
        return Instance._playerData;
    }
    public static StageData GetStageData()
    {
        return Instance._stageData;
    }

}
