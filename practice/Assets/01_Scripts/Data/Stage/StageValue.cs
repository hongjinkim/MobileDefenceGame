using System.Collections;
using UnityEngine;


public class StageValue
{
    public int StageNum { get; private set; }

    public StageWaveValue[] Waves { get; set; } // 스테이지의 웨이브 정보

    public StageEnemyValue[] Boss { get; set; } // 스테이지의 보스 정보

    public StageValue(int stageNum)
    {
        StageNum = stageNum;
    }
}
