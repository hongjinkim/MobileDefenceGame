using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DataBase))]
public class GameDataManager : MonoBehaviour
{
    public DataBase DataBase;

    private void Awake()
    {
        if (DataBase == null)
            DataBase = GetComponent<DataBase>();
    }

 
}
