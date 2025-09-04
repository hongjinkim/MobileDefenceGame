using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class PopupEventBase : MonoBehaviour
{
    public event Action OnClose = delegate { };

    [SerializeField] protected bool IsAutoClose = false;
    [EnableIf("IsAutoClose")][SerializeField] protected float AutoCloseTime = 1;

    protected RectTransform RectTransform
        => rectTransform == null
        ? (rectTransform = (RectTransform)transform)
        : rectTransform;

    protected CanvasGroup CanvasGroup
        => canvasGroup == null
        ? (canvasGroup = GetComponent<CanvasGroup>())
        : canvasGroup;

    private RectTransform rectTransform = null;
    private CanvasGroup canvasGroup = null;


    protected IEnumerator AutoClose()
    {
        if (!IsAutoClose) yield break;

        yield return new WaitForSeconds(AutoCloseTime);

        Close();
    }

    public abstract void Open();

    public virtual void Close() => OnClose?.Invoke();

}
