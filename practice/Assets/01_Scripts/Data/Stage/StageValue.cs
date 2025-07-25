using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageValue
{
    [ShowInInspector]
    public int StageNum { get; private set; }

    public StageEnemyValue EnemyInfo = new StageEnemyValue(); // Enemy stats for this spawn

    public StageValue(int stageNum)
    {
        StageNum = stageNum;
    }
}
