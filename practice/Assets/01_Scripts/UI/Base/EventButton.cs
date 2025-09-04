using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventButton : UIButton
{
    public event Action OnClick = delegate { };

    protected override void OnClicked()
    {
        OnClick?.Invoke();
    }
}

