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
    public int CurrentStage;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Start()
    {
        TestDeck();
    }

    public void GainExp(BigNum GetExp)
    {
        
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
        if (/*에너지 체크*/ true)
        {
            UIManager.Instance.StartStageTransition();
        }
        else
        {
            Debug.LogWarning("에너지가 부족하여 스테이지를 시작할 수 없습니다.");
        }
    }

}

