﻿using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ChapterData
{
    /* 적 시트 */
    private int MaxStage { get; set; } // 스테이지 배경 맵 개수
    /* 스테이지 시트 */

    // 스테이지 정보 사전
    [ShowInInspector, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout, KeyLabel = "Chapter ID", ValueLabel = "Info")]
    public Dictionary<string, ChapterValue> StageDict = new Dictionary<string, ChapterValue>();
    [ShowInInspector, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout, KeyLabel = "Chapter ID", ValueLabel = "Chapter Name")]
    public Dictionary<int, string> StageNameDict = new Dictionary<int, string>(); // 스테이지 배경명
                                                                                  //private int mapCount = 0;


    public void LoadData()
    {
        MaxStage = DataTable.ChapterInfo.ChapterInfoList.Count;

        for (int i = 1; i <= MaxStage; i++)
        {
            ChapterValue Chapter = new ChapterValue();
            Chapter.StageNum = i;
            var EnemyDict = DataTable.EnemyStat.GetDictionary();

            // 스테이지 적 스탯
            // 적 체력
            Chapter.EnemyInfo.EnemyHP.Start = DataTable.EnemyStat.EnemyStatMap["Start"].Monster_HP;
            Chapter.EnemyInfo.EnemyHP.Constant = DataTable.EnemyStat.EnemyStatMap["Constant"].Monster_HP;
            Chapter.EnemyInfo.EnemyHP.Exponent = (double)DataTable.EnemyStat.EnemyStatMap["Exponent"].Monster_HP;
            Chapter.EnemyInfo.EnemyHP.SetEnemyStat(i);

            // 적 공격력
            Chapter.EnemyInfo.EnemyAttack.Start = DataTable.EnemyStat.EnemyStatMap["Start"].Monster_Attack;
            Chapter.EnemyInfo.EnemyAttack.Constant = DataTable.EnemyStat.EnemyStatMap["Constant"].Monster_Attack;
            Chapter.EnemyInfo.EnemyAttack.Exponent = (double)DataTable.EnemyStat.EnemyStatMap["Exponent"].Monster_Attack;
            Chapter.EnemyInfo.EnemyAttack.SetEnemyStat(i);

            // 적 골드
            Chapter.EnemyInfo.EnemyGold.Start = DataTable.EnemyStat.EnemyStatMap["Start"].Monster_GoldDrop;
            Chapter.EnemyInfo.EnemyGold.Constant = DataTable.EnemyStat.EnemyStatMap["Constant"].Monster_GoldDrop;
            Chapter.EnemyInfo.EnemyGold.Exponent = (double)DataTable.EnemyStat.EnemyStatMap["Exponent"].Monster_GoldDrop;
            Chapter.EnemyInfo.EnemyGold.SetEnemyStat(i);

            // 적 보스 공격력 배수
            Chapter.EnemyInfo.BossAttackMultiplier.Start = DataTable.EnemyStat.EnemyStatMap["Start"].Boss_AttackMultiply;
            Chapter.EnemyInfo.BossAttackMultiplier.Constant = DataTable.EnemyStat.EnemyStatMap["Constant"].Boss_AttackMultiply;
            Chapter.EnemyInfo.BossAttackMultiplier.Exponent = (double)DataTable.EnemyStat.EnemyStatMap["Exponent"].Boss_AttackMultiply;
            Chapter.EnemyInfo.BossAttackMultiplier.SetEnemyStat(i);

            // 적 보스 체력 배수
            Chapter.EnemyInfo.BossHPMultiplier.Start = DataTable.EnemyStat.EnemyStatMap["Start"].Boss_HPMultiply;
            Chapter.EnemyInfo.BossHPMultiplier.Constant = DataTable.EnemyStat.EnemyStatMap["Constant"].Boss_HPMultiply;
            Chapter.EnemyInfo.BossHPMultiplier.Exponent = (double)DataTable.EnemyStat.EnemyStatMap["Exponent"].Boss_HPMultiply;
            Chapter.EnemyInfo.BossHPMultiplier.SetEnemyStat(i);

            // 웨이브 정보
            var stageRows = DataTable.Chapter.ChapterList
            .Where(row => row.Chapter_ID == i)   // 스테이지 단위 필터
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

            Chapter.WaveValueDict = newDict;

            StageDict[i.ToString()] = Chapter;
        }

        for (int i = 1; i <= MaxStage; i++)
        {
            StageNameDict[i] = DataTable.ChapterInfo.ChapterInfoMap[i - 1].ChapterName; // 스테이지 배경명
        }
    }
}