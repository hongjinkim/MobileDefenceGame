using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableKeyValuePair<TKey, TValue>
{
    public TKey Key;
    public TValue Value;

    public SerializableKeyValuePair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}
public abstract class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    // 인스펙터에 노출될 리스트
    [SerializeField]
    private List<SerializableKeyValuePair<TKey, TValue>> keyValuePairs = new List<SerializableKeyValuePair<TKey, TValue>>();

    // 실제 게임 로직에서 사용할 딕셔너리
    private Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

    // Public으로 노출하여 실제 딕셔너리처럼 사용할 수 있게 함
    public Dictionary<TKey, TValue> ToDictionary()
    {
        return _dictionary;
    }

    // Unity가 직렬화된 데이터를 불러온 후 호출
    public void OnAfterDeserialize()
    {
        _dictionary.Clear();
        foreach (var pair in keyValuePairs)
        {
            // 키 중복 체크
            if (pair.Key != null && !_dictionary.ContainsKey(pair.Key))
            {
                _dictionary.Add(pair.Key, pair.Value);
            }
        }
    }

    // Unity가 데이터를 직렬화하기 전 호출 (여기서는 특별히 할 일이 없음)
    public void OnBeforeSerialize()
    {
    }
}


// Dictionary 직렬화용 클래스
//[Serializable]
//public class StringIntDictionary : SerializableDictionary<string, int> { }

