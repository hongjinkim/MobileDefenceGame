using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLevelUp : UIButton
{


#if DEBUG_ON
    protected override void OnClicked()
    {
        StageManager.Instance.OnPlayerLevelUp();
    }
#endif

}
