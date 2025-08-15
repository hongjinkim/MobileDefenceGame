using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStartButton : UIButton
{
    protected override void OnClicked()
    {
        PlayerManager.CheckEnergyToStart();
    }
}
