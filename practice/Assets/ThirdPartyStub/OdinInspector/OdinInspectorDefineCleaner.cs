#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class OdinInspectorDefineCleaner
{
    static OdinInspectorDefineCleaner()
    {
        // 프로젝트에 Odin Inspector 폴더가 존재하는지 체크
        string[] possiblePaths = {
            "Assets/Sirenix/",
            "Assets/Plugins/Sirenix/"
        };

        bool odinExists = false;
        foreach (var path in possiblePaths)
        {
            if (Directory.Exists(path))
            {
                odinExists = true;
                break;
            }
        }

        // ODIN_INSPECTOR define 상태 확인
        var targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

        if (!odinExists && defines.Contains("ODIN_INSPECTOR"))
        {
            // 자동으로 define 제거
            defines = string.Join(";", defines.Split(';').Where(d => d != "ODIN_INSPECTOR"));
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
            Debug.Log("<color=orange>Odin Inspector가 프로젝트에 없어서 ODIN_INSPECTOR define을 자동 제거했습니다.</color>");
        }
    }
}
#endif