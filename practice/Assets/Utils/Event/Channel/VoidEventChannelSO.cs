using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Void Event Channel")]
public class VoidEventChannelSO : ScriptableObject
{
    // C# 델리게이트
    private event UnityAction onEventTriggered;

    // 이벤트를 구독하는 메서드
    public void AddListener(UnityAction action)
    {
        onEventTriggered += action;
    }

    // 이벤트 구독을 해제하는 메서드
    public void RemoveListener(UnityAction action)
    {
        onEventTriggered -= action;
    }

    // 이벤트를 발생시키는 메서드
    public void TriggerEvent()
    {
        onEventTriggered?.Invoke();
    }
}