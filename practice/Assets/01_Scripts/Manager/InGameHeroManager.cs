using DataTable;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class InGameHeroManager : BasicSingleton<InGameHeroManager>
{
    [SerializeField] private GameObject heroPrefab;
    public List<HeroControl> HeroDeckList;
    public List<string> HeroSummonedIDs;

    public List<SkillUpgradeValue> allSkillUpgrades;
    

    //public SkillChoiceUI skillChoiceUI;

    private int currentSummonedHeroCount = 0;

    public static HeroControl FindNearTarget(Vector3 pos)
    {
        HeroControl select = Instance.HeroDeckList[0]; //메인 히어로
        var distance = Vector2.Distance(pos, Instance.HeroDeckList[0].CenterPoint.position);

        HeroControl compare;
        int count = Instance.HeroDeckList.Count;
        for (int i = 1; i < count; i++)
        {
            compare = Instance.HeroDeckList[i];
            if (compare.IsHeroActive()) //살아있는 경우만 서치 포함
            {
                var HeroDistance = Vector2.Distance(pos, compare.CenterPoint.position);
                if (HeroDistance < distance)
                {
                    distance = HeroDistance;
                    select = compare;
                }
            }
        }
        return select;
    }

    public static bool IsHeroActive() => Instance.HeroDeckList.Exists(hero => hero.IsHeroActive());
  
    public void InstantiateHero()
    {
        foreach(var pair in PlayerManager.Instance.HeroDeck)
        {
            var heroId = pair.Value;

            var newHero = Instantiate(heroPrefab);
            var heroComp = newHero.GetComponent<HeroControl>();
            heroComp.ID = heroId;
            heroComp.LoadData();

            foreach(var keyValue in heroComp.Value.SkillUpgradeDict)
            {
                allSkillUpgrades.Add(keyValue.Value);
            }

            
            HeroDeckList.Add(heroComp);
            newHero.SetActive(false);
            
        }
    }

    //소환처리
    public void SummonHero(int idx)
    {
        if (HeroDeckList[idx].gameObject.activeSelf)
        {
            Debug.Log("Hero Already Summoned");
            return;
        }

        var hero = HeroDeckList[idx];
        hero.transform.position = PositionInfo.Instance.HeroPos[currentSummonedHeroCount].position;
        currentSummonedHeroCount++;
        hero.Init();
        HeroSummonedIDs.Add(hero.ID);

        hero.gameObject.SetActive(true);
    }

    public void SummonHero(string ID)
    {
        var summonHero = HeroDeckList.Find(x => x.ID == ID);

        if (summonHero.gameObject.activeSelf)
        {
            Debug.Log("Hero Already Summoned");
            return;
        }

        if (summonHero == null)
        {
            Debug.LogError($"Hero with ID {ID} not found in deck.");
            return;
        }

        summonHero.transform.position = PositionInfo.Instance.HeroPos[currentSummonedHeroCount].position;
        currentSummonedHeroCount++;
        summonHero.Init();
        HeroSummonedIDs.Add(summonHero.ID);

        summonHero.gameObject.SetActive(true);
    }
}
