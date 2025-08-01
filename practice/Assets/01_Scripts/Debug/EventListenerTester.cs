#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class EventListenerTester : MonoBehaviour
{
    [Title("이벤트 등록 테스트")]
    [EnumPaging]
    public EEventType eventType = EEventType.DataLoaded;

    [InfoBox("단순/페이로드 이벤트 구분 필요시 직접 수정")]
    public bool isSimpleEvent = true;

    [Button("이벤트 발생시키기 (Raise)")]
    public void RaiseSelectedEvent()
    {
        if (isSimpleEvent)
            EventManager.Raise(eventType);
        else
            EventManager.Raise<object>(eventType, null); // payload는 임의(null) 전달
    }

    [Title("현재 구독 중인 리스너 목록")]
    [ListDrawerSettings(Expanded = true)]
    [ShowInInspector, ReadOnly]
    private List<string> _subscribers = new();

    [Button("구독자 목록 새로고침")]
    public void RefreshSubscribers()
    {
        _subscribers = EventManager.GetSubscribers(eventType, isSimpleEvent);
    }

    private void OnValidate() // 인스펙터에서 값이 바뀔 때마다 자동으로 구독자 새로고침
    {
        RefreshSubscribers();
    }
}
#endif