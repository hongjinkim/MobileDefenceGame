
using Sirenix.OdinInspector;
using Sirenix.Serialization;
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

    // 스테이지 정보 사전
    [ShowInInspector, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout, KeyLabel = "Stage ID", ValueLabel = "Info")]
    public Dictionary<string, StageValue> StageDict = new Dictionary<string, StageValue>();
    [ShowInInspector, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout, KeyLabel = "Stage ID", ValueLabel = "Stage Name")]
    public Dictionary<int, string> StageNameDict = new Dictionary<int, string>(); // 스테이지 배경명
                                                                                  //private int mapCount = 0;


    public void LoadData()
    {
        MaxStage = DataTable.StageInfo.StageInfoList.Count;

        for (int i = 1; i <= MaxStage; i++)
        {
            StageValue Stage = new StageValue();
            Stage.StageNum = i;
            var EnemyDict = DataTable.Enemy.GetDictionary();

            // 스테이지 적 스탯
            // 적 체력
            Stage.EnemyInfo.EnemyHP.Start = DataTable.Enemy.EnemyMap["Start"].Monster_HP;
            Stage.EnemyInfo.EnemyHP.Constant = DataTable.Enemy.EnemyMap["Constant"].Monster_HP;
            Stage.EnemyInfo.EnemyHP.Exponent = (double)DataTable.Enemy.EnemyMap["Exponent"].Monster_HP;
            Stage.EnemyInfo.EnemyHP.SetEnemyStat(i);

            // 적 공격력
            Stage.EnemyInfo.EnemyAttack.Start = DataTable.Enemy.EnemyMap["Start"].Monster_Attack;
            Stage.EnemyInfo.EnemyAttack.Constant = DataTable.Enemy.EnemyMap["Constant"].Monster_Attack;
            Stage.EnemyInfo.EnemyAttack.Exponent = (double)DataTable.Enemy.EnemyMap["Exponent"].Monster_Attack;
            Stage.EnemyInfo.EnemyAttack.SetEnemyStat(i);

            // 적 골드
            Stage.EnemyInfo.EnemyGold.Start = DataTable.Enemy.EnemyMap["Start"].Monster_GoldDrop;
            Stage.EnemyInfo.EnemyGold.Constant = DataTable.Enemy.EnemyMap["Start"].Monster_GoldDrop;
            Stage.EnemyInfo.EnemyGold.Exponent = (double)DataTable.Enemy.EnemyMap["Exponent"].Monster_GoldDrop;
            Stage.EnemyInfo.EnemyGold.SetEnemyStat(i);

            // 적 보스 공격력 배수
            Stage.EnemyInfo.BossAttackMultiplier.Start = DataTable.Enemy.EnemyMap["Start"].Boss_AttackMultiply;
            Stage.EnemyInfo.BossAttackMultiplier.Constant = DataTable.Enemy.EnemyMap["Start"].Boss_AttackMultiply;
            Stage.EnemyInfo.BossAttackMultiplier.Exponent = (double)DataTable.Enemy.EnemyMap["Start"].Boss_AttackMultiply;
            Stage.EnemyInfo.BossAttackMultiplier.SetEnemyStat(i);

            // 적 보스 체력 배수
            Stage.EnemyInfo.BossHPMultiplier.Start = DataTable.Enemy.EnemyMap["Start"].Boss_HPMultiply;
            Stage.EnemyInfo.BossHPMultiplier.Constant = DataTable.Enemy.EnemyMap["Start"].Boss_HPMultiply;
            Stage.EnemyInfo.BossHPMultiplier.Exponent = (double)DataTable.Enemy.EnemyMap["Start"].Boss_HPMultiply;
            Stage.EnemyInfo.BossHPMultiplier.SetEnemyStat(i);

            // 웨이브 정보
            var stageRows = DataTable.Stage.StageList
            .Where(row => row.Stage_ID == i)   // 스테이지 단위 필터
            .ToList();

            var newDict = new Dictionary<int, StageWaveValue>();

            foreach (var group in stageRows.GroupBy(r => r.Wave_ID)) // Wave_ID로 묶기
            {
                var stageWave = new StageWaveValue();

                foreach (var row in group)
                {
                    stageWave.SpawnDatas.Add(new EnemySpawnData
                    {
                        SpawnPattern = row.Wave_Pattern,
                        EnemyID = row.Wave_EnemyID,
                        SpawnCount = row.Wave_EnemyCount,
                        SpawnDelay = row.Wave_Delay,
                    });
                }

                newDict[group.Key] = stageWave; // Wave_ID -> StageWaveValue
            }

            Stage.WaveValueDict = newDict;

            StageDict[i.ToString()] = Stage;
        }

        for (int i = 1; i <= MaxStage; i++)
        {
            StageNameDict[i] = DataTable.StageInfo.StageInfoMap[i-1].StageName; // 스테이지 배경명
        }
    }
}
