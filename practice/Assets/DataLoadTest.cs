using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEngine;

public class DataLoadTest : MonoBehaviour
{
    void Awake()
    {
        UnityGoogleSheet.LoadAllData();
        // UnityGoogleSheet.Load<DefaultTable.Data.Load>(); it's same!
        // or call DefaultTable.Data.Load(); it's same!
    }

    void Start()
    {
        foreach (var value in DefaultTable.Data.DataList)
        {
            Debug.Log(value.index + "," + value.intValue + "," + value.strValue);
        }
        var dataFromMap = DefaultTable.Data.DataMap[0];
        Debug.Log("dataFromMap : " + dataFromMap.index + ", " + dataFromMap.intValue + "," + dataFromMap.strValue);
    }
}
