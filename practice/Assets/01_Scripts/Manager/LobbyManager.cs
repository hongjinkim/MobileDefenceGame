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
        // Tap to Start ��ư ����
        TapToStartButton.gameObject.SetActive(false);
        LoadingBar.SetActive(false);

        yield return null;

        BgmSound.Play();

        // ���ͳ� ���� ����
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

        // ���� �α��� ���� ���� �б�

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
            Debug.Log(lastLoginMethod); // �׳� ��� ���ܵ�
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

        // ������ �� �ð� ���� ���
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

        //������ & �ð� �غ�Ǹ� �Ѿ �� �ֵ��� �����
        LoadingBar.SetActive(false);
        TapToStartButton.gameObject.SetActive(true);
    }


    // �÷����� �α��� ���� -> ���� �ð� ��û�ϱ�
    private void GetPlayFabServerTime()
    {
        ClockManager.StartRequest();
    }

    // �α��� ���� �� �α��� ���� �˾� �ݱ�
    private void HideLoginMethodPopup()
    {
        if (PopupLoginMethod.activeSelf == true)
        {
            PopupLoginMethod.SetActive(false);
        }
    }

    // �ð� �޾ƿ��� ���� -> ���� ��� �޾ƿ���� ��û
    private void CompareHeader()
    {
        SetTimeReady();
        HeaderDataManager.CompareHeader();
    }

    // �ű� ������ ����
    private void CreateNewUser()
    {
        print("�ű� ������ Ȯ�ε�. ��������� ���� ���");
        GameDataManager.ResetPlayerData();
        randomIdCreator.Create();
    }

    // ���� ó��
    private void OnValidHeader()
    {
        //print("���� ������ Ȯ�ε�. ���� ����� ��� ��ġ");

        //print("^^ ���������ʹ� ������ ����. Ŭ���̾�Ʈ �����Ϳ� �� �غ����� ��");
        if (ClientDataReader.TryLoadClientData(out PlayerData playerData) == true)
        {
            //print("^^ Ŭ���̾�Ʈ ������ ���������� ���� -> Ŭ���̾�Ʈ ������ ���");
            // Ŭ���̾�Ʈ ������ �ִ� ��쿡�� Ŭ���̾�Ʈ ������ ���;
            GameDataManager.OverWritePlayerData(playerData);
            SetDataReady();
        }
        else
        {
            //print("^^ Ŭ���̾�Ʈ �����Ϳ� ������ ���� -> ���� ��û");
            // ����� �����ѵ� Ŭ���̾�Ʈ �����Ͱ� ���ٸ�, ���� �����ͷ� �������� ����
            AskRestore();
        }
    }

    //
    private void ForceOverwrite()
    {
        print("���� ����� ����");
        PopupOverwriteSpinner.SetActive(true);
    }

    // ���� ó��
    private void AskRestore()
    {
        print("���� ���� Ȯ�� �ʿ�. ��������� ����ġ �ϰų�, Ŭ���̾�Ʈ �����͸� ����");
        //RestorePopup.gameObject.SetActive(true);
    }

    // ���� ó��
    private void OnBanned(string customData)
    {
        print("���� ������ Ȯ�ε�");
        //BanPopup.gameObject.SetActive(true);
    }


    // �ӽ� ���̵� ���� �ϱ�
    private void OnSuccessUpdateRandomDisplayName(string displayName)
    {
        OnSuccessRegisterRandomDisplayName?.Invoke(displayName);
        ForceUploadNewUserPlayerData();
    }

    private void ForceUploadNewUserPlayerData()
    {
        print("�ű� ���� ������ ���ε�");
        PlayerDataUploader.OnSuccessUploadAll += OnSuccessForceUploadNewUserData;
        ClientDataToServer.ForceUploadAll();
    }

    private void OnSuccessForceUploadNewUserData()
    {
        PlayerDataUploader.OnSuccessUploadAll -= OnSuccessForceUploadNewUserData;
        print("�ű� ���� ������ -> ������ ���ε� �Ϸ�");
        ClientSaveManager.ForceSaveAll();
        print("�ű� ���� ������ -> Ŭ���̾�Ʈ�� �ʱ� ������ ���� �Ϸ�");
        SetDataReady();
    }

    // �����˾����� �����ϱ� ����
    private void SuccessRestore()
    {
        SetDataReady();
        ClientDataToServer.ForceUploadAll();
    }

    // �α��� �� ���� �ð� �޾ƿ��� ����
    private void SetTimeReady() => onCompleteTimeSetting = true;

    // � ������(Ŭ���̾�Ʈor����)�� �÷������� ���� �Ϸ�
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

    // ���� ���� ��
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
        print("�ڵ� �����ϵ��� ��� �Ϸ�");

        ClientSaveManager.Add($"SAVE_KEY_VALUE_{playerID}", Player.Value);

        // ���� �����Ͱ� �������� ���⿡ �߰� ���

        ClientSaveManager.Run();
    }
}

