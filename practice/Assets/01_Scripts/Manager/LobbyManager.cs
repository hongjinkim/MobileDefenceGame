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
    public LoginManager LoginManager;
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


    //private RandomUsernameUploader randomIdCreator;
    private bool onCompleteDataLoading = false;
    private bool onCompleteTimeSetting = false;
    private bool onCompleteTitleData = false;
    private AsyncOperation mainSceneLoadOperation = null;

    private PlayerData Player => GameDataManager.PlayerData;

    private void RegisterDataToSaveManager(string playerID)
    {
        print("자동 저장하도록 등록 완료");

        ClientSaveManager.Add($"SAVE_KEY_VALUE_{playerID}", Player.Value);

        // 추후 데이터가 많아지면 여기에 추가 등록

        ClientSaveManager.Run();
    }
}

