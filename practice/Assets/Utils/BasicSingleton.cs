using UnityEngine;

/// <summary>
/// Generic MonoBehaviour-based Singleton.
/// - ������ Instance ����
/// - DontDestroyOnLoad �ɼ� ����
/// - �ߺ� �ν��Ͻ� �ڵ� ����
/// - ShuttingDown ���� ����
/// </summary>
public abstract class BasicSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [Tooltip("�� ��ȯ �ÿ��� �������� ����")]
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
                    // ������ ã�ƿ���
                    m_Instance = FindFirstObjectByType<T>();

                    if (m_Instance == null)
                    {
                        // �� ������Ʈ ����
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        m_Instance = singletonObject.AddComponent<T>();
                    }

                    // ���������� DontDestroyOnLoad ����
                    if (m_Instance is BasicSingleton<T> singleton && singleton.dontDestroyOnLoadEnabled)
                    {
                        DontDestroyOnLoad(m_Instance.gameObject);
                    }

                    // �ʱ�ȭ �ݹ�
                    if (m_Instance is BasicSingleton<T> basic)
                        basic.OnSingletonInit();
                }

                return m_Instance;
            }
        }
    }

    /// <summary>
    /// �ߺ� �ν��Ͻ� �߻� �� ó��
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
            Debug.LogWarning($"[Singleton] {typeof(T).Name} �ߺ� �ν��Ͻ� �߰� �� ���ŵ�");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �̱��� �ʱ�ȭ ������ ȣ��Ǵ� �ݹ� (������ override)
    /// </summary>
    protected virtual void OnSingletonInit() { }

    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }

    private void OnDestroy()
    {
        // �� ��ε�� Destroy�� ���� �ٽ� ���� �� �־�� ��
        if (Application.isPlaying)
            m_ShuttingDown = true;
    }
}