using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageValue
{
    public int StageNum { get; private set; }

    public StageEnemyValue EnemyInfo = new StageEnemyValue(); // Enemy stats for this spawn

    [ShowInInspector, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout, KeyLabel = "Wave ID", ValueLabel = "Info")]
    public Dictionary<int, StageWaveValue> WaveValueDict = new Dictionary<int, StageWaveValue>(); //몬스터 등장정보

    public StageValue(int stageNum)
    {
        StageNum = stageNum;
    }
}
