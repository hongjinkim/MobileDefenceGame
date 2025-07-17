
using UGS;
using UnityEngine;


public class DataReader : MonoBehaviour
{
    public static DataReader Instance { get; private set; } = null;
    public VoidEventChannelSO DataLoadedEvent;

    // data 클래스들을 여기에 선언
    public PlayerData PlayerData;
    public InitialData InitialData;
    public HeroData HeroData;
    public StageData StageData;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        UnityGoogleSheet.LoadAllData();
        Initialize();
    }

    // 시작 시 데이터 초기화
    private void Initialize()
    {
        LoadData();
        DataLoadedEvent.RaiseEvent();
    }

    private void LoadData()
    {
        // 각 데이터 클래스의 데이터를 로드
        //PlayerData.LoadData();
        InitialData = new InitialData();
        HeroData = new HeroData();
        StageData = new StageData();
    }

}
