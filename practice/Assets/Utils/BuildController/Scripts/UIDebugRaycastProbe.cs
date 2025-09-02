using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDebugRaycastProbe : MonoBehaviour
{
    PointerEventData _ped;
    List<RaycastResult> _results = new();

    void Awake()
    {
        var es = EventSystem.current;
        if (es == null) Debug.LogError("No EventSystem in scene!");
        _ped = new PointerEventData(es);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _results.Clear();
            _ped.position = Input.mousePosition;
            EventSystem.current.RaycastAll(_ped, _results);

            Debug.Log($"[UI Raycast] hits: {_results.Count}");
            for (int i = 0; i < _results.Count; i++)
            {
                var r = _results[i];
                Debug.Log($"{i,2}: {r.gameObject.name} | canv={r.module?.ToString()} | dist={r.distance}");
            }
        }
    }
}
