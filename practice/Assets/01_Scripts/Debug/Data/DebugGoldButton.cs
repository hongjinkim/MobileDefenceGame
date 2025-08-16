using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGoldButton : UIButton
{

    public int GoldAmount = 500;

#if DEBUG_ON
    private void Awake()
    {
        
    }
    protected override void OnClicked()
    {
        DataBase.PlayerData.Value.UpdateGold(GoldAmount);
    }
#endif
}
