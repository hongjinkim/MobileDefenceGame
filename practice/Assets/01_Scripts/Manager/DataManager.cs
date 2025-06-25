using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : BasicSingleton<DataManager>
{
    private PlayerData _playerData = new();
    

    public static PlayerData GetPlayerData()
	{
        return Instance._playerData;
    }

}
