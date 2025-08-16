using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGoldButton : UIButton
{

#if DEBUG_ON
    private void Awake()
    {
        
    }
    protected override void OnClicked()
    {
        DataBase.PlayerData.Value.UpdateGold(500);
    }
#endif
}
