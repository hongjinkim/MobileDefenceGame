using UnityEngine;

/// <summary>
/// Generic MonoBehaviour-based Singleton.
/// - 안전한 Instance 접근
/// - DontDestroyOnLoad 옵션 지원
/// - 중복 인스턴스 자동 정리
/// - ShuttingDown 상태 방지
/// </summary>
public abstract class BasicSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [Tooltip("씬 전환 시에도 유지할지 여부")]
    [SerializeField] private bool dontDestroyOnLoadEnabled = false;

    private static readonly object m_Lock = new object();
    private static bool m_ShuttingDown = false;
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            if (m_ShuttingDown) return null;

            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    // 씬에서 찾아오기
                    m_Instance = FindFirstObjectByType<T>();

                    if (m_Instance == null)
                    {
                        // 새 오브젝트 생성
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        m_Instance = singletonObject.AddComponent<T>();
                    }

                    // 선택적으로 DontDestroyOnLoad 적용
                    if (m_Instance is BasicSingleton<T> singleton && singleton.dontDestroyOnLoadEnabled)
                    {
                        DontDestroyOnLoad(m_Instance.gameObject);
                    }

                    // 초기화 콜백
                    if (m_Instance is BasicSingleton<T> basic)
                        basic.OnSingletonInit();
                }

                return m_Instance;
            }
        }
    }

    /// <summary>
    /// 중복 인스턴스 발생 시 처리
    /// </summary>
    protected virtual void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this as T;
            if (dontDestroyOnLoadEnabled)
                DontDestroyOnLoad(gameObject);
        }
        else if (m_Instance != this)
        {
            Debug.LogWarning($"[Singleton] {typeof(T).Name} 중복 인스턴스 발견 → 제거됨");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 싱글톤 초기화 시점에 호출되는 콜백 (선택적 override)
    /// </summary>
    protected virtual void OnSingletonInit() { }

    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }

    private void OnDestroy()
    {
        // 씬 언로드로 Destroy된 경우는 다시 만들 수 있어야 함
        if (Application.isPlaying)
            m_ShuttingDown = true;
    }
}