using UnityEngine;


public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; } = null;

    public LocalizationSystem System = null;

    public bool GetLanguageInit() => System.GetLanguageInit();
    public void SetLanguageInit() => System.SetLanguageInit();
    public ELanguage GetCurrentLanguage() => System.GetCurrentLanguage();
    public void SetCurrentLanguage(ELanguage lang) => System.SetCurrentLanguage(lang);
    private void Initialize() => System.Initialize();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();        
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


#if UNITY_EDITOR
    private void Reset()
    {
        try
        {
            if (System == null)
            {
                string path = "Assets/Utils/LocalizationSystem/LocalizationSystem.asset";
                var system = UnityEditor.AssetDatabase.LoadAssetAtPath<LocalizationSystem>(path);
                System = system;
            }
        }
        catch
        {

        }
    }
#endif

}


