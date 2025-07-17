using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageValue
{
    public int StageNum { get; private set; }

    public StageEnemyValue EnemyInfo = new StageEnemyValue(); // Enemy stats for this spawn

    public Dictionary<int, StageWaveValue> WaveValueDict = new Dictionary<int, StageWaveValue>();

    public StageValue(int stageNum)
    {
        StageNum = stageNum;
    }
}
