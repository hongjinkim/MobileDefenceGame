using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeroData
{
    [ShowInInspector, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout, KeyLabel = "Hero ID", ValueLabel = "Info")]
    public Dictionary<string, HeroValue> HeroDict = new Dictionary<string, HeroValue>();

    private int heroCount;

    public HeroData()
    {
        LoadData();
    }   

    private void LoadData()
    {
        var heroList = DataTable.HERO.HEROList;
        heroCount = heroList.Count;

        for (int i = 0; i < heroCount; i++)
        {
            var heroData = new HeroValue();
            heroData.ID = heroList[i].Hero_ID;
            heroData.Grade = heroList[i].Hero_Grade;
            heroData.Name = heroList[i].Hero_Name;
            heroData.Description = heroList[i].Hero_Description;
            //HeroIcon = Resources.Load<Sprite>($"Icons/Heroes/{hero.아이콘}");

            HeroDict.Add(heroData.ID, heroData);
        }
    }
}
