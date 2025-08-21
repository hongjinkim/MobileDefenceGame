using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
	//저장할 데이터 (서버에 주고 받을 데이터만 여기에 작성)
	public PlayerValue Value = new PlayerValue();
	public PlayerHeroValue Hero = new PlayerHeroValue();


    public void LoadData()
	{
		TestLoadPlayerValue();
		TestLoadPlayerValue();
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

    }
}