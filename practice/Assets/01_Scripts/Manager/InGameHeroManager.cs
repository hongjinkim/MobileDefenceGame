using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameHeroManager : BasicSingleton<InGameHeroManager>
{
    [SerializeField] private List<HeroControl> SummonedHeroList;

    public static HeroControl FindNearTarget(Vector3 pos)
    {
        HeroControl select = Instance.SummonedHeroList[0]; //메인 히어로
        var distance = Vector2.Distance(pos, Instance.SummonedHeroList[0].CenterPoint.position);

        HeroControl compare;
        int count = Instance.SummonedHeroList.Count;
        for (int i = 1; i < count; i++)
        {
            compare = Instance.SummonedHeroList[i];
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

    public static bool IsHeroActive() => Instance.SummonedHeroList.Exists(hero => hero.IsHeroActive());
  

    //소환처리
    public void SummonHero()
    {
        var newHero = HeroPoolManager.Instance.Pop(EPoolType.Hero);
        var heroComp = newHero.GetComponent<HeroControl>();

        heroComp.Init(SummonedHeroList.Count);

        // 영웅 위치 및 인덱스 설정
        heroComp.transform.position =  PositionInfo.Instance.HeroPos[SummonedHeroList.Count].position;


        
        SummonedHeroList.Add(heroComp);
    }
}
