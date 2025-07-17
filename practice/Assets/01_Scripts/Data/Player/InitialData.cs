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

    public InitialData()
    {
        LoadData();
    }

    private void LoadData()
    {
        var InitialData = DataTable.시작값.시작값List[0];

        GamePlayingSpeed = InitialData.게임진행속도;
        StartAttack = InitialData.기본_공격력;
        StartHealth = InitialData.기본_체력;
        AttackSpeed = InitialData.기본_공속;
        AttackRange = InitialData.기본_사거리;
    }
}
