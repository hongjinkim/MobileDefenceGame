using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEnergyButton : UIButton
{

#if DEBUG_ON
    private void Awake()
    {
        
    }
    protected override void OnClicked()
    {
        DataBase.PlayerData.Value.UpdateEnergy(5);
    }
#endif
}
