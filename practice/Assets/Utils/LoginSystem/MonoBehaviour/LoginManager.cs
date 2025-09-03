using System;
using UnityEngine;


public class LoginManager : MonoBehaviour
{
    public static string PlayerMasterId => PlayFab.PlayFabSettings.staticPlayer.PlayFabId;

    public static string PlayerTitleId => PlayFab.PlayFabSettings.staticPlayer.EntityId;


    // 플래이팹 로그인을 성공한 Player Title Id 값을 전달 (캐싱 목적)
    public static Action<string> OnAuthenticatedPlayerTitleId;

    // 플래이팹 로그인 처리가 완료되었을 때 (로그인 이후 프로세스로 진행하도록 연결 필수)
    public static Action OnFinishPlayFabLoginProcess;

    // 플레이팹에 로그인 했으나 차단처리(Banned)된 유저일 때(아무것도 연결하지 않으면 즉시 앱 종료)
    public static Action<string> OnBannedPlayFabAccount;

    // 로그인 프로세스가 진행 중에 로그인을 재시도할 때(아무것도 연결하지 않으면 아무것도 안함)
    public static Action OnPendingLoginProcess;

    // 인터넷 연결이 안되는 상태일 때(아무것도 연결하지 않으면 즉시 앱 종료)
    public static Action OnInternetNotReachable;

    // 구글플레이서비스를 아예 사용할 수 없는 상태일 때
    public static Action OnGooglePlayGameServiceUnavailable;

    // 구글플레이서비스 계정연동을 승인하지 않았을 때(아무것도 연결하지 않으면 즉시 앱 종료)
    public static Action OnDeniedGooglePlayServiceTerms;

    // 디버그 빌드용 : 빌드에서 자동로그인을 강제 차단한 경우
    public static Action OnForcedIgnoreAutoLogin;

    // 형식 오류 : 이메일 형식이 아님
    public static Action OnFilteredNotEmailFormat;

    // 형식 오류 : 잘못된 비밀번호 형식
    public static Action OnFilteredBadPassword;

    // 형식 오류 : 잘못된 닉네임
    public static Action OnFilteredBadUsername;

    // 형식 오류 : 너무 짧은 비밀번호
    public static Action OnFilteredTooShortPassword;

    // 형식 오류 : 너무 긴 비밀번호
    public static Action OnFilteredTooLongPassword;

    // 형식 오류 : 너무 짧은 닉네임
    public static Action OnFilteredTooShortUsername;

    // 형식 오류 : 너무 긴 닉네임
    public static Action OnFilteredTooLongUsername;



    // -------

    public static LoginManager Instance { get; private set; } = null;


    [SerializeField] private GameObject PoomDebugConsole;

    [SerializeField] private PlayFabSharedSettings PlayFabSettings;

    public PlayFabEmailLoginModule Email { get; private set; } = null;
#if UNITY_EDITOR
    public PlayFabGuestLoginModule Guest { get; private set; } = null;
#endif
    private PlayFabGoogleLoginModule GoogleToPlayFab { get; set; } = null;


    private bool isInitialized = false;


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

    //private void Start()
    //{
    //    string liveServerTitleId = "B83BB";

    //    if (PlayFabSettings.TitleId == liveServerTitleId)
    //    {
    //        if (PoomDebugConsole.activeSelf == false)
    //        {
    //            LoginWithGooglePlayService();
    //        }
    //    }
    //}


    public void LoginWithGooglePlayService()
    {
        //print($"※ 구글 로그인");

        //if (isInitialized == false) Initialize();

        //Google.Login();
    }

    public void LoginWithEmail(string email, string password, string username)
    {
        print($"※ 테스터 로그인: {email}");

        if (isInitialized == false) Initialize();

        Email.Login(email, password, username);
    }
#if UNITY_EDITOR
    public void LoginGuest()
    {
        print($"※ 게스트 로그인: {SystemInfo.deviceUniqueIdentifier}");

        if (isInitialized == false) Initialize();

        Guest.Login();
    }
#endif

    private void Initialize()
    {
        //Google = new GoogleSocialLoginModule();
        //Google.ResponseSuccess += OnSuccessGoogleSocialLogin;
        //Google.ResponseError += OnError;

        //GoogleToPlayFab = new PlayFabGoogleLoginModule();
        //GoogleToPlayFab.ResponseSuccess += OnSuccess;
        //GoogleToPlayFab.ResponseError += OnError;

        Email = new PlayFabEmailLoginModule();
        Email.ResponseSuccess += OnSuccess;
        Email.ResponseError += OnError;
#if UNITY_EDITOR
        Guest = new PlayFabGuestLoginModule();
        Guest.ResponseSuccess += OnSuccess;
        Guest.ResponseError += OnError;
#endif
        isInitialized = true;
    }

    //private void OnSuccessGoogleSocialLogin()
    //    => GoogleToPlayFab.LoginWithGooglePlayGamesService();

    private void OnSuccess(string playerTitleId, string loginMethodName)
    {
        PlayerPrefs.SetString("KIKI_LAST_LOGIN_METHOD", loginMethodName);

#if UNITY_EDITOR
        Debug.Log($"<{GetType()}> 로그인 성공. PlayFab Player Title ID : {playerTitleId}");
#endif
        OnAuthenticatedPlayerTitleId?.Invoke(playerTitleId);
        OnFinishPlayFabLoginProcess?.Invoke();
    }

    private void OnError(LoginResponse response)
    {
        Debug.Log("################ Error - " + response + " / " + response.Status);

        if (response.Status == LoginError.GoogleSocialLoginFailed)
        {
            Debug.LogError($"<{GetType()}> {response.Message}");
            OnGooglePlayGameServiceUnavailable?.Invoke();
        }
        else if (response.Status == LoginError.GooglePlayServiceDenied)
        {
            if (OnDeniedGooglePlayServiceTerms == null) { ForceQuitGame(); }
            else { OnDeniedGooglePlayServiceTerms?.Invoke(); }
        }
        else if (response.Status == LoginError.AccountBanned)
        {
            if (OnBannedPlayFabAccount == null) { ForceQuitGame(); }
            else { OnBannedPlayFabAccount?.Invoke(response.Message); }
        }
        else if (response.Status == LoginError.InternetNotReachable)
        {
            if (OnInternetNotReachable == null) { ForceQuitGame(); }
            else { OnInternetNotReachable?.Invoke(); }
        }
        else if (response.Status == LoginError.OnPending)
        {
            OnPendingLoginProcess?.Invoke();
        }
        else if (response.Status == LoginError.NotEmailFormat)
        {
            OnFilteredNotEmailFormat?.Invoke();
        }
        else if (response.Status == LoginError.BadPassword)
        {
            OnFilteredBadPassword?.Invoke();
        }
        else if (response.Status == LoginError.BadUsername)
        {
            OnFilteredBadUsername?.Invoke();
        }
        else if (response.Status == LoginError.TooShortPassword)
        {
            OnFilteredTooShortPassword?.Invoke();
        }
        else if (response.Status == LoginError.TooLongPassword)
        {
            OnFilteredTooLongPassword?.Invoke();
        }
        else if (response.Status == LoginError.TooShortUserName)
        {
            OnFilteredTooShortUsername?.Invoke();
        }
        else if (response.Status == LoginError.TooLongUserName)
        {
            OnFilteredTooLongUsername?.Invoke();
        }
        else if (string.IsNullOrWhiteSpace(response.Message) == true)
        {
            Debug.LogError(response.Message);
        }
        else
        {
            Debug.LogError($"<{GetType()}> Unknown Error. {response.Message}");
        }
    }

    private void ForceQuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}

