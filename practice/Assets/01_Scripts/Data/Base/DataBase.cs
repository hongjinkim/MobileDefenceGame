
using Sirenix.OdinInspector;

using UGS;
using UnityEngine;
using System;


public class DataBase : MonoBehaviour
{
    public static DataBase Instance { get; private set; } = null;
    public bool IsLoaded { get; private set; }
    public event Action OnLoaded; // ������ ������ �ε� �Ϸ� ��ȣ

    // data Ŭ�������� ���⿡ ����
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

    // ���� �� ������ �ʱ�ȭ
    private void Initialize()
    {
        LoadData();
        IsLoaded = true;
        Debug.Log("DataBase Initialized and Data Loaded Successfully.");

        OnLoaded?.Invoke();
    }

    private void LoadData()
    {
        // �� ������ Ŭ������ �����͸� �ε�
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
