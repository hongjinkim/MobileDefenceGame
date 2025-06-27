using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEngine;

public class DataManager : BasicSingleton<DataManager>
{
    private PlayerData _playerData = new();

	private void Awake()
	{
		UnityGoogleSheet.LoadAllData();
	}


	public static PlayerData GetPlayerData()
	{
        return Instance._playerData;
    }

	//foreach (var value in DefaultTable.Data.DataList)
 //       {
 //           Debug.Log(value.index + "," + value.intValue + "," + value.strValue);
 //       }
	//var dataFromMap = DefaultTable.Data.DataMap[0];
	//Debug.Log("dataFromMap : " + dataFromMap.index + ", " + dataFromMap.intValue + "," + dataFromMap.strValue);

}
