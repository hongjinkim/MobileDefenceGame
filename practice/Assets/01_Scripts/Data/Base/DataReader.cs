
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
        // 각 생성자에서 데이터 로드
        PlayerData = new PlayerData();
        InitialData = new InitialData();
        HeroData = new HeroData();

        DataLoadedEvent.RaiseEvent();
    }

}
