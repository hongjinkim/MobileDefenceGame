#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using Sirenix.Serialization;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeroData
{
    [ShowInInspector, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout, KeyLabel = "Hero ID", ValueLabel = "Info")]
    public Dictionary<string, HeroValue> HeroDict = new Dictionary<string, HeroValue>();

    private int heroCount;


    public void LoadData()
    {
        var heroList = DataTable.Hero.HeroList;
        heroCount = heroList.Count;

        for (int i = 0; i < heroCount; i++)
        {
            var heroData = new HeroValue();
            heroData.ID = heroList[i].Hero_ID;
            heroData.Grade = heroList[i].Hero_Grade;
            heroData.Name = heroList[i].Hero_Name;
            heroData.Description = heroList[i].Hero_Description;
            //HeroIcon = Resources.Load<Sprite>($"Icons/Heroes/{hero.아이콘}");

            heroData.SkillUpgradeDict = new Dictionary<string, SkillUpgradeValue>();
            var skillID = heroList[i].Skill_ID;
            var SkillUpgradeList = DataTable.SkillUpgrade.SkillUpgradeList.FindAll(skill => skill.Skill_ID == skillID);

            HeroDict.Add(heroData.ID, heroData);
        }
    }
}
