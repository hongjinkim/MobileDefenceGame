using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class HeroUpgradeData
{
    public int Initial_MaxLevel;
    public int Initial_MaxPromotionLevel;
    public int MaxLevel;
    public int AttackPowerIncrease;
    public int HealthIncrease;
    public int Initial_UpgradeStoneCost;
    public float UpgradeStoneIncrease;
    public int Initial_UpgradeGoldCost;
    public float UpgradeGoldIncrease;

    public void LoadData()
    {
        var list = DataTable.Hero_Upgrade.Hero_UpgradeList;
        
        Initial_MaxLevel = list[0].Initial_MaxLevel;
        Initial_MaxPromotionLevel = list[0].Initial_MaxPromotionLevel;
        MaxLevel = list[0].MaxLevel;
        AttackPowerIncrease = list[0].AttackPowerIncrease;
        HealthIncrease = list[0].HealthIncrease;
        Initial_UpgradeStoneCost = list[0].Initial_UpgradeStoneCost;
        UpgradeStoneIncrease = list[0].UpgradeStoneIncrease;
        Initial_UpgradeGoldCost = list[0].Initial_UpgradeGoldCost;
        UpgradeGoldIncrease = list[0].UpgradeGoldIncrease;
    }
}
