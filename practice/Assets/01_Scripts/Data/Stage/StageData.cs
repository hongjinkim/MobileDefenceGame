using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    //public StageData()
    //{
    //    LoadData();
    //}

    //private void LoadData()
    //{
    //    var stageData = DataTable.스테이지.스테이지List;

    //    MaxStage = stageData[0].스테이지_최대스테이지;

    //    for (int i = 0; i < this.MaxStage; i++)
    //    {
    //        int StageNum = i + 1;
    //        StageValue Stage = new StageValue(i);

    //        foreach(var wave in Stage.Waves)
    //        {
    //            wave.SpawnPattern = 
    //            wave.EnemyValue.EnemyHP.Start = 
    //        }

    //        // 적 체력
    //        Stage.EnemyInfo.EnemyHP.Start = m적.FindEntity(x => x.f구분 == "적스탯_시작값").f적_체력;
    //        Stage.EnemyInfo.EnemyHP.Constant = m적.FindEntity(x => x.f구분 == "적스탯_상수곱").f적_체력;
    //        Stage.EnemyInfo.EnemyHP.Exponent = m적.FindEntity(x => x.f구분 == "적스탯_지수곱").f적_체력;
    //        Stage.EnemyInfo.EnemyHP.SetEnemyStat(StageNum);

    //        // 적 공격력
    //        Stage.EnemyInfo.EnemyAttack.Start = m적.FindEntity(x => x.f구분 == "적스탯_시작값").f적_공격력;
    //        Stage.EnemyInfo.EnemyAttack.Constant = m적.FindEntity(x => x.f구분 == "적스탯_상수곱").f적_공격력;
    //        Stage.EnemyInfo.EnemyAttack.Exponent = m적.FindEntity(x => x.f구분 == "적스탯_지수곱").f적_공격력;
    //        Stage.EnemyInfo.EnemyAttack.SetEnemyStat(StageNum);

    //        // 적 골드
    //        Stage.EnemyInfo.EnemyGold.Start = m적.FindEntity(x => x.f구분 == "적스탯_시작값").f적_골드드랍;
    //        Stage.EnemyInfo.EnemyGold.Constant = m적.FindEntity(x => x.f구분 == "적스탯_상수곱").f적_골드드랍;
    //        Stage.EnemyInfo.EnemyGold.Exponent = m적.FindEntity(x => x.f구분 == "적스탯_지수곱").f적_골드드랍;
    //        Stage.EnemyInfo.EnemyGold.SetEnemyStat(StageNum);

    //        // 적 보스 공격력 배수
    //        Stage.EnemyInfo.BossAttackMultiplier.Start = m적.FindEntity(x => x.f구분 == "적스탯_시작값").f보스공격력배수;
    //        Stage.EnemyInfo.BossAttackMultiplier.Constant = m적.FindEntity(x => x.f구분 == "적스탯_상수곱").f보스공격력배수;
    //        Stage.EnemyInfo.BossAttackMultiplier.Exponent = m적.FindEntity(x => x.f구분 == "적스탯_지수곱").f보스공격력배수;
    //        Stage.EnemyInfo.BossAttackMultiplier.SetEnemyStat(StageNum);

    //        // 적 보스 체력 배수
    //        Stage.EnemyInfo.BossHPMultiplier.Start = m적.FindEntity(x => x.f구분 == "적스탯_시작값").f보스체력배수;
    //        Stage.EnemyInfo.BossHPMultiplier.Constant = m적.FindEntity(x => x.f구분 == "적스탯_상수곱").f보스체력배수;
    //        Stage.EnemyInfo.BossHPMultiplier.Exponent = m적.FindEntity(x => x.f구분 == "적스탯_지수곱").f보스체력배수;
    //        Stage.EnemyInfo.BossHPMultiplier.SetEnemyStat(StageNum);

    //        StageDict[StageNum] = Stage;
    //    }

    //    //반복스테이지 몬스터 정보
    //    for (int i = 0; i < 20; i++)
    //    {
    //        int StageNum = m스테이지.FindEntity(x => x.f스테이지_반복 == (i + 1)).f스테이지_반복;
    //        StageMonValue Stage = new StageMonValue(StageNum);

    //        float TotalSum = m스테이지.FindEntity(x => x.f스테이지_반복 == StageNum).f비중합;
    //        Stage.MonsterRatio[0] = m스테이지.FindEntity(x => x.f스테이지_반복 == StageNum).f적0_등장비중 / TotalSum;
    //        Stage.MonsterRatio[1] = m스테이지.FindEntity(x => x.f스테이지_반복 == StageNum).f적1_등장비중 / TotalSum;
    //        Stage.MonsterRatio[2] = m스테이지.FindEntity(x => x.f스테이지_반복 == StageNum).f적2_등장비중 / TotalSum;
    //        Stage.MonsterRatio[3] = m스테이지.FindEntity(x => x.f스테이지_반복 == StageNum).f적3_등장비중 / TotalSum;
    //        Stage.BossIndex = (StageNum - 1) % 4;

    //        StageMonDict[StageNum] = Stage;
    //    }

    //    // 배경명
    //    for (int i = 0; i < MapNumber; i++)
    //    {
    //        StageNameDict[i] = m스테이지.FindEntity(x => x.f배경index == i).f배경명;
    //    }
    //}

    //// 보스 시간 반환
    //public float GetBossStageTime() => BossStageTime * (1 + StatManager.Instance.FinalStageBossTimerIncrease);

    //// 해당 스테이지의 몬스터 수 반환
    //public int GetStageEnemyCount(int stage)
    //{
    //    int count = StageEnemyCountStart + Mathf.FloorToInt((float)stage / StageEnemyCountUnit) * StageEnemyCountRaise;
    //    count = Mathf.Clamp(count, 0, StageEnemyCountMax);
    //    return count;
    //}

    //// 스테이지 난이도 키값 반환
    //public string GetStageDifficultyKey(int stage) => $"DIFFICULTY_{(stage - 1) / (MapNumber * 20):0}";

    //public int GetStageDifficultyIndex(int stage) => (stage - 1) / (MapNumber * 20);

    //// 스테이지 배경 맵 반환
    //public string GetStageName(int stage) => StageNameDict[GetStageBackgroundIndex(stage)];

    ////스테이지 배경index 반환
    //public int GetStageBackgroundIndex(int stage) => ((stage - 1) / 20) % 10;

    ////스테이지 몬스터확률 반환
    //public float GetStageMonRatio(int stage, int index) => StageMonDict[stageRepeat(stage)].MonsterRatio[index];

    ////스테이지 보스 구분index 반환
    //public int GetBossIndex(int stage) => StageMonDict[stageRepeat(stage)].BossIndex;
    //public int GetBossBoxRewardAmount(int stage)
    //{
    //    //int raiseRatio = Mathf.Max(0, (stage-1) / BossBox_StageUnit);
    //    //int amount = (int)(60*(BossBox_Start + raiseRatio * BossBox_Raise));
    //    int amount = (int)(BossBox_Start + BossBox_Raise * (stage - 1));
    //    //Debug.Log($"Boss Box Reward : {amount}");
    //    return amount;
    //}
    //public int GetNormalEnemyBoxRewardAmount(int stage, int bossReward)
    //{
    //    int enemyCount = DataManager.Instance.Stage.GetStageEnemyCount(stage);
    //    int quotient = bossReward / enemyCount;
    //    float ratio = (bossReward / (float)enemyCount) - quotient;
    //    //Debug.Log(enemyCount + " / " + quotient + " / " + ratio);
    //    if (Random.Range(0, 1f) < ratio)
    //    {
    //        return quotient + 1;
    //    }
    //    else
    //    {
    //        return quotient;
    //    }
    //}

    ////반복스테이지 index반환식
    //private int stageRepeat(int stage)
    //{
    //    int repeatIndex = stage % 20;
    //    if (repeatIndex == 0) { repeatIndex = 20; }
    //    return repeatIndex;
    //}
}
