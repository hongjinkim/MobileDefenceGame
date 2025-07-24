using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SerializedDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    private Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();

    public TValue this[TKey key]
    {
        get => dict[key];
        set
        {
            dict[key] = value;
            SyncLists();
        }
    }

    public Dictionary<TKey, TValue>.KeyCollection Keys => dict.Keys;
    public Dictionary<TKey, TValue>.ValueCollection Values => dict.Values;
    public int Count => dict.Count;

    public bool ContainsKey(TKey key) => dict.ContainsKey(key);

    public bool Remove(TKey key)
    {
        var result = dict.Remove(key);
        SyncLists();
        return result;
    }

    public void Add(TKey key, TValue value)
    {
        dict.Add(key, value);
        SyncLists();
    }

    public void Clear()
    {
        dict.Clear();
        keys.Clear();
        values.Clear();
    }

    public Dictionary<TKey, TValue>.Enumerator GetEnumerator() => dict.GetEnumerator();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (var kvp in dict)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        dict = new Dictionary<TKey, TValue>();
        for (int i = 0; i < Math.Min(keys.Count, values.Count); i++)
        {
            if (!dict.ContainsKey(keys[i]))
                dict.Add(keys[i], values[i]);
        }
    }

    private void SyncLists()
    {
        keys.Clear();
        values.Clear();
        foreach (var kvp in dict)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }
}