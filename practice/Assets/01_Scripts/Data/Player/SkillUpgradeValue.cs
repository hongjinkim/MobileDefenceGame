using System.Collections;
using UnityEngine;

public class SkillUpgradeValue
{
    public string ID; // 스킬 업그레이드 ID
    public string SkillID; // 스킬 ID
    public int UpgradeLevel; // 업그레이드 레벨
    public string Description; // 업그레이드 설명
    public float UpgradeValue; // 업그레이드 값 (예: 공격력 증가, 방어력 증가 등)
    public float Cost; // 업그레이드 비용 (예: 골드, 자원 등)
    public SkillUpgradeValue(string id, string skillId, int upgradeLevel, string description, float upgradeValue, float cost)
    {
        ID = id;
        SkillID = skillId;
        UpgradeLevel = upgradeLevel;
        Description = description;
        UpgradeValue = upgradeValue;
        Cost = cost;
    }
}
