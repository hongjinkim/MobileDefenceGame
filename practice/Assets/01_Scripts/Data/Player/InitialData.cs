using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InitialData
{

    public float GamePlayingSpeed;

    public int StartAttack;
    public int StartHealth;
    public float AttackSpeed;
    public float AttackRange;

    public void LoadData()
    {
        var InitialData = DataTable.Initial.GetList()[0];

        GamePlayingSpeed = InitialData.GameSpeed;
        StartAttack = InitialData.Attack;
        StartHealth = InitialData.HP;
        AttackSpeed = InitialData.AttackSpeed;
        AttackRange = InitialData.AttackRange;
    }
}
