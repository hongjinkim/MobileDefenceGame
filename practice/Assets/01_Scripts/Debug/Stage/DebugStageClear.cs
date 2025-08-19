using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStageClear : UIButton
{
#if DEBUG_ON
    private void Awake()
    {

    }
    protected override void OnClicked()
    {
        PlayerManager.Instance.ProceedToNextStage();
    }
#endif
}
