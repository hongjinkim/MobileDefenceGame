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
        var heroList = DataTable.Hero.GetList();
        heroCount = heroList.Count;
        var skillList = DataTable.Skill.GetList();


        for (int i = 0; i < heroCount; i++)
        {
            var heroData = new HeroValue();
            heroData.ID = heroList[i].Hero_ID;
            heroData.Grade = heroList[i].Hero_Grade;
            heroData.Name = heroList[i].Hero_Name;
            heroData.Description = heroList[i].Hero_Description;
            heroData.HeroClass = heroList[i].Hero_Class;
            heroData.Element = heroList[i].Hero_Element;
            heroData.AttackPower = heroList[i].Initial_Attack;
            heroData.Health = heroList[i].Initial_Health;
            heroData.AttackSpeed = DataTable.Initial.InitialList[0].AttackSpeed;
            //HeroIcon = Resources.Load<Sprite>($"Icons/Heroes/{hero.아이콘}");

            heroData.SkillDict = new Dictionary<string, SkillValue>();
            var skillID = heroList[i].Skill_ID;
            

            foreach(var skill in skillList)
            {
                var value = new SkillValue();

                value.Hero_ID = heroList[i].Hero_ID;
                value.Name = skill.Skill_Name;
                value.Description = skill.Skill_Description;
                value.Tier = skill.Skill_Tier;
                value.CanDuplicate = skill.CanDuplicate;

                heroData.SkillDict[skill.Skill_Name] = value;
                
            }



            HeroDict.Add(heroList[i].Hero_ID, heroData);
        }
    }
}
