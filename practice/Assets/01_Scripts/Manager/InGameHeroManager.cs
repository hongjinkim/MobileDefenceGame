using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameHeroManager : BasicSingleton<InGameHeroManager>
{
    [SerializeField] private Transform CenterPosition;
    public List<HeroControl> HeroList;

    public static HeroControl FindNearTarget(Vector3 pos)
    {
        HeroControl select = Instance.HeroList[0]; //메인 히어로
        var distance = Vector2.Distance(pos, Instance.HeroList[0].CenterPoint.position);

        HeroControl compare;
        int count = Instance.HeroList.Count;
        for (int i = 1; i < count; i++)
        {
            compare = Instance.HeroList[i];
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

    public static bool IsHeroActive() => Instance.HeroList.Exists(hero => hero.IsHeroActive());
  

    //소환처리
    private void SummonHero()
    {
       
    }
}
