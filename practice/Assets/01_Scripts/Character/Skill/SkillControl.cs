using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무기 행동 정의
[System.Serializable]
public class SkillBehavior
{
    public SkillBehaviorType type;
    //public SkillAbility ability;
    public float value; // 추가 수치값이 필요한 경우
}

public enum SkillBehaviorType
{
    OnHit,
    OnCrit,
    OnKill,
    Passive,
    ActiveSkill
}

public enum SkillUpgradeType
{
    Acquisition,            // 처음 획득
    BasicEnhancement,       // 일반 강화
    AdvancedEnhancement,    // 고급 강화
    BreakthroughRequirement,// 돌파 조건
    Breakthrough            // 돌파
}

[System.Serializable]
public class SkillUpgradeData
{
    public SkillUpgradeType upgradeType; // 업그레이드 타입
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
