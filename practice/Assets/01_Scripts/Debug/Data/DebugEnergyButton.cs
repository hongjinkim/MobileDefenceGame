using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEnergyButton : UIButton
{
    public int EnergyAmount = 5;

#if DEBUG_ON
    private void Awake()
    {
        
    }
    protected override void OnClicked()
    {
        DataBase.PlayerData.Value.UpdateEnergy(EnergyAmount);
    }
#endif
}
