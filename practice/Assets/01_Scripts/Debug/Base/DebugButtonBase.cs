using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButtonBase : UIButton
{

#if DEBUG_ON
    private void Awake()
    {
        
    }
    protected override void OnClicked()
    {
        
    }
#endif
}
