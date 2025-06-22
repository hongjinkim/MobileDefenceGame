using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PoolManager<T, U> : BasicSingleton<T> where T : MonoBehaviour where U : Enum
{
    [SerializeField] protected List<PoolBase<U>> m_Pools = new List<PoolBase<U>>();

    public List<PoolBase<U>> Pools => m_Pools;

    protected void Awake()
    {
        m_Pools = GetComponentsInChildren<PoolBase<U>>().ToList();
    }

    public void AddPools(PoolBase<U> pool) => m_Pools.Add(pool);

    public void Initialize()
    {
        for (int i = 0; i < Pools.Count; i++)
        {
            Pools[i].Initialize();
        }
    }

    public GameObject Pop(U Index)
    {
        int PoolIndex = GetEnumIndex(Index);
        if (PoolIndex != -1)
            return Pools[PoolIndex].Pop();
        else
            return null;
    }

    public GameObject Pop(U Index, Vector3 position)
    {
        var obj = Pop(Index);
        obj.transform.position = position;
        return obj;
    }

    public void Push(GameObject gameObject, U Index)
    {
        int PoolIndex = GetEnumIndex(Index);
        if (PoolIndex != -1)
            Pools[PoolIndex].Push(gameObject);
    }

    // 해당 Index와 동일한 값을 리스트에서 찾아 반환
    // 
    private int GetEnumIndex(U Index)
    {
        for (int i = 0; i < Pools.Count; i++)
        {
            // 풀에 들어있는 오브젝트의 Enum타입이 요청 Enum타입과 동일한 경우(Index : 요청들어온 타입)
            if (Pools[i].PoolType.CompareTo(Index) == 0)
                return i;
        }
        return -1;
    }
}
