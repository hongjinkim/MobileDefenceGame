using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.AI;



// 유저 정보 관리
public class PlayerManager : BasicSingleton<PlayerManager>
{
    [ShowInInspector, DictionaryDrawerSettings(KeyLabel = "Order", ValueLabel = "Hero ID")]
    public Dictionary<int, string> HeroDeck = new Dictionary<int, string>();

    public int CurrentStage = 1; // 추후 서버에서 진행상황 받아서 설정

    public PlayerData playerData => GameDataManager.PlayerData;


    private void Start()
    {
        TestDeck();
    }

    public void GainExp(BigNum GetExp)
    {
        
    }

    public void ProceedToNextStage()
    {
        CurrentStage++;
        EventManager.Raise(EEventType.StageCleared);
    }

    private void TestDeck()
    {
        HeroDeck[0] = "HERO_001";
        HeroDeck[1] = "HERO_002";
        HeroDeck[2] = "HERO_003";
        HeroDeck[3] = "HERO_004";
        HeroDeck[4] = "HERO_005";
    }

    public static void CheckEnergyToStart()
    {
        // 에너지가 충분한지 확인하고, 충분하다면 스테이지 시작
        if (GameDataManager.PlayerData.Value.CurrentEnergy >= 5)
        {
            GameDataManager.PlayerData.Value.UpdateEnergy(-5); // 에너지 소모
            UIManager.Instance.StartStageTransition();
        }
        else
        {
            ToastManager.Show("에너지가 충분하지 않습니다", 2.0f, ToastPosition.Center);
        }
    }

}

