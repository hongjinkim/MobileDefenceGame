
using Sirenix.OdinInspector;

using UGS;
using UnityEngine;


public class DataBase : MonoBehaviour
{
    public static DataBase Instance { get; private set; } = null;
    private bool isDataLoaded = false;



    // data 클래스들을 여기에 선언

    [TabGroup("Tabs", "Player"), HideLabel][InlineProperty][SerializeField] private PlayerData playerData = new PlayerData();
    [TabGroup("Tabs", "Initial"), HideLabel][InlineProperty][SerializeField] private InitialData initialData = new InitialData();
    [TabGroup("Tabs", "Hero"), HideLabel][InlineProperty][SerializeField] private HeroData heroData = new HeroData();
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
        isDataLoaded = true;
        EventManager.Raise(EEventType.DataLoaded);
    }

    private void LoadData()
    {
        // 각 데이터 클래스의 데이터를 로드
        playerData.LoadData();
        initialData.LoadData();
        heroData.LoadData();
        stageData.LoadData();
    }


    public static bool TryGetHeroValue(string id, out HeroValue value)
    {
        return Instance.heroData.HeroDict.TryGetValue(id, out value);
    }
    public static bool TryGetStageValue(int id, out StageValue value)
    {
        return Instance.stageData.StageDict.TryGetValue(id.ToString(), out value);
    }
}
