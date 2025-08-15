using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ExclusivePanelGroup : MonoBehaviour
{
    [Serializable]
    public struct PanelEntry
    {
        public string Id;       // "Lobby", "Shop", "Settings" 등 고유 ID
        public GameObject Panel;
    }

    [Header("한 번에 하나만 활성화될 패널 그룹")]
    [SerializeField] private List<PanelEntry> panels = new List<PanelEntry>();

    [Tooltip("씬 시작 시 켤 기본 패널 (Id 기준, 비우면 모두 비활성화)")]
    [SerializeField] private string defaultPanelId = "";

    private readonly Dictionary<string, GameObject> _map = new();
    public GameObject Current { get; private set; }

    void Awake()
    {
        BuildMap();
        if (!string.IsNullOrEmpty(defaultPanelId) && _map.TryGetValue(defaultPanelId, out var def))
        {
            ShowOnly(def);
        }
        else
        {
            // 기본값이 없으면 모두 끔
            SetAllActive(false);
            Current = null;
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        // 중복/누락 간단 방어: null 제거, 중복 ID 경고
        var seen = new HashSet<string>();
        for (int i = panels.Count - 1; i >= 0; i--)
        {
            var p = panels[i];
            if (p.Panel == null)
            {
                panels.RemoveAt(i);
                continue;
            }
            if (!string.IsNullOrEmpty(p.Id))
            {
                if (!seen.Add(p.Id))
                {
                    Debug.LogWarning($"ExclusivePanelGroup: 중복 ID '{p.Id}'", this);
                }
            }
        }
    }
#endif

    private void BuildMap()
    {
        _map.Clear();
        foreach (var p in panels)
        {
            if (p.Panel == null) continue;
            if (!string.IsNullOrEmpty(p.Id))
            {
                _map[p.Id] = p.Panel; // 나중 항목이 앞을 덮음 (의도적)
            }
        }
    }

    private void SetAllActive(bool active)
    {
        foreach (var p in panels)
        {
            if (p.Panel != null)
                p.Panel.SetActive(active);
        }
    }

    /// <summary>해당 패널만 On</summary>
    public void ShowOnly(GameObject target)
    {
        if (target == null)
        {
            Debug.LogError("ShowOnly: target이 null입니다.", this);
            return;
        }

        foreach (var p in panels)
        {
            if (p.Panel == null) continue;
            p.Panel.SetActive(p.Panel == target);
        }
        Current = target;
    }

    /// <summary>Index로 전환</summary>
    public void ShowOnly(int index)
    {
        if (index < 0 || index >= panels.Count)
        {
            Debug.LogError($"ShowOnly: index {index} 범위 초과", this);
            return;
        }
        ShowOnly(panels[index].Panel);
    }

    /// <summary>Id로 전환</summary>
    public void ShowOnly(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogError("ShowOnly: id가 비어있습니다.", this);
            return;
        }
        if (!_map.TryGetValue(id, out var go) || go == null)
        {
            Debug.LogError($"ShowOnly: id '{id}'에 해당하는 패널이 없습니다.", this);
            return;
        }
        ShowOnly(go);
    }

    /// <summary>모두 끄기</summary>
    public void HideAll()
    {
        SetAllActive(false);
        Current = null;
    }

    /// <summary>현재 켜진 패널의 Id 가져오기 (없으면 빈 문자열)</summary>
    public string CurrentId
    {
        get
        {
            if (Current == null) return "";
            foreach (var p in panels)
            {
                if (p.Panel == Current) return p.Id;
            }
            return "";
        }
    }
}
