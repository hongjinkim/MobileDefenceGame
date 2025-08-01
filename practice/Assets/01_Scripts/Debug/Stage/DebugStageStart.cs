using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStageStart : UIButton
{


#if DEBUG_ON
    protected override void OnClicked()
    {
        StageManager.Instance.StageStart();
    }
#endif

}
