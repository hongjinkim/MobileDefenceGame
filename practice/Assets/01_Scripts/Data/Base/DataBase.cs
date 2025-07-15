using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEngine;

public class DataBase : BasicSingleton<DataBase>
{
    private DataReader data;

    private void Awake()
    {
        data = DataReader.Instance;
    }

    public static PlayerData GetPlayerData()
    {
        return Instance.data.PlayerData;
    }

    public static InitialData GetInitialData()
    {
        return Instance.data.InitialData;
    }

    // Uncomment the following lines to debug the data loading

    //foreach (var value in DefaultTable.Data.DataList)
    //       {
    //           Debug.Log(value.index + "," + value.intValue + "," + value.strValue);
    //       }
    //var dataFromMap = DefaultTable.Data.DataMap[0];
    //Debug.Log("dataFromMap : " + dataFromMap.index + ", " + dataFromMap.intValue + "," + dataFromMap.strValue);

}
