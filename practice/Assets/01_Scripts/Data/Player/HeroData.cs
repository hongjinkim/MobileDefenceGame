using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            heroData.Grade = heroList[i].Hero_Grade;
            heroData.Name = heroList[i].Hero_Name;
            heroData.Description = heroList[i].Hero_Description;
            heroData.AttackPower = heroList[i].Initial_Attack;
            heroData.Health = heroList[i].Initial_Health;
            heroData.AttackSpeed = heroList[i].AttackSpeed;
            //HeroIcon = Resources.Load<Sprite>($"Icons/Heroes/{hero.아이콘}");

            heroData.SkillUpgradeDict = new Dictionary<string, SkillUpgradeValue>();
            var skillID = heroList[i].Skill_ID;
            var skillUpgradeList = DataTable.SkillUpgrade.SkillUpgradeList.FindAll(skill => skill.Skill_ID == skillID).ToList();

            foreach(var skillUpgrade in skillUpgradeList)
            {
                var value = new SkillUpgradeValue();

                value.HeroID = heroList[i].Hero_ID;
                value.SkillID = skillUpgrade.Upgrade_Name;
                value.Description = skillUpgrade.Upgrade_Descption;
                value.Tier = skillUpgrade.Upgrade_Tier;
                value.Behavior = skillUpgrade.Skill_Behavior;
                value.value = skillUpgrade.Upgrade_Value;


                heroData.SkillUpgradeDict[skillUpgrade.Upgrade_Name] = value;
                
            }



            HeroDict.Add(heroList[i].Hero_ID, heroData);
        }
    }
}
