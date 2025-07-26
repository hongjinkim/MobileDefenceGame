using TMPro;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif



[ExecuteInEditMode]
public class BuildController : MonoBehaviour
{
    public static BuildController Instance = null;

    [Header("빌드세팅")]
    [SerializeField] private bool 디버그끄기;
    //[SerializeField] private bool 라이브서버;
    //[SerializeField] private bool AAB빌드;
    //[SerializeField] private string 배포버전;
    //[SerializeField] private int 번들버전;


    //수정이 필요할때 HideInInspector를 없애고 링크를 걸면 됨
    [SerializeField] private GameObject DEBUG;
    //[SerializeField] private GameObject VersionCheck;
    //[SerializeField] private TextMeshProUGUI VersionCheck_Test;

    //[SerializeField] private string ServerTitleId_Live;
    //[SerializeField] private string ServerTitleId_Test;
    //[SerializeField] private TextMeshProUGUI ServerText_Test;
    //[SerializeField] private TextMeshProUGUI ServerText_Live;

    private string AppVersion_pre;
    private int BundleCode_pre;
    private bool AABbuild_pre;

    private const string DEBUG_ON = "DEBUG_ON";
    private const string DEBUG_OFF = "DEBUG_OFF";

    //public bool GetLiveServer => 라이브서버;
    //public string GetAppVersion => 배포버전;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else if (Instance != this) { Destroy(gameObject); }
#if !UNITY_EDITOR
        VersionCheck_Test.enabled = false;
#endif
    }


#if UNITY_EDITOR
    void OnEnable()
    {
        EditorApplication.update += EditorUpdate;

        BundleCode_pre = PlayerSettings.Android.bundleVersionCode;
        AppVersion_pre = PlayerSettings.bundleVersion;
        AABbuild_pre = EditorUserBuildSettings.buildAppBundle;
    }

    void OnDisable()
    {
        EditorApplication.update -= EditorUpdate;
    }

    //상대적으로 구동이 가벼울떄 사용 (양방제어도 가능)
    void EditorUpdate()
    {
        if (Application.isPlaying) { return; }

        DebugSetting();
        ServerSetting();
        AABbuildSetting();
        AppVersionSetting();
        BundleVersionSetting();
    }

    //상대적으로 구동이 무거울때 사용 (양방제어는 포기)
    private void OnValidate()
    {
        if (Application.isPlaying) { return; }

        PreprocessorSetting();
    }


    void DebugSetting()
    {
        DEBUG.SetActive(!디버그끄기);
    }

    void ServerSetting()
    {
        //PlayFabSettings.staticSettings.TitleId = 라이브서버 ? ServerTitleId_Live : ServerTitleId_Test;
        //ServerText_Test.enabled = !라이브서버;
        //ServerText_Live.enabled = 라이브서버;

        ////버전체크기능을 라이브서버 여부에 종속시킴
        //VersionCheck.SetActive(라이브서버);
        //VersionCheck_Test.enabled = 라이브서버;
    }

    void AABbuildSetting()
    {
        ////PlayerSetting에서 변경한 경우
        //if (AABbuild_pre.Equals(EditorUserBuildSettings.buildAppBundle) == false)
        //{
        //    AAB빌드 = EditorUserBuildSettings.buildAppBundle;
        //    AABbuild_pre = EditorUserBuildSettings.buildAppBundle;
        //}
        ////Inspector에서 변경한 경우
        //else if (AABbuild_pre.Equals(AAB빌드) == false)
        //{
        //    EditorUserBuildSettings.buildAppBundle = AAB빌드;
        //    AABbuild_pre = AAB빌드;

        //    RepaintWindow("UnityEditor.BuildPlayerWindow");
        //}
    }

    void AppVersionSetting()
    {
        ////PlayerSetting에서 변경한 경우
        //if (AppVersion_pre.Equals(PlayerSettings.bundleVersion) == false)
        //{
        //    배포버전 = PlayerSettings.bundleVersion;
        //    AppVersion_pre = PlayerSettings.bundleVersion;
        //}
        ////Inspector에서 변경한 경우
        //else if (AppVersion_pre.Equals(배포버전) == false)
        //{
        //    PlayerSettings.bundleVersion = 배포버전;
        //    AppVersion_pre = 배포버전;
        //}
    }

    void BundleVersionSetting()
    {
        ////PlayerSetting에서 변경한 경우
        //if (BundleCode_pre.Equals(PlayerSettings.Android.bundleVersionCode) == false)
        //{
        //    번들버전 = PlayerSettings.Android.bundleVersionCode;
        //    BundleCode_pre = PlayerSettings.Android.bundleVersionCode;
        //}

        ////Inspector에서 변경한 경우
        //else if (BundleCode_pre.Equals(번들버전) == false)
        //{
        //    PlayerSettings.Android.bundleVersionCode = 번들버전;
        //    BundleCode_pre = 번들버전;
        //}
    }

    void PreprocessorSetting()
    {
        BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        string currentSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
        string newSymbols;

        if (!디버그끄기)
        {
            if (currentSymbols.Contains(DEBUG_ON) == true) { return; } //이미 On이면 return

            if (currentSymbols.Contains(DEBUG_OFF) == false) { newSymbols = currentSymbols + DEBUG_ON + ";"; }
            else { newSymbols = currentSymbols.Replace(DEBUG_OFF, DEBUG_ON); }
        }
        else
        {
            if (currentSymbols.Contains(DEBUG_OFF) == true) { return; } //이미 off이면 return

            if (currentSymbols.Contains(DEBUG_ON) == false) { newSymbols = currentSymbols + DEBUG_OFF + ";"; }
            else { newSymbols = currentSymbols.Replace(DEBUG_ON, DEBUG_OFF); }
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, newSymbols);
        AssetDatabase.Refresh();
    }


    //Window repaint하는 메서드
    //string값 변경할때 쓰면 typing cursor가 해제되어 오히려 불편하기 때문에, 단순한 bool변경시만 repaint에 활용하면 좋을듯
    void RepaintWindow(string windowType)
    {
        EditorWindow focusedWindow = EditorWindow.focusedWindow; //현재 focus된 창
        bool isOpen = false;
        var windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
        foreach (var window in windows)
        {
            if (window.GetType().ToString() == windowType) { isOpen = true; }
        }

        //창이 열려있는 상태라면 갱신
        if (isOpen)
        {
            EditorWindow buildSettingsWindow = EditorWindow.GetWindow(typeof(EditorWindow).Assembly.GetType(windowType));
            if (buildSettingsWindow != null) { buildSettingsWindow.Repaint(); }
            if (focusedWindow != null) { focusedWindow.Focus(); }
        }
    }

#endif
}

