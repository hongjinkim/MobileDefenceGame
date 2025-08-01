using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// 범용 어드레서블 프리팹 매니저
public class PrefabManager : MonoBehaviour
{
    // Address(보통 string) → 캐싱된 에셋
    private readonly Dictionary<string, UnityEngine.Object> prefabCache = new();

    /// <summary>
    /// Addressable 에셋을 비동기로 로드(캐싱 포함)
    /// </summary>
    public IEnumerator LoadPrefabAsync<T>(string address, Action<T> onLoaded) where T : UnityEngine.Object
    {
        if (prefabCache.TryGetValue(address, out var cached) && cached is T cachedT)
        {
            onLoaded?.Invoke(cachedT);
            yield break;
        }

        var handle = Addressables.LoadAssetAsync<T>(address);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            prefabCache[address] = handle.Result;
            onLoaded?.Invoke(handle.Result);
        }
        else
        {
            Debug.LogError($"PrefabManager: Failed to load prefab at address: {address}");
            onLoaded?.Invoke(null);
        }
    }

    /// <summary>
    /// 즉시 반환(이미 로드된 경우에만)
    /// </summary>
    public T GetCachedPrefab<T>(string address) where T : UnityEngine.Object
    {
        if (prefabCache.TryGetValue(address, out var cached) && cached is T cachedT)
            return cachedT;
        return null;
    }

    /// <summary>
    /// Address로 바로 Instantiate (비동기)
    /// </summary>
    public void InstantiateAsync(string address, Action<GameObject> onInstantiated)
    {
        Addressables.InstantiateAsync(address).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
                onInstantiated?.Invoke(handle.Result);
            else
            {
                Debug.LogError($"PrefabManager: Instantiate 실패: {address}");
                onInstantiated?.Invoke(null);
            }
        };
    }

    /// <summary>
    /// 미리 로드된 프리팹을 직접 Instantiate
    /// </summary>
    public GameObject InstantiateFromCache(string address, Vector3 pos, Quaternion rot)
    {
        var prefab = GetCachedPrefab<GameObject>(address);
        if (prefab == null)
        {
            Debug.LogError($"PrefabManager: 캐싱된 프리팹이 없습니다: {address}");
            return null;
        }
        return Instantiate(prefab, pos, rot);
    }

    /// <summary>
    /// Address의 캐싱 해제 (메모리 언로드)
    /// </summary>
    public void ReleasePrefab(string address)
    {
        if (prefabCache.TryGetValue(address, out var cached))
        {
            Addressables.Release(cached);
            prefabCache.Remove(address);
        }
    }

    /// <summary>
    /// 전체 캐시 언로드
    /// </summary>
    public void ReleaseAll()
    {
        foreach (var kv in prefabCache)
            Addressables.Release(kv.Value);
        prefabCache.Clear();
    }
}
