using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSummonHero : UIButton
{
    public int idx;

#if DEBUG_ON
    protected override void OnClicked()
    {
        InGameHeroManager.Instance.SummonHero(idx);
    }
#endif
}
