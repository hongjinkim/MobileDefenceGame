using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerData
{
    //저장할 데이터 (서버에 주고 받을 데이터만 여기에 작성)
    [TabGroup("Tabs", "PlayerValue"), HideLabel][InlineProperty] public PlayerValue Value = new PlayerValue();
    [TabGroup("Tabs", "Hero"), HideLabel][InlineProperty] public PlayerHeroValue Hero = new PlayerHeroValue();

    // 데이터 로드, 추후 서버에서 받아오는 부분 구현 예정
    public void LoadData()
	{
		TestLoadPlayerValue();
		TestLoadPlayerHeroValue();
    }

	private void TestLoadPlayerValue()
	{
		// 테스트용 데이터 로드
		Value.MaxEnergy = 30;
		Value.CurrentEnergy = 5;
		Value.Gold = 1000;
		Value.Crystal = 200;
    }
	private void TestLoadPlayerHeroValue()
	{
		var master = DataBase.GetHeroData();

        // 1) 도감(컬렉션) 키 보장 (마스터 전체를 Hidden으로 초기화)
        Hero.EnsureCollectionKeys(master);

        // 2) 테스트용 도감 세팅: 마스터 전체를 Seen으로 설정
        Hero.SetAllSeen(master);

        // 3) 테스트용 보유 세팅: 마스터의 앞 3개를 획득한다고 가정
        var first3 = master.HeroDict.Keys.Take(3).ToList();
        if (first3.Count >= 1) Hero.Acquire(first3[0], level: 5, star: 1);
        if (first3.Count >= 2) Hero.Acquire(first3[1], level: 10, star: 2);
        if (first3.Count >= 3) Hero.Acquire(first3[2], level: 1, star: 3);

        // 4) 덱 구성(보유한 애들만, 중복 제거, 최대 5칸)
        Hero.SetDeck("Team1", first3, maxSize: 5, ownedOnly: true);
    }
}