
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageManager : BasicSingleton<StageManager>
{
    [ReadOnly] public int currentStage;
    [ReadOnly] public int level;
    [ReadOnly] public int exp;

    [ShowInInspector]
    private StageValue stageValue;

    public SkillChoiceUI skillChoiceUI;

    private bool _spawnStarted = false;
    private bool _choiceConsumed = false;   // 이번 선택 1회만 허용

    private Queue<LevelUpRequest> levelUpQueue = new Queue<LevelUpRequest>();
    private bool isLevelUpFlowActive = false;

    public void StageStart()
    {
        InitStage();
        _spawnStarted = false;
        StartCoroutine(StartAfterTwoLevelUpsOnce());
    }

    private void InitStage()
    {
        InGameHeroManager.Instance.InstantiateHero();

        currentStage = PlayerManager.Instance.currentStage;
        Debug.Log($"stage {currentStage} started");
        if (DataBase.TryGetStageValue(currentStage, out var value))
        {
            stageValue = value;
        }
        else
        {
            Debug.Log($"id : {currentStage}에 해당하는 데이터를 불러오는데 실패");
        }

        level = 1;
        exp = 0;
    }

    private IEnumerator StartAfterTwoLevelUpsOnce()
    {
        if (_spawnStarted) yield break;

        RequestLevelUp();
        RequestLevelUp();

        // 이번 플로우가 실제 시작될 때까지 대기
        yield return new WaitUntil(() => isLevelUpFlowActive);
        // 그리고 끝날 때까지 대기
        yield return new WaitUntil(() => !isLevelUpFlowActive);

        if (_spawnStarted) yield break;
        _spawnStarted = true;

        yield return StartCoroutine(StageSpawn());
    }

    private IEnumerator StageSpawn()
    {
        // Wave ID 오름차순
        foreach (var kv in stageValue.WaveValueDict)
        {
            var waveId = kv.Key;
            var wave = kv.Value;

            yield return StartCoroutine(RunOneWave(waveId, wave.SpawnDatas));
        }
    }

    private IEnumerator RunOneWave(int waveId, List<EnemySpawnData> waveList)
    {
        Debug.Log($"Wave {waveId} started, count:{waveList?.Count}");

        // 불변 리스트로 복사해서 안전하게 순회
        var list = new List<EnemySpawnData>(waveList ?? new List<EnemySpawnData>());

        for (int i = 0; i < list.Count; i++)
        {
            var sd = list[i];
            Debug.Log($"[Wave {waveId}] {i}/{list.Count - 1} Spawn start: id={sd.EnemyID}, count={sd.SpawnCount}, delay={sd.SpawnDelay}");

            // fire-and-forget 스타일 (SpawnEnemy가 즉시 리턴하는 버전)
            EnemyManager.Instance.SpawnEnemy(sd.SpawnPattern, sd.SpawnCount, sd.EnemyID);

            // 혹시 EnemyManager가 busy 플래그로 중복 호출을 무시한다면 한 프레임 양보해보세요
            yield return null;

            if (sd.SpawnDelay > 0f)
                yield return new WaitForSecondsRealtime(sd.SpawnDelay);
        }

        Debug.Log($"Wave {waveId} done.");
    }

    public void RequestLevelUp()
    {
        levelUpQueue.Enqueue(new LevelUpRequest());
        if (!isLevelUpFlowActive)
            StartLevelUpFlow();
    }

    private void StartLevelUpFlow()
    {
        if (levelUpQueue.Count == 0) return;

        isLevelUpFlowActive = true;
        Time.timeScale = 0f;

        ShowNextSelection();
    }

    private void ShowNextSelection()
    {
        if (levelUpQueue.Count == 0)
        {
            EndLevelUpFlow();
            return;
        }

        _choiceConsumed = false;            // 새 창을 띄울 때마다 리셋

        // 여기서는 Dequeue 하지 않음! 선택 완료 시점에 Dequeue
        var choices = GenerateSkillChoices();
        skillChoiceUI.Show(choices, OnLevelUpChoiceMade);
    }

    private void OnLevelUpChoiceMade(SkillUpgradeValue chosen)
    {
        // 동일 프레임/중복 클릭 방지
        if (_choiceConsumed) return;
        _choiceConsumed = true;

        ApplyUpgrade(chosen);
        Debug.Log($"{chosen.HeroID} 에 {chosen.SkillID} 적용됨");

        // 이번 요청 1개 소모
        if (levelUpQueue.Count > 0)
            levelUpQueue.Dequeue();

        // UI 닫힘 애니메이션이 있다면 unscaled로 돌리거나, 닫힘 완료를 기다린 뒤 다음 Show
        StartCoroutine(ShowNextAfterUIClose());
    }

    private IEnumerator ShowNextAfterUIClose()
    {
        // SkillChoiceUI에 IsClosed가 있다면 그걸 기다리는 게 최고
        // 없으면 최소 한 프레임은 비워서 겹침/레이아웃 꼬임 방지
        // yield return new WaitUntil(() => skillChoiceUI.IsClosed);
        yield return null; // 한 프레임 대기 (unscaled)

        ShowNextSelection();
    }

    private void EndLevelUpFlow()
    {
        isLevelUpFlowActive = false;
        Time.timeScale = 1f;

    }


    // 실제 강화/영웅 소환 적용
    private void ApplyUpgrade(SkillUpgradeValue selected)
    {
        if (selected.Tier == ESkillUpgradeTier.Summon)
            InGameHeroManager.Instance.SummonHero(selected.HeroID);
        else
            /* 강화 적용 로직 */

            Debug.Log($"Applied upgrade: {selected.SkillID} on Hero: {selected.HeroID}");
            ;
    }

    // 선택지 3개 생성
    private List<SkillUpgradeValue> GenerateSkillChoices()
    {
        var result = new List<SkillUpgradeValue>();
        var summonCandidates = InGameHeroManager.Instance.allSkillUpgrades
            .FindAll(x => x.Tier == ESkillUpgradeTier.Summon && !InGameHeroManager.Instance.HeroSummonedIDs.Contains(x.HeroID));

        // 미소환 영웅 우선 3개까지
        var summonShuffle = new List<SkillUpgradeValue>(summonCandidates);
        Shuffle(summonShuffle);
        for (int i = 0; i < 3 && i < summonShuffle.Count; i++)
            result.Add(summonShuffle[i]);

        // 부족하면 강화로
        if (result.Count < 3)
        {
            var otherCandidates = InGameHeroManager.Instance.allSkillUpgrades
                .FindAll(x => x.Tier != ESkillUpgradeTier.Summon)
                .FindAll(x => InGameHeroManager.Instance.HeroSummonedIDs.Contains(x.HeroID) /* or 기타 조건 */);

            Shuffle(otherCandidates);

            int n = 0;
            while (result.Count < 3 && n < otherCandidates.Count)
                result.Add(otherCandidates[n++]);
        }
        return result;
    }

    // Fisher-Yates Shuffle
    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T t = list[i];
            list[i] = list[j];
            list[j] = t;
        }
    }

    // 외부에서 여러 번 호출할 수도 있음
    public void OnPlayerLevelUp()
    {
        RequestLevelUp();
    }

    // 선택지 타입 등 확장할 때 아래 구조 활용 가능
    public class LevelUpRequest { /* 확장 가능: 어떤 원인으로 레벨업? 등 정보 추가 가능 */ }
}
