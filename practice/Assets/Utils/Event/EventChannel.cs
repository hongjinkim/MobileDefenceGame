using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public abstract class EventChannelSO<T> : ScriptableObject
{
    // 인스펙터에서 리스너를 등록하기 위한 UnityEvent
    public UnityEvent<T> onEventRaised;

    // 코드 기반으로 리스너를 등록하기 위한 C# Action (기존 방식)
    private UnityAction<T> _onEventRaised;

    public void AddListener(UnityAction<T> action)
    {
        _onEventRaised += action;
    }

    public void RemoveListener(UnityAction<T> action)
    {
        _onEventRaised -= action;
    }

    public void RaiseEvent(T value)
    {
        // 1. 인스펙터에 연결된 리스너들 호출
        onEventRaised?.Invoke(value);

        // 2. 코드로 구독한 리스너들 호출
        _onEventRaised?.Invoke(value);
    }
}