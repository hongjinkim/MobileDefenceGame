using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InitialData
{

    public float GameSpeed;
    public float AttackSpeed;
    public float AttackRange_Melee;
    public float AttackRange_Range;

    public void LoadData()
    {
        var InitialData = DataTable.Initial.GetList()[0];

        GameSpeed = InitialData.GameSpeed;
        AttackSpeed = InitialData.AttackSpeed;
        AttackRange_Melee = InitialData.AttackRange_Melee;
        AttackRange_Range = InitialData.AttackRange_Range;
    }
}
