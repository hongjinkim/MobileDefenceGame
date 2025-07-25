using Sirenix.OdinInspector;
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
    [ShowInInspector, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout, KeyLabel = "Stage ID", ValueLabel = "Info")]
    public Dictionary<int, StageValue> StageDict = new Dictionary<int, StageValue>();
    [ShowInInspector, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout, KeyLabel = "Wave ID", ValueLabel = "Info")]
    public Dictionary<int, StageWaveValue> WaveValueDict = new Dictionary<int, StageWaveValue>(); //몬스터 등장정보

    public Dictionary<int, string> StageNameDict = new Dictionary<int, string>(); // 스테이지 배경명
                                                                                  //private int mapCount = 0;

    public StageData()
    {
        LoadData();
    }   

    private void LoadData()
    {
        MaxStage = 2;//DataTable.스테이지.스테이지List.Max(stage => stage.스테이지_스테이지ID);

        for (int i = 1; i <= MaxStage; i++)
        {
            StageValue Stage = new StageValue(i);
            var EnemyDict = DataTable.ENEMY.GetDictionary();

            // 스테이지 적 스탯
            // 적 체력
            Stage.EnemyInfo.EnemyHP.Start = DataTable.ENEMY.ENEMYMap["Start"].Monster_HP;
            Stage.EnemyInfo.EnemyHP.Constant = DataTable.ENEMY.ENEMYMap["Constant"].Monster_HP;
            Stage.EnemyInfo.EnemyHP.Exponent = (double)DataTable.ENEMY.ENEMYMap["Exponent"].Monster_HP;
            Stage.EnemyInfo.EnemyHP.SetEnemyStat(i);

            // 적 공격력
            Stage.EnemyInfo.EnemyAttack.Start = DataTable.ENEMY.ENEMYMap["Start"].Monster_Attack;
            Stage.EnemyInfo.EnemyAttack.Constant = DataTable.ENEMY.ENEMYMap["Constant"].Monster_Attack;
            Stage.EnemyInfo.EnemyAttack.Exponent = (double)DataTable.ENEMY.ENEMYMap["Exponent"].Monster_Attack;
            Stage.EnemyInfo.EnemyAttack.SetEnemyStat(i);

            // 적 골드
            Stage.EnemyInfo.EnemyGold.Start = DataTable.ENEMY.ENEMYMap["Start"].Monster_GoldDrop;
            Stage.EnemyInfo.EnemyGold.Constant = DataTable.ENEMY.ENEMYMap["Start"].Monster_GoldDrop;
            Stage.EnemyInfo.EnemyGold.Exponent = (double)DataTable.ENEMY.ENEMYMap["Exponent"].Monster_GoldDrop;
            Stage.EnemyInfo.EnemyGold.SetEnemyStat(i);

            // 적 보스 공격력 배수
            Stage.EnemyInfo.BossAttackMultiplier.Start = DataTable.ENEMY.ENEMYMap["Start"].Boss_AttackMultiply;
            Stage.EnemyInfo.BossAttackMultiplier.Constant = DataTable.ENEMY.ENEMYMap["Start"].Boss_AttackMultiply;
            Stage.EnemyInfo.BossAttackMultiplier.Exponent = (double)DataTable.ENEMY.ENEMYMap["Start"].Boss_AttackMultiply;
            Stage.EnemyInfo.BossAttackMultiplier.SetEnemyStat(i);

            // 적 보스 체력 배수
            Stage.EnemyInfo.BossHPMultiplier.Start = DataTable.ENEMY.ENEMYMap["Start"].Boss_HPMultiply;
            Stage.EnemyInfo.BossHPMultiplier.Constant = DataTable.ENEMY.ENEMYMap["Start"].Boss_HPMultiply;
            Stage.EnemyInfo.BossHPMultiplier.Exponent = (double)DataTable.ENEMY.ENEMYMap["Start"].Boss_HPMultiply;
            Stage.EnemyInfo.BossHPMultiplier.SetEnemyStat(i);

            // 웨이브 정보
            var lastWave = DataTable.STAGE.STAGEList.Max(wave => wave.Wave_ID);
            for(int j = 1; j <= lastWave; j++)
            {
                var waves = DataTable.STAGE.STAGEList.Where(wave => wave.Wave_ID == i).ToList();
                foreach (var wave in waves)
                {
                    WaveValueDict[j] = new StageWaveValue
                    {
                        ID = wave.Wave_ID
                    };
                    var enemySpawnData = new EnemySpawnData
                    {
                        EnemyID = wave.Wave_EnemyID,
                        SpawnCount = wave.Wave_EnemyCount,
                        SpawnDelay = wave.Wave_Delay
                    };
                    WaveValueDict[j].spawnDatas.Add(enemySpawnData);
                 }
            }
            StageDict[i] = Stage;
        }
    }
}
