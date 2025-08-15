using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum ToastPosition { Top, Center, Bottom }

[DisallowMultipleComponent]
public class ToastManager : BasicSingleton<ToastManager>
{ 
    [Header("Containers (under an Overlay Canvas)")]
    [SerializeField] private RectTransform topContainer;
    [SerializeField] private RectTransform centerContainer;
    [SerializeField] private RectTransform bottomContainer;

    [Header("Prefab")]
    [SerializeField] private ToastView toastPrefab;

    [Header("Defaults")]
    [SerializeField] private float defaultDuration = 1.6f;
    [SerializeField] private Color defaultBg = new(0, 0, 0, 0.82f);
    [SerializeField] private Color defaultText = Color.white;
    [SerializeField] private int maxStackPerPosition = 3; // 동시에 표시 허용 개수

    readonly Dictionary<ToastPosition, List<ToastView>> _active = new()
    {
        { ToastPosition.Top,    new List<ToastView>() },
        { ToastPosition.Center, new List<ToastView>() },
        { ToastPosition.Bottom, new List<ToastView>() },
    };

    // 간단한 풀
    readonly Stack<ToastView> _pool = new();

    RectTransform GetContainer(ToastPosition pos) =>
        pos switch
        {
            ToastPosition.Top => topContainer,
            ToastPosition.Center => centerContainer,
            _ => bottomContainer
        };

    ToastView Spawn(RectTransform parent)
    {
        ToastView view = _pool.Count > 0 ? _pool.Pop() : Instantiate(toastPrefab);
        view.transform.SetParent(parent, false);
        view.gameObject.SetActive(true);
        return view;
    }

    void Despawn(ToastView view)
    {
        if (!view) return;
        view.HideImmediate();
        view.transform.SetParent(transform, false);
        view.gameObject.SetActive(false);
        _pool.Push(view);
    }

    public static void Show(string message,
                            float duration = -1f,
                            ToastPosition position = ToastPosition.Bottom,
                            Color? bg = null,
                            Color? text = null)
    {
        Instance?.ShowInternal(message,
            duration < 0 ? Instance.defaultDuration : duration,
            position, bg ?? Instance.defaultBg, text ?? Instance.defaultText);
    }

    void ShowInternal(string message, float duration, ToastPosition pos, Color bg, Color txt)
    {
        var container = GetContainer(pos);
        if (!container || !toastPrefab)
        {
            Debug.LogWarning("[ToastManager] Not configured.");
            return;
        }

        // 스택 제한: 가득 찼으면 가장 오래된 것부터 제거
        var list = _active[pos];
        while (list.Count >= maxStackPerPosition)
        {
            var oldest = list[0];
            list.RemoveAt(0);
            Despawn(oldest);
        }

        var toast = Spawn(container);
        toast.SetText(message);
        toast.SetColors(bg, txt);
        list.Add(toast);

        toast.Play(duration, () =>
        {
            list.Remove(toast);
            Despawn(toast);
        });
    }
}
