using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugRestoreLimitOnOffButton : UIButton
{
    public static bool Ignore = false;

    public TMP_Text Label;

#if DEBUG_ON

    protected override void OnEnable()
    {
        base.OnEnable();
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        if (Ignore == true)
        {
            Label.color = Color.green;
            Label.SetText("복원시간 무시 중");
        }
        else
        {
            Label.color = Color.white;
            Label.SetText("복원시간 적용 중");
        }
    }

    protected override void OnClicked()
    {
        Ignore = !Ignore;
        UpdateLabel();
    }
#endif
}
