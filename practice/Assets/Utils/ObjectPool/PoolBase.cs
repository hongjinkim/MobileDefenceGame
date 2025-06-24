using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBase<T> : MonoBehaviour where T : Enum
{
    [SerializeField] private GameObject m_GameObj;
    [SerializeField] public T PoolType;
    [SerializeField] private List<GameObject> m_List = new List<GameObject>();
    [SerializeField] private int m_AwakeCreateCount = 20;
    //[SerializeField] private int m_OverCreateCount = 5;

    public List<GameObject> popList()
    {
        var newList = new List<GameObject>();
        foreach (var item in m_List)
        {
            if (item.activeSelf)
                newList.Add(item);
        }

        return newList;
    }

    public void Initialize()
    {
        for (int i = 0; i < m_List.Count; i++)
        {
            if (m_List[i].activeSelf)
            {
                Push(m_List[i]);
            }
        }
    }

    private void Awake()
    {
        if (m_GameObj != null)
        {
            for (int i = 0; i < m_AwakeCreateCount; i++)
            {
                m_List.Add(Instantiate(m_GameObj));
                m_List[i].SetActive(false);
                m_List[i].transform.SetParent(transform);
                m_List[i].transform.localPosition = Vector3.zero;
            }
        }
        else Debug.LogError($"등록된 게임오브젝트가 없습니다.{gameObject.name}");
    }

    public GameObject Pop()
    {
        for (int i = 0; i < m_List.Count; i++)
        {
            if (!m_List[i].activeSelf)
            {
                m_List[i].SetActive(true);
                return m_List[i];
            }
        }

        GameObject go = Instantiate(m_GameObj);
        go.SetActive(true);
        go.transform.SetParent(transform);
        m_List.Add(go);
        return go;
    }

    public GameObject DropToastPop()
    {
        int LastIndex = m_List.Count - 1;
        for (int i = 0; i < m_List.Count; i++)
        {
            if (!m_List[i].activeSelf)
            {
                m_List[i].transform.SetSiblingIndex(LastIndex);
                m_List[i].SetActive(true);
                return m_List[i];
            }
        }

        GameObject go = Instantiate(m_GameObj);
        go.SetActive(true);
        go.transform.SetParent(transform);
        m_List.Add(go);
        return go;
    }


    public GameObject OncePop()
    {
        if (m_List.Count == 0)
        {
            GameObject go = Instantiate(m_GameObj);
            go.SetActive(true);
            go.transform.SetParent(transform);
            m_List.Add(go);
            return go;
        }

        else
        {
            m_List[0].SetActive(true);
            return m_List[0];
        }
    }

    public GameObject Pop(Transform parent, Vector3 position)
    {
        GameObject go = Pop();
        go.transform.SetParent(parent);
        if (go.GetComponent<RectTransform>())
        {
            go.GetComponent<RectTransform>().anchoredPosition = position;
        }
        else go.transform.localPosition = position;
        return go;
    }

    public GameObject Pop(Vector3 position)
    {
        GameObject go = Pop();
        if (go.GetComponent<RectTransform>())
        {
            go.GetComponent<RectTransform>().anchoredPosition = position;
        }
        else go.transform.position = position;
        return go;
    }

    public void Push(GameObject go)
    {
        if (m_List.Contains(go))
        {
            go.transform.SetParent(transform);
            //go.transform.localPosition = Vector3.zero;
            go.SetActive(false);
        }
        else Debug.LogWarning($"등록되지 않은 게임 오브젝트입니다.{gameObject.name}");
    }

    public void PushAll()
    {
        for (int i = 0; i < m_List.Count; i++)
        {
            if (m_List[i].activeSelf)
            {
                Push(m_List[i]);
            }
        }
    }
}

