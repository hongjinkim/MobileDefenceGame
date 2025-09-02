using TMPro;
using UnityEngine;
using PlayFab;

#if UNITY_EDITOR
using UnityEditor;
#endif



[ExecuteInEditMode]
public class BuildController : MonoBehaviour
{
    public static BuildController Instance = null;

    [Header("���弼��")]
    [SerializeField] private bool ����ײ���;
    [SerializeField] private bool ���̺꼭��;
    [SerializeField] private bool AAB����;
    [SerializeField] private string ��������;
    [SerializeField] private int �������;


    [SerializeField] private GameObject DEBUG;
    [SerializeField] private GameObject VersionCheck;
    [SerializeField] private TextMeshProUGUI VersionCheck_Test;

    [SerializeField] private string ServerTitleId_Live;
    [SerializeField] private string ServerTitleId_Test;
    [SerializeField] private TextMeshProUGUI ServerText_Test;
    [SerializeField] private TextMeshProUGUI ServerText_Live;

    private string AppVersion_pre;
    private int BundleCode_pre;
    private bool AABbuild_pre;

    private const string DEBUG_ON = "DEBUG_ON";
    private const string DEBUG_OFF = "DEBUG_OFF";

    public bool GetLiveServer => ���̺꼭��;
    public string GetAppVersion => ��������;

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

    //��������� ������ ����� ��� (������ ����)
    void EditorUpdate()
    {
        if (Application.isPlaying) { return; }

        DebugSetting();
        ServerSetting();
        AABbuildSetting();
        AppVersionSetting();
        BundleVersionSetting();
    }

    //��������� ������ ���ſﶧ ��� (�������� ����)
    private void OnValidate()
    {
        if (Application.isPlaying) { return; }

        PreprocessorSetting();
    }


    void DebugSetting()
    {
        DEBUG.SetActive(!����ײ���);
    }

    void ServerSetting()
    {
        //PlayFabSettings.staticSettings.TitleId = ���̺꼭�� ? ServerTitleId_Live : ServerTitleId_Test;
        //ServerText_Test.enabled = !���̺꼭��;
        //ServerText_Live.enabled = ���̺꼭��;

        ////����üũ����� ���̺꼭�� ���ο� ���ӽ�Ŵ
        //VersionCheck.SetActive(���̺꼭��);
        //VersionCheck_Test.enabled = ���̺꼭��;
    }

    void AABbuildSetting()
    {
        //PlayerSetting���� ������ ���
        if (AABbuild_pre.Equals(EditorUserBuildSettings.buildAppBundle) == false)
        {
            AAB���� = EditorUserBuildSettings.buildAppBundle;
            AABbuild_pre = EditorUserBuildSettings.buildAppBundle;
        }
        //Inspector���� ������ ���
        else if (AABbuild_pre.Equals(AAB����) == false)
        {
            EditorUserBuildSettings.buildAppBundle = AAB����;
            AABbuild_pre = AAB����;

            RepaintWindow("UnityEditor.BuildPlayerWindow");
        }
    }

    void AppVersionSetting()
    {
        //PlayerSetting���� ������ ���
        if (AppVersion_pre.Equals(PlayerSettings.bundleVersion) == false)
        {
            �������� = PlayerSettings.bundleVersion;
            AppVersion_pre = PlayerSettings.bundleVersion;
        }
        //Inspector���� ������ ���
        else if (AppVersion_pre.Equals(��������) == false)
        {
            PlayerSettings.bundleVersion = ��������;
            AppVersion_pre = ��������;
        }
    }

    void BundleVersionSetting()
    {
        //PlayerSetting���� ������ ���
        if (BundleCode_pre.Equals(PlayerSettings.Android.bundleVersionCode) == false)
        {
            ������� = PlayerSettings.Android.bundleVersionCode;
            BundleCode_pre = PlayerSettings.Android.bundleVersionCode;
        }

        //Inspector���� ������ ���
        else if (BundleCode_pre.Equals(�������) == false)
        {
            PlayerSettings.Android.bundleVersionCode = �������;
            BundleCode_pre = �������;
        }
    }

    void PreprocessorSetting()
    {
        BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        string currentSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
        string newSymbols;

        if (!����ײ���)
        {
            if (currentSymbols.Contains(DEBUG_ON) == true) { return; } //�̹� On�̸� return

            if (currentSymbols.Contains(DEBUG_OFF) == false) { newSymbols = currentSymbols + DEBUG_ON + ";"; }
            else { newSymbols = currentSymbols.Replace(DEBUG_OFF, DEBUG_ON); }
        }
        else
        {
            if (currentSymbols.Contains(DEBUG_OFF) == true) { return; } //�̹� off�̸� return

            if (currentSymbols.Contains(DEBUG_ON) == false) { newSymbols = currentSymbols + DEBUG_OFF + ";"; }
            else { newSymbols = currentSymbols.Replace(DEBUG_ON, DEBUG_OFF); }
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, newSymbols);
        AssetDatabase.Refresh();
    }


    //Window repaint�ϴ� �޼���
    //string�� �����Ҷ� ���� typing cursor�� �����Ǿ� ������ �����ϱ� ������, �ܼ��� bool����ø� repaint�� Ȱ���ϸ� ������
    void RepaintWindow(string windowType)
    {
        EditorWindow focusedWindow = EditorWindow.focusedWindow; //���� focus�� â
        bool isOpen = false;
        var windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
        foreach (var window in windows)
        {
            if (window.GetType().ToString() == windowType) { isOpen = true; }
        }

        //â�� �����ִ� ���¶�� ����
        if (isOpen)
        {
            EditorWindow buildSettingsWindow = EditorWindow.GetWindow(typeof(EditorWindow).Assembly.GetType(windowType));
            if (buildSettingsWindow != null) { buildSettingsWindow.Repaint(); }
            if (focusedWindow != null) { focusedWindow.Focus(); }
        }
    }

#endif
}

