using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCrystalButton : UIButton
{

#if DEBUG_ON
    private void Awake()
    {
        
    }
    protected override void OnClicked()
    {
        DataBase.PlayerData.Value.UpdateCrystal(100);
    }
#endif
}
