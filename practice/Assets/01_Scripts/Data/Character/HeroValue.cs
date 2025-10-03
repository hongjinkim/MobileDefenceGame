using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeroValue
{
    // ∞Ì¡§ Ω∫≈»
    public string ID;
    public string Name;
    public string Description;
    public EGrade Grade;
    public EHeroClassType HeroClass;
    public EElementType Element;
    public BigNum AttackPower;
    public BigNum Health;
    public float AttackSpeed;

    [ShowInInspector, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout, KeyLabel = "Skill ID", ValueLabel = "Info")]
    public Dictionary<string, SkillValue> SkillDict = new Dictionary<string, SkillValue>();
}
