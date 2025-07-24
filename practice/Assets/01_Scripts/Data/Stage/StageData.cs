using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class StageData
{
    /* 적 시트 */
    


    private int MaxStage { get; set; } // 스테이지 배경 맵 개수
    /* 스테이지 시트 */
    public int StageEnemyCountStart { get; set; }
    public int StageEnemyCountRaise { get; set; }
    public int StageEnemyCountUnit { get; set; }
    public int StageEnemyCountMax { get; set; }


    // 스테이지 정보 사전
    public Dictionary<int, StageValue> StageDict = new Dictionary<int, StageValue>();
    //public Dictionary<int, StageWaveValue> StageMonDict = new Dictionary<int, StageWaveValue>(); //몬스터 등장정보
    public Dictionary<int, string> StageNameDict = new Dictionary<int, string>(); // 스테이지 배경명
                                                                                  //private int mapCount = 0;

    public StageData()
    {
        LoadData();
    }   

    private void LoadData()
    {
        MaxStage = 2;//DataTable.스테이지.스테이지List.Max(stage => stage.스테이지_스테이지ID);

        for (int i = 1; i <= this.MaxStage; i++)
        {
            StageValue Stage = new StageValue(i);


            // 스테이지 적 스탯
            // 적 체력
            Stage.EnemyInfo.EnemyHP.Start = DataTable.적.적Map["Start"].적_체력;
            Stage.EnemyInfo.EnemyHP.Constant = DataTable.적.적Map["Constant"].적_체력;
            Stage.EnemyInfo.EnemyHP.Exponent = (double)DataTable.적.적Map["Exponent"].적_체력;
            Stage.EnemyInfo.EnemyHP.SetEnemyStat(i);

            // 적 공격력
            Stage.EnemyInfo.EnemyAttack.Start = DataTable.적.적Map["Start"].적_공격력;
            Stage.EnemyInfo.EnemyAttack.Constant = DataTable.적.적Map["Constant"].적_공격력;
            Stage.EnemyInfo.EnemyAttack.Exponent = (double)DataTable.적.적Map["Exponent"].적_공격력;
            Stage.EnemyInfo.EnemyAttack.SetEnemyStat(i);

            // 적 골드
            Stage.EnemyInfo.EnemyGold.Start = DataTable.적.적Map["Start"].적_골드드랍;
            Stage.EnemyInfo.EnemyGold.Constant = DataTable.적.적Map["Start"].적_골드드랍;
            Stage.EnemyInfo.EnemyGold.Exponent = (double)DataTable.적.적Map["Exponent"].적_골드드랍;
            Stage.EnemyInfo.EnemyGold.SetEnemyStat(i);

            // 적 보스 공격력 배수
            Stage.EnemyInfo.BossAttackMultiplier.Start = DataTable.적.적Map["Start"].적_보스공격력배수;
            Stage.EnemyInfo.BossAttackMultiplier.Constant = DataTable.적.적Map["Start"].적_보스공격력배수;
            Stage.EnemyInfo.BossAttackMultiplier.Exponent = (double)DataTable.적.적Map["Start"].적_보스공격력배수;
            Stage.EnemyInfo.BossAttackMultiplier.SetEnemyStat(i);

            // 적 보스 체력 배수
            Stage.EnemyInfo.BossHPMultiplier.Start = DataTable.적.적Map["Start"].적_보스체력배수;
            Stage.EnemyInfo.BossHPMultiplier.Constant = DataTable.적.적Map["Start"].적_보스체력배수;
            Stage.EnemyInfo.BossHPMultiplier.Exponent = (double)DataTable.적.적Map["Start"].적_보스체력배수;
            Stage.EnemyInfo.BossHPMultiplier.SetEnemyStat(i);

            // 웨이브 정보
            var waveDict = Stage.WaveValueDict;
            var lastWave = DataTable.스테이지.스테이지List.Max(wave => wave.스테이지_웨이브ID);
            for(int j = 1; j <= lastWave; i++)
            {
                var waves = DataTable.스테이지.스테이지List.Where(wave => wave.스테이지_웨이브ID == i).ToList();
                foreach (var wave in waves)
                {
                    waveDict[j] = new StageWaveValue
                    {
                        ID = wave.스테이지_웨이브ID,
                    };
                    var enemySpawnData = new EnemySpawnData
                    {
                        EnemyID = wave.스테이지_적ID,
                        SpawnCount = wave.스테이지_적수,
                        SpawnDelay = wave.스테이지_딜레이,
                    };
                    waveDict[j].spawnDatas.Add(enemySpawnData);
                 }
            }
            


            StageDict[i] = Stage;
        }
    }
}
