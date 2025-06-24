using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public abstract class EventChannelSO<T> : ScriptableObject
{
    // C# 델리게이트 (코드에서 구독/발생용)
    private event UnityAction<T> onEventTriggered;

    // 이벤트를 구독하는 메서드
    public void AddListener(UnityAction<T> action)
    {
        onEventTriggered += action;
    }

    // 이벤트 구독을 해제하는 메서드
    public void RemoveListener(UnityAction<T> action)
    {
        onEventTriggered -= action;
    }

    // 이벤트를 발생시키는 메서드
    public void TriggerEvent(T value)
    {
        // C# 델리게이트를 통해 이벤트를 발생시킵니다.
        onEventTriggered?.Invoke(value);
    }
}