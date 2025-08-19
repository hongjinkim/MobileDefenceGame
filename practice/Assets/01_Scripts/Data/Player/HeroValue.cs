using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeroValue
{
    public string Name;
    public string Description;
    public EGrade Grade;
    public BigNum AttackPower;
    public BigNum Health;
    public float AttackSpeed;

    [ShowInInspector, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout, KeyLabel = "Upgrade ID", ValueLabel = "Info")]
    public Dictionary<string, SkillUpgradeValue> SkillUpgradeDict = new Dictionary<string, SkillUpgradeValue>();
}
