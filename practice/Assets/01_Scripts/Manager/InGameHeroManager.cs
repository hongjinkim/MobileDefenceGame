using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameHeroManager : BasicSingleton<InGameHeroManager>
{
    [SerializeField] private GameObject heroPrefab;
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
    public void SummonHero(int idx)
    {
        var newHero = Instantiate(heroPrefab);
        var heroComp = newHero.GetComponent<HeroControl>();

        if(PlayerManager.Instance.HeroDeck.TryGetValue(idx, out var id))
        {
            string heroID = id;
            heroComp.Init(heroID, SummonedHeroList.Count);

            // 영웅 위치 및 인덱스 설정
            heroComp.transform.position = PositionInfo.Instance.HeroPos[SummonedHeroList.Count].position;

            SummonedHeroList.Add(heroComp);
        }
        else
        {
            Debug.Log($"현재 덱에 {idx}번째에 할당된 영웅이 없음.");
        }
    }
}
