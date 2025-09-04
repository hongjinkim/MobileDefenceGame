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
            Label.SetText("�����ð� ���� ��");
        }
        else
        {
            Label.color = Color.white;
            Label.SetText("�����ð� ���� ��");
        }
    }

    protected override void OnClicked()
    {
        Ignore = !Ignore;
        UpdateLabel();
    }
#endif
}
