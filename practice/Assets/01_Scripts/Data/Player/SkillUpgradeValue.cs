using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SkillUpgradeValue
{
    public string ID; // 스킬 업그레이드 ID
    public string Description; // 업그레이드 설명
    public ESkillUpgradeTier Tier;
    public ESkillBehaviorType Behavior;
    public float value;
}
