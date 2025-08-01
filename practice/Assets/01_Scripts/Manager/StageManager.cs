
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StageManager : BasicSingleton<StageManager>
{
    [ReadOnly] public int currentStage;
    [ReadOnly] public int level;
    [ReadOnly] public int exp;

    [ShowInInspector]
    private StageValue stageValue;

    public SkillChoiceUI skillChoiceUI;

    private Queue<LevelUpRequest> levelUpQueue = new Queue<LevelUpRequest>();
    private bool isLevelUpChoiceActive = false;

    public void StageStart()
    {
        InitStage();
        // 게임 시작시 2번 연속 선택 처리
        RequestLevelUp();
        RequestLevelUp();
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

    public void RequestLevelUp()
    {
        levelUpQueue.Enqueue(new LevelUpRequest());
        TryStartNextLevelUp(); // 이름을 명확하게 바꿔봄
    }

    private void TryStartNextLevelUp()
    {
        // 오직 선택이 진행중이 아닐 때만 시작!
        if (isLevelUpChoiceActive || levelUpQueue.Count == 0)
            return;

        isLevelUpChoiceActive = true;
        Time.timeScale = 0f;

        var choices = GenerateSkillChoices();
        skillChoiceUI.Show(choices, OnLevelUpChoiceMade);
    }

    private void OnLevelUpChoiceMade(SkillUpgradeValue chosen)
    {
        ApplyUpgrade(chosen);
        Time.timeScale = 1f;
        isLevelUpChoiceActive = false;
        levelUpQueue.Dequeue();

        // 선택이 끝난 직후 바로 다음 큐가 남아있으면 진행
        TryStartNextLevelUp();
    }

    // 실제 강화/영웅 소환 적용
    private void ApplyUpgrade(SkillUpgradeValue selected)
    {
        if (selected.Tier == ESkillUpgradeTier.Summon)
            InGameHeroManager.Instance.HeroSummonedIDs.Add(selected.ID);
        else
            /* 강화 적용 로직 */
            ;
    }

    // 선택지 3개 생성
    private List<SkillUpgradeValue> GenerateSkillChoices()
    {
        var result = new List<SkillUpgradeValue>();
        var summonCandidates = InGameHeroManager.Instance.allSkillUpgrades
            .FindAll(x => x.Tier == ESkillUpgradeTier.Summon && !InGameHeroManager.Instance.HeroSummonedIDs.Contains(x.ID));

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
                .FindAll(x => InGameHeroManager.Instance.HeroSummonedIDs.Contains(x.ID) /* or 기타 조건 */);

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
