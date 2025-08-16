using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCrystalButton : UIButton
{
    public int CrystalAmount = 100;

#if DEBUG_ON
    private void Awake()
    {
        
    }
    protected override void OnClicked()
    {
        DataBase.PlayerData.Value.UpdateCrystal(CrystalAmount);
    }
#endif
}
