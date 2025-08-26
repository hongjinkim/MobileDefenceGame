
using Sirenix.OdinInspector;

using UGS;
using UnityEngine;
using System;


public class DataBase : MonoBehaviour
{
    public static DataBase Instance { get; private set; } = null;
    public bool IsLoaded { get; private set; }
    public event Action OnLoaded; // 마스터 데이터 로드 완료 신호

    // data 클래스들을 여기에 선언
    [TabGroup("Tabs", "Initial"), HideLabel][InlineProperty][SerializeField] public InitialData initialData = new InitialData();
    [TabGroup("Tabs", "Hero"), HideLabel][InlineProperty][SerializeField] private HeroData heroData = new HeroData();
    [TabGroup("Tabs", "HeroUpgrade"), HideLabel][InlineProperty][SerializeField] private HeroUpgradeData heroUpgradeData = new HeroUpgradeData();
    [TabGroup("Tabs", "HeroUpgrade"), HideLabel][InlineProperty][SerializeField] private EquipmentData equipmentData = new EquipmentData();
    [TabGroup("Tabs", "Stage"), HideLabel][InlineProperty][SerializeField] private StageData stageData = new StageData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            UnityGoogleSheet.LoadAllData();
            Initialize();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // 시작 시 데이터 초기화
    private void Initialize()
    {
        LoadData();
        IsLoaded = true;
        Debug.Log("DataBase Initialized and Data Loaded Successfully.");

        OnLoaded?.Invoke();
    }

    private void LoadData()
    {
        // 각 데이터 클래스의 데이터를 로드
        initialData.LoadData();
        heroData.LoadData();
        heroUpgradeData.LoadData();
        stageData.LoadData();
    }


    public static HeroData GetHeroData() => Instance.heroData;
    public static bool TryGetHeroValue(string id, out HeroValue value)
    {
        return Instance.heroData.HeroDict.TryGetValue(id, out value);
    }

    public static bool TryGetStageValue(int id, out StageValue value)
    {
        return Instance.stageData.StageDict.TryGetValue(id.ToString(), out value);
    }
    public static bool TryGetStageName(int id, out string name)
    {
        return Instance.stageData.StageNameDict.TryGetValue(id, out name);
    }
}
