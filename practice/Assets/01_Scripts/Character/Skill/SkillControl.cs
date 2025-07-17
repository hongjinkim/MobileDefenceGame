using GoogleSheet.Core.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무기 행동 정의
[System.Serializable]
public class SkillBehavior
{
    public ESkillBehaviorType type;
    //public SkillAbility ability;
    public float value; // 추가 수치값이 필요한 경우
}


[UGS(typeof(ESkillBehaviorType))]
public enum ESkillBehaviorType
{
    None, // 행동 없음
    OnHit, // 적중 시 행동
    OnCrit, // 크리티컬 적중 시 행동
    OnKill, // 적 처치 시 행동
    Passive, // 지속적인 행동
    OnTimer // 일정 시간마다 행동
}

[UGS(typeof(ESkillUpgradeTier))]
public enum ESkillUpgradeTier // 스킬 업그레이드 단계
{
    Acquisition,            // 처음 획득
    Normal,       // 일반 강화
    Advanced,    // 고급 강화
    EvolveRequirement,// 돌파 조건
    Evolve            // 돌파
}

[UGS(typeof(ESkillUpgradeType))]
public enum ESkillUpgradeType // 스킬 업그레이드 타입
{ 
    DamageUP,          // 데미지 증가
    ProjectileUp,      // 투사체 개수 증가
    PierceUp,         // 관통 증가
    AreaUp,           // 범위 증가
}

[System.Serializable]
public class SkillUpgradeData
{
    public ESkillUpgradeTier upgradeType; // 업그레이드 타입
    public float damageMultiplier;
    public float speedMultiplier;
    public float rangeMultiplier;
    public SkillBehavior[] newBehaviors; // 새로운 행동들
    //public SkillBehavior[] modifiedBehaviors; // 기존 행동 수정
    public string upgradeDescription;
}

public class SkillControl : MonoBehaviour
{
    private List<SkillBehavior> behaviors = new List<SkillBehavior>();
    private Dictionary<string, SkillUpgradeData> upgradeDict = new Dictionary<string, SkillUpgradeData>();

    public void UpgradeSkill(string id)
    {
        if(!upgradeDict.ContainsKey(id))
        {
            Debug.LogError($"Skill with ID {id} not found.");
            return;
        }
        var upgradeData = upgradeDict[id];

        // 새로운 행동 추가
        foreach (var behavior in upgradeData.newBehaviors)
        {
            behaviors.Add(behavior);
        }

        // 기존 행동 수정
        //foreach (var modifiedBehavior in upgradeData.modifiedBehaviors)
        //{
        //    // 기존 행동 찾아서 교체 또는 수정
        //}
    }

    
}
