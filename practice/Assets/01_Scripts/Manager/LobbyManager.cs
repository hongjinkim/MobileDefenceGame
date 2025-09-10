using DG.Tweening;
using PlayFab;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : BasicSingleton<LobbyManager>
{
    public static event Action<string> OnSuccessRegisterRandomDisplayName;
    public static event Action OnReadyToLoadMainScene;

    [Header("Server Ids")]
    public string LiveServerId = "";
    public string TestServerId = "";

    [Header("Managers")]
    [HideInInspector]public LoginManager LoginManager;
    public ClockManager ClockManager;
    public ServerHeaderManager HeaderDataManager;
    public ServerPlayerDataReceiver ServerDataReceiver;
    public ClientSaveDataLoader ClientDataReader;
    public PlayerDataUploader ClientDataToServer;

    [Header("Popups")]
    public GameObject PopupNoInternet;
    public GameObject PopupLoginMethod;
    public GameObject PopupLoginFailure;
    public GameObject PopupOverwriteSpinner;
    public GameObject PopupTesterLogin;
    public GameObject PopupServerUnderConstruction;
    public GameObject BlockSpinner;
    public GameObject PopupPrivatePolicy;

    [Header("UI")]
    public Button TapToStartButton;
    public TextMeshProUGUI ProgressText;
    //public PrologueDisplay Prologue;
    //public FadeOutEvent FadeOutScene;
    //public PopupRestoreDisplay RestorePopup;
    //public PopupBanDisplay BanPopup;
    public GameObject DebugPanel;
    public GameObject LoadingBar;
    public Image LoadingFill;
    //public Image LoadingBarIcon;
    //public Animator LoadingBarAnimator;
    //public Sprite LoadingFinishedSprite;
    //public GameObject LoadingUIEffect;
    private float loadingbarLength;
    private float loadingbarHalfLength;

    [Header("SOUND")]
    public AudioPlayerDissolve BgmSound;

    //public PoomVolumeHandler BGMVolumeHandler = null;
    //public PoomVolumeHandler SFXVolumeHandler = null;


    private RandomUsernameUploader randomIdCreator;
    private bool onCompleteDataLoading = false;
    private bool onCompleteTimeSetting = false;
    private bool onCompleteTitleData = false;
    private AsyncOperation mainSceneLoadOperation = null;

    private PlayerData Player => GameDataManager.PlayerData;

    private void Awake()
    {
        randomIdCreator = new RandomUsernameUploader();
        randomIdCreator.OnSuccess += OnSuccessUpdateRandomDisplayName;
        loadingbarLength = LoadingFill.rectTransform.rect.size.x;
        loadingbarHalfLength = loadingbarLength / 2;
    }

    private void OnEnable()
    {
        LoginManager.OnBannedPlayFabAccount += OnBanned;
        LoginManager.OnInternetNotReachable += ShowNoInternetPopup;
        LoginManager.OnFinishPlayFabLoginProcess += HideLoginMethodPopup;
        LoginManager.OnFinishPlayFabLoginProcess += GetPlayFabServerTime;
        ClockManager.OnSuccessSetValidClockAfterLogin += CompareHeader;
        ServerHeaderManager.OnEmptyServerRecord += CreateNewUser;
        ServerHeaderManager.OnForceOverwrite += ForceOverwrite;
        ServerHeaderManager.OnDifferPlayerId += AskRestore;
        ServerHeaderManager.OnDifferDeviceId += AskRestore;
        ServerHeaderManager.OnNext += OnValidHeader;
        //PrologueDisplay.OnFinish += GoToMainScene;
        PopupForceOverwriter.OnFinishOverwrite += SetDataReady;
        PlayFabServerTitleDataReceiver.OnRefresh += SetTitleDataReady;
        ServerStateChecker.OnServerUnderConstruction += UnderConstruction;
        //RestorePopup.OnSuccessRestore += SuccessRestore;
        //RestorePopup.OnEmptyData += CreateNewUser;
        //RestorePopup.OnExit += MuteSound;

        TapToStartButton.onClick.AddListener(OnClickTapToStartButton);
    }


    private void OnDisable()
    {
        LoginManager.OnBannedPlayFabAccount -= OnBanned;
        LoginManager.OnInternetNotReachable -= ShowNoInternetPopup;
        LoginManager.OnFinishPlayFabLoginProcess -= HideLoginMethodPopup;
        LoginManager.OnFinishPlayFabLoginProcess -= GetPlayFabServerTime;
        ClockManager.OnSuccessSetValidClockAfterLogin -= CompareHeader;
        ServerHeaderManager.OnEmptyServerRecord -= CreateNewUser;
        ServerHeaderManager.OnForceOverwrite -= ForceOverwrite;
        ServerHeaderManager.OnDifferPlayerId -= AskRestore;
        ServerHeaderManager.OnDifferDeviceId -= AskRestore;
        ServerHeaderManager.OnNext -= OnValidHeader;
        //PrologueDisplay.OnFinish -= GoToMainScene;
        PopupForceOverwriter.OnFinishOverwrite -= SetDataReady;
        PlayFabServerTitleDataReceiver.OnRefresh -= SetTitleDataReady;
        ServerStateChecker.OnServerUnderConstruction -= UnderConstruction;
        //RestorePopup.OnSuccessRestore -= SuccessRestore;
        //RestorePopup.OnEmptyData -= CreateNewUser;
        //RestorePopup.OnExit -= MuteSound;

        TapToStartButton.onClick.RemoveListener(OnClickTapToStartButton);
    }
    private IEnumerator Start()
    {
        // Tap to Start 버튼 숨김
        TapToStartButton.gameObject.SetActive(false);
        LoadingBar.SetActive(false);

        yield return null;

        BgmSound.Play();

        // 인터넷 상태 점검
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            PopupNoInternet.SetActive(true);
        }

        while (Application.internetReachability == NetworkReachability.NotReachable)
        {
            yield return null;
        }

        if (PopupNoInternet.activeSelf == true)
        {
            PopupNoInternet.SetActive(false);
        }

        LoadingBar.SetActive(true);
        OnProgress(0f);

        // 지난 로그인 관련 상태 읽기

        bool isTestServer = PlayFabSettings.staticSettings.TitleId == TestServerId;
        bool isUnityEditor = false;

#if UNITY_EDITOR
        isUnityEditor = true;
#endif
        string lastLoginMethod = PlayerPrefs.GetString("KIKI_LAST_LOGIN_METHOD", "");
        if (isUnityEditor == true || isTestServer == true)
        {
            PopupTesterLogin.SetActive(true);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log(lastLoginMethod); // 그냥 기록 남겨둠
            if (lastLoginMethod == "GUEST")
            {
                LoginManager.LoginGuest();
            }
            else if (lastLoginMethod == "GPGS")
            {

            }
            else if (lastLoginMethod == string.Empty)
            {
                PopupPrivatePolicy.gameObject.SetActive(true);
            }
#endif

            if (lastLoginMethod == "GPGS")
            {

            }
            else if (lastLoginMethod == string.Empty)
            {
                PopupPrivatePolicy.gameObject.SetActive(true);
            }
        }
        float randomHalf = UnityEngine.Random.Range(0.3f, 0.4f);
        float t = 0;
        float fakeDuration = 1.5f;

        // 데이터 및 시간 세팅 대기
        while (onCompleteTimeSetting == false || onCompleteDataLoading == false)
        {
            t += Time.deltaTime / fakeDuration;
            if (t > 1) t = 1f;
            OnProgress(t * randomHalf);
            yield return null;
        }

        OnReadyToLoadMainScene?.Invoke();

        while (onCompleteTitleData == false)
        {
            yield return null;
        }

        OnProgress(randomHalf);

        mainSceneLoadOperation = SceneManager.LoadSceneAsync("MainScene");
        mainSceneLoadOperation.allowSceneActivation = false;

        while (mainSceneLoadOperation.progress < 0.9f)
        {
            OnProgress(randomHalf + (0.9f - randomHalf) * Mathf.Clamp01(mainSceneLoadOperation.progress / 0.9f));
            yield return null;
        }

        OnProgress(1f);
        //LoadingBarAnimator.enabled = false;
        //LoadingBarIcon.sprite = LoadingFinishedSprite;
        //LoadingBarIcon.transform.DOPunchScale(Vector3.one * 0.1f, 0.8f);
        //LoadingUIEffect.SetActive(true);


        yield return new WaitForSeconds(1.5f);

        //데이터 & 시간 준비되면 넘어갈 수 있도록 만들기
        LoadingBar.SetActive(false);
        TapToStartButton.gameObject.SetActive(true);
    }


    // 플레이팹 로그인 성공 -> 서버 시간 요청하기
    private void GetPlayFabServerTime()
    {
        ClockManager.StartRequest();
    }

    // 로그인 성공 시 로그인 선택 팝업 닫기
    private void HideLoginMethodPopup()
    {
        if (PopupLoginMethod.activeSelf == true)
        {
            PopupLoginMethod.SetActive(false);
        }
    }

    // 시간 받아오기 성공 -> 서버 헤더 받아오라고 요청
    private void CompareHeader()
    {
        SetTimeReady();
        HeaderDataManager.CompareHeader();
    }

    // 신규 유저로 생성
    private void CreateNewUser()
    {
        print("신규 유저로 확인됨. 서버헤더가 없는 경우");
        GameDataManager.ResetPlayerData();
        randomIdCreator.Create();
    }

    // 정상 처리
    private void OnValidHeader()
    {
        //print("기존 유저로 확인됨. 서버 헤더와 기기 일치");

        //print("^^ 서버데이터는 문제가 없음. 클라이언트 데이터와 비교 해보려고 함");
        if (ClientDataReader.TryLoadClientData(out PlayerData playerData) == true)
        {
            //print("^^ 클라이언트 데이터 정상적으로 있음 -> 클라이언트 데이터 사용");
            // 클라이언트 데이터 있는 경우에는 클라이언트 데이터 사용;
            GameDataManager.OverWritePlayerData(playerData);
            SetDataReady();
        }
        else
        {
            //print("^^ 클라이언트 데이터에 문제가 있음 -> 복원 요청");
            // 헤더는 멀쩡한데 클라이언트 데이터가 없다면, 서버 데이터로 복원여부 묻기
            AskRestore();
        }
    }

    //
    private void ForceOverwrite()
    {
        print("강제 덮어쓰기 시작");
        PopupOverwriteSpinner.SetActive(true);
    }

    // 복원 처리
    private void AskRestore()
    {
        print("복원 여부 확인 필요. 서버헤더와 불일치 하거나, 클라이언트 데이터만 없음");
        //RestorePopup.gameObject.SetActive(true);
    }

    // 차단 처리
    private void OnBanned(string customData)
    {
        print("차단 유저로 확인됨");
        //BanPopup.gameObject.SetActive(true);
    }


    // 임시 아이디 생성 하기
    private void OnSuccessUpdateRandomDisplayName(string displayName)
    {
        OnSuccessRegisterRandomDisplayName?.Invoke(displayName);
        ForceUploadNewUserPlayerData();
    }

    private void ForceUploadNewUserPlayerData()
    {
        print("신규 유저 데이터 업로드");
        PlayerDataUploader.OnSuccessUploadAll += OnSuccessForceUploadNewUserData;
        ClientDataToServer.ForceUploadAll();
    }

    private void OnSuccessForceUploadNewUserData()
    {
        PlayerDataUploader.OnSuccessUploadAll -= OnSuccessForceUploadNewUserData;
        print("신규 유저 데이터 -> 서버에 업로드 완료");
        ClientSaveManager.ForceSaveAll();
        print("신규 유저 데이터 -> 클라이언트에 초기 데이터 저장 완료");
        SetDataReady();
    }

    // 복원팝업에서 복원하기 성공
    private void SuccessRestore()
    {
        SetDataReady();
        ClientDataToServer.ForceUploadAll();
    }

    // 로그인 및 서버 시간 받아오기 성공
    private void SetTimeReady() => onCompleteTimeSetting = true;

    // 어떤 데이터(클라이언트or서버)로 플레이할지 결정 완료
    private void SetDataReady()
    {
        RegisterDataToSaveManager(PlayFabSettings.staticPlayer.EntityId);
        ClientSaveManager.ForceSaveAll();

        if (BlockSpinner.activeSelf == true)
        {
            BlockSpinner.SetActive(false);
        }

        onCompleteDataLoading = true;
    }

    // 서버 점검 중
    private void UnderConstruction(string message)
    {
        StopAllCoroutines();
        PopupServerUnderConstruction.SetActive(true);
    }

    private void OnClickTapToStartButton()
    {
        TapToStartButton.gameObject.SetActive(false);
        StopAllCoroutines();

        //bool prologue = Player.Value.IsPrologue;
        //if (prologue == true)
        //{
        //    StartCoroutine(FadeOutAndActiveMainScene());
        //}
        //else
        //{
        //    Player.Value.SetPrologue(true);
        //    Prologue.gameObject.SetActive(true);
        //    Prologue.StartPrologue();
        //}
    }

    private IEnumerator FadeOutAndActiveMainScene()
    {
        //MuteSound();

        //FadeOutScene.gameObject.SetActive(true);
        float fadeTime = 1f; //FadeOutScene.StartFadeOut();

        yield return new WaitForSeconds(fadeTime);

        yield return StartCoroutine(ActiveMainScene());
    }

    private void GoToMainScene()
    {
        StartCoroutine(ActiveMainScene());
    }

    private IEnumerator ActiveMainScene()
    {
        while (mainSceneLoadOperation.progress < 0.9f)
        {
            yield return null;
        }

        mainSceneLoadOperation.allowSceneActivation = true;
    }


    private void OnProgress(float progress)
    {
        LoadingFill.fillAmount = progress;
        //LoadingBarIcon.transform.localPosition = new Vector2(loadingbarLength * progress - loadingbarHalfLength, 2);
        ProgressText.text = (int)(progress * 100) + "%";
    }


    private void ShowNoInternetPopup()
    {
        PopupNoInternet.SetActive(true);
    }


    private void SetTitleDataReady()
    {
        onCompleteTitleData = true;
    }

private void RegisterDataToSaveManager(string playerID)
    {
        print("자동 저장하도록 등록 완료");

        ClientSaveManager.Add($"SAVE_KEY_VALUE_{playerID}", Player.Value);

        // 추후 데이터가 많아지면 여기에 추가 등록

        ClientSaveManager.Run();
    }
}

