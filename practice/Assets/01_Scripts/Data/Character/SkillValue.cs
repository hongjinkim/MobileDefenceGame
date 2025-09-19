using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SkillValue
{
    public string Hero_ID;
    public string Name;
    public string Description;
    public ESkillUpgradeTier Tier;
    public ESkillBehaviorType Behavior;
    public ESkillUpgradeType UpgradeType;
    public float Value;
}
