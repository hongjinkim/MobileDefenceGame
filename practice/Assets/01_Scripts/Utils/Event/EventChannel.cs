using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public abstract class EventChannelSO<T> : ScriptableObject
{
    [HideInInspector] public UnityEvent<T> OnEventRaisedInspector;

    // C# 델리게이트 (코드에서 구독/발생용)
    private event UnityAction<T> onEventRaised;

    // 이벤트를 구독하는 메서드
    public void AddListener(UnityAction<T> action)
    {
        onEventRaised += action;
    }

    // 이벤트 구독을 해제하는 메서드
    public void RemoveListener(UnityAction<T> action)
    {
        onEventRaised -= action;
    }

    // 이벤트를 발생시키는 메서드
    public void TriggerEvent(T value)
    {
        // C# 델리게이트를 통해 이벤트를 발생시킵니다.
        onEventRaised?.Invoke(value);

        // UnityEvent도 함께 발생시켜 인스펙터 연결된 함수도 호출되도록 합니다.
        // 필요에 따라 둘 중 하나만 사용하거나, UnityEvent 대신 C# 이벤트를 직렬화 불가능한 필드로 사용하고
        // 인스펙터에서 보여주지 않는 방식으로 구현할 수도 있습니다.
        OnEventRaisedInspector?.Invoke(value);

        Debug.Log($"Event raised: {this.name} with value: {value}");
    }
}

[CreateAssetMenu(menuName = "Events/Int Event Channel")]
public class IntEventChannelSO : EventChannelSO<int>
{
    // 추가적인 로직이나 필드를 필요에 따라 여기에 정의할 수 있습니다.
}


[CreateAssetMenu(menuName = "Events/String Event Channel")]
public class StringEventChannelSO : EventChannelSO<string>
{
    // 추가적인 로직이나 필드를 필요에 따라 여기에 정의할 수 있습니다.
}

[CreateAssetMenu(menuName = "Events/Dictionary Event Channel")]
public class DictionaryEventChannelSO : EventChannelSO<Dictionary<string, object>>
{
    // 추가적인 로직이나 필드를 필요에 따라 여기에 정의할 수 있습니다.
}


[CreateAssetMenu(menuName = "Events/Void Event Channel")]
public class VoidEventChannelSO : ScriptableObject
{
    // C# 델리게이트
    private event UnityAction onEventRaised;

    // 이벤트를 구독하는 메서드
    public void AddListener(UnityAction action)
    {
        onEventRaised += action;
    }

    // 이벤트 구독을 해제하는 메서드
    public void RemoveListener(UnityAction action)
    {
        onEventRaised -= action;
    }

    // 이벤트를 발생시키는 메서드
    public void RaiseEvent()
    {
        onEventRaised?.Invoke();
        Debug.Log($"Event raised: {this.name}");
    }
}