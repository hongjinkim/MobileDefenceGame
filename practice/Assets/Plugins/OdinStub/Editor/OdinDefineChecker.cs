#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

[InitializeOnLoad]
public static class OdinDefineChecker
{
    private static readonly string[] odinFiles =
    {
        "Assets/Plugins/Sirenix/OdinInspector/Editor/Sirenix.OdinInspector.Editor.dll",
        "Assets/Plugins/Sirenix/OdinInspector/Sirenix.OdinInspector.Attributes.dll",
        "Packages/com.sirenix.odininspector/Sirenix.OdinInspector.Attributes.dll"
    };

    static OdinDefineChecker()
    {
        // Unity 초기화 후 실행
        EditorApplication.update += CheckAndApply;
    }

    private static void CheckAndApply()
    {
        EditorApplication.update -= CheckAndApply;

        bool odinExists = odinFiles.Any(File.Exists);

        string symbol = "ODIN_INSPECTOR";
        var targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        var currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

        if (odinExists && !currentDefines.Contains(symbol))
        {
            DefineUtils.AddDefineSymbol(symbol);
            Debug.Log("[OdinStub] ODIN_INSPECTOR define added.");
            AssetDatabase.Refresh(); // 강제 재컴파일
        }
        else if (!odinExists && currentDefines.Contains(symbol))
        {
            DefineUtils.RemoveDefineSymbol(symbol);
            Debug.Log("[OdinStub] ODIN_INSPECTOR define removed.");
            AssetDatabase.Refresh();
        }
    }
}
#endif
