using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayFabServerTitleDataReceiver : MonoBehaviour
{
    public static event Action OnRefresh;

    public static Dictionary<string, string> StaticData = new();

    private static PlayFabServerTitleDataReceiver instance = null;

    //제한사항 관련
    public static int ResourceLimit;
    public static int CreditLimit;
    private IJSONSerializer serializer;


    [SerializeField]
    private float RefreshSeconds = 600;

    private TimeSpan RefreshTerm => TimeSpan.FromSeconds(RefreshSeconds);

    [Header("CHECK ONLY")]
    public List<TitleDataKeyPair> Current = new();

    private DateTimeOffset lastRefreshUTC;
    private bool isInitialized = false;
    private bool isOnPending = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        serializer = GetComponent<IJSONSerializer>();
    }

    private void OnEnable()
    {
        TimeManager.OnSecondChanged += CheckTime;
        LobbyManager.OnReadyToLoadMainScene += Request;
    }

    private void OnDisable()
    {
        TimeManager.OnSecondChanged -= CheckTime;
        LobbyManager.OnReadyToLoadMainScene -= Request;
    }

    //private void Start()
    //{
    //    Request();    
    //}


    public void ForceRefresh() => Request();


    private void CheckTime(DateTimeOffset now)
    {
        if (lastRefreshUTC + RefreshTerm < now)
        {
            Request();
        }
    }


    private void Request()
    {
        if (isOnPending == true) { return; }

        isOnPending = true;
        lastRefreshUTC = TimeManager.NowUTC;

        try
        {
            var request = new GetTitleDataRequest();
            PlayFabClientAPI.GetTitleData(request, OnSuccess, OnError);
        }
        catch (Exception e)
        {
            isOnPending = false;
            Debug.LogException(e);
        }
    }

    private void OnSuccess(GetTitleDataResult result)
    {
        isOnPending = false;

        StaticData.Clear();
        StaticData = result.Data;

#if UNITY_EDITOR
        Current.Clear();
        foreach ((string key, string json) in StaticData)
        {
            Current.Add(new TitleDataKeyPair
            {
                Key = key,
                JSON = json
            });
        }
#endif
        if (isInitialized == false)
        {
            isInitialized = true;
            CheckAppVersion(); //앱버전 체크
            setLimits(); //제한정보 등록
            OnRefresh?.Invoke();
        }
    }

    private void OnError(PlayFabError error)
    {
        isOnPending = false;
        Debug.LogError(error.ErrorMessage, gameObject);
    }

    private void CheckAppVersion()
    {
        if (BuildController.Instance.GetLiveServer)
        {
            int localAppVersion = AppVersionToInt(BuildController.Instance.GetAppVersion);
            int serverAppVersion = AppVersionToInt(serializer.Deserialize<VersionInfo>(StaticData["AppVersion"]).Released);

            if (localAppVersion < serverAppVersion) { AppVersionChecker.Instance.RequestAppUpdate(); }

            Debug.Log($"로컬버전 : {BuildController.Instance.GetAppVersion}, 서버배포버전 : {serializer.Deserialize<VersionInfo>(StaticData["AppVersion"]).Released}");
        }
    }

    int AppVersionToInt(string AppVersion)
    {
        //.으로 버전 소분류(최소 두개로 구분)를 하고, 최대 3개까지 있다고 전제함
        string[] str = AppVersion.Split('.');
        bool problem = false;
        int value = 0;

        if (str.Length < 2) { problem = true; } //x.x의 최소구성이 아닌경우 체크

        else
        {
            try
            {
                int weight;
                for (int i = 0; i < str.Length; i++)
                {
                    if (i == 0) { weight = 1_000_000; }
                    else if (i == 1) { weight = 1_000; }
                    else { weight = 1; }

                    int parsed = int.Parse(str[i]);
                    if (parsed > 999) { problem = true; break; } //각 구분번호가 세자리를 넘는 경우 체크

                    value += parsed * weight;
                }
            }
            catch //int Parsing이 안되는 경우 체크
            {
                problem = true;
            }
        }

        if (problem) { Debug.LogError("버전 string 구성에 문제가 있습니다"); }

        return value;
    }


    private void setLimits()
    {
        var Limits = serializer.Deserialize<LimitSettings>(StaticData["LimitSettings"]);
        ResourceLimit = Limits.Resource;
        CreditLimit = Limits.Credit;
    }

    [Serializable]
    public class TitleDataKeyPair
    {
        public string Key;
        public string JSON;
    }

    public class VersionInfo
    {
        public string Released;
    }

    public class LimitSettings
    {
        public int Resource;
        public int Credit;
    }
}

