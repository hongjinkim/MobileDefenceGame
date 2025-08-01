using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EventManager : BasicSingleton<EventManager>
{
    // 페이로드 있는 이벤트
    private static readonly Dictionary<EEventType, Delegate> eventTable = new();
    // 단순 이벤트 (페이로드 없음)
    private static readonly Dictionary<EEventType, Action> simpleEventTable = new();

    // 구독 - 제네릭(데이터)
    public static void Subscribe<T>(EEventType eventType, Action<T> listener)
    {
        if (eventTable.TryGetValue(eventType, out var del))
            eventTable[eventType] = Delegate.Combine(del, listener);
        else
            eventTable[eventType] = listener;
    }
    public static void Unsubscribe<T>(EEventType eventType, Action<T> listener)
    {
        if (eventTable.TryGetValue(eventType, out var del))
        {
            var current = Delegate.Remove(del, listener);
            if (current == null) eventTable.Remove(eventType);
            else eventTable[eventType] = current;
        }
    }
    public static void Raise<T>(EEventType eventType, T payload)
    {
        if (eventTable.TryGetValue(eventType, out var del))
            (del as Action<T>)?.Invoke(payload);
    }

    // 구독 - 단순
    public static void Subscribe(EEventType eventType, Action listener)
    {
        if (simpleEventTable.TryGetValue(eventType, out var action))
            simpleEventTable[eventType] = action + listener;
        else
            simpleEventTable[eventType] = listener;
    }
    public static void Unsubscribe(EEventType eventType, Action listener)
    {
        if (simpleEventTable.TryGetValue(eventType, out var action))
        {
            action -= listener;
            if (action == null) simpleEventTable.Remove(eventType);
            else simpleEventTable[eventType] = action;
        }
    }
    public static void Raise(EEventType eventType)
    {
        if (simpleEventTable.TryGetValue(eventType, out var action))
            action?.Invoke();
    }

    public static List<string> GetSubscribers(EEventType eventType, bool simple = false)
    {
        var result = new List<string>();

        if (!simple)
        {
            if (eventTable.TryGetValue(eventType, out var del))
            {
                foreach (var d in del.GetInvocationList())
                {
                    result.Add($"{d.Target} :: {d.Method.Name}");
                }
            }
        }
        else
        {
            if (simpleEventTable.TryGetValue(eventType, out var del))
            {
                foreach (var d in del.GetInvocationList())
                {
                    result.Add($"{d.Target} :: {d.Method.Name}");
                }
            }
        }
        return result;
    }
}
