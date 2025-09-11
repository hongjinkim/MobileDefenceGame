#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class EventListenerTester : MonoBehaviour
{
    [Title("�̺�Ʈ ��� �׽�Ʈ")]
    [EnumPaging]
    public EEventType eventType = EEventType.MainSceneOpened;

    [InfoBox("�ܼ�/���̷ε� �̺�Ʈ ���� �ʿ�� ���� ����")]
    public bool isSimpleEvent = true;

    [Button("�̺�Ʈ �߻���Ű�� (Raise)")]
    public void RaiseSelectedEvent()
    {
        if (isSimpleEvent)
            EventManager.Raise(eventType);
        else
            EventManager.Raise<object>(eventType, null); // payload�� ����(null) ����
    }

    [Title("���� ���� ���� ������ ���")]
    [ListDrawerSettings(Expanded = true)]
    [ShowInInspector, ReadOnly]
    private List<string> _subscribers = new();

    [Button("������ ��� ���ΰ�ħ")]
    public void RefreshSubscribers()
    {
        _subscribers = EventManager.GetSubscribers(eventType, isSimpleEvent);
    }

    private void OnValidate() // �ν����Ϳ��� ���� �ٲ� ������ �ڵ����� ������ ���ΰ�ħ
    {
        RefreshSubscribers();
    }
}
#endif