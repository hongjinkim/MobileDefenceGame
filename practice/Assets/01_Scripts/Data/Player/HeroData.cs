using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class HeroData
{
    public Dictionary<int, HeroValue> HeroDict = new Dictionary<int, HeroValue>();

    private int heroCount;

    public HeroData()
    {
        LoadData();
    }

    private void LoadData()
    {
        var heroList = DataTable.영웅.영웅List;
        heroCount = heroList.Count;

        for (int i = 0; i < heroCount; i++)
        {
            var heroData = new HeroValue();
            heroData.ID = heroList[i].영웅_ID;
            heroData.Grade = heroList[i].영웅_희귀도;
            heroData.Name = heroList[i].영웅_이름;
            heroData.Description = heroList[i].영웅_설명;
            //HeroIcon = Resources.Load<Sprite>($"Icons/Heroes/{hero.아이콘}");

            HeroDict.Add(heroData.ID, heroData);
        }
    }
}
