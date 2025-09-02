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

    //���ѻ��� ����
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
            CheckAppVersion(); //�۹��� üũ
            setLimits(); //�������� ���
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

            Debug.Log($"���ù��� : {BuildController.Instance.GetAppVersion}, ������������ : {serializer.Deserialize<VersionInfo>(StaticData["AppVersion"]).Released}");
        }
    }

    int AppVersionToInt(string AppVersion)
    {
        //.���� ���� �Һз�(�ּ� �ΰ��� ����)�� �ϰ�, �ִ� 3������ �ִٰ� ������
        string[] str = AppVersion.Split('.');
        bool problem = false;
        int value = 0;

        if (str.Length < 2) { problem = true; } //x.x�� �ּұ����� �ƴѰ�� üũ

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
                    if (parsed > 999) { problem = true; break; } //�� ���й�ȣ�� ���ڸ��� �Ѵ� ��� üũ

                    value += parsed * weight;
                }
            }
            catch //int Parsing�� �ȵǴ� ��� üũ
            {
                problem = true;
            }
        }

        if (problem) { Debug.LogError("���� string ������ ������ �ֽ��ϴ�"); }

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

