using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupHeader : MonoBehaviour
{
    public EPopupType Type;
    public PopupEventBase PopupEvent = null;

#if UNITY_EDITOR
    private void Reset()
    {
        if (PopupEvent == null)
        {
            try
            {
                PopupEvent = GetComponentInChildren<PopupEventBase>(true);
            }
            catch { }
        }
    }
#endif


    private void OnEnable()
    {
        PopupEvent.Open();
    }

}
