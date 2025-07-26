#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

[InitializeOnLoad]
public static class AssetController
{
    const string ODIN_INSPECTOR = "ODIN_INSPECTOR";
    const string ODIN_INSPECTOR_OFF = "ODIN_INSPECTOR_OFF";

    static AssetController()
    {
        // Unity가 시작(프로젝트 열림)시 자동 검사 및 동기화
        CheckAndSetOdinDefine();
    }

    /// <summary>
    /// 에디터 메뉴로 언제든 동기화 가능
    /// </summary>
    [MenuItem("Tools/Odin/동기화: 에셋 유무에 따라 Define 자동 정리")]
    public static void ManualCheckAndSetOdinDefine()
    {
        CheckAndSetOdinDefine();
    }

    /// <summary>
    /// 실제 정의 체크 및 Scripting Define Symbols 갱신
    /// </summary>
    public static void CheckAndSetOdinDefine()
    {
        bool odinExists =
            Directory.Exists("Assets/Sirenix") ||
            Directory.Exists("Assets/Plugins/Sirenix");

        BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        string currentSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

        // 세미콜론으로 split, 중복/공백 제거, Odin 관련 define만 정리
        var symbolList = new List<string>();
        foreach (var s in currentSymbols.Split(';'))
        {
            var trimmed = s.Trim();
            if (!string.IsNullOrEmpty(trimmed) &&
                trimmed != ODIN_INSPECTOR &&
                trimmed != ODIN_INSPECTOR_OFF)
            {
                symbolList.Add(trimmed);
            }
        }

        bool changed = false;

        if (odinExists)
        {
            // 에셋 있으면 ON
            if (!symbolList.Contains(ODIN_INSPECTOR))
            {
                symbolList.Add(ODIN_INSPECTOR);
                changed = true;
                Debug.Log("<color=cyan>Odin Inspector/Serializer 에셋이 있음: ODIN_INSPECTOR define 활성화</color>");
            }
        }
        else
        {
            // 에셋 없으면 OFF
            if (!symbolList.Contains(ODIN_INSPECTOR_OFF))
            {
                symbolList.Add(ODIN_INSPECTOR_OFF);
                changed = true;
                Debug.Log("<color=magenta>Odin Inspector/Serializer 에셋이 없음: ODIN_INSPECTOR_OFF define 활성화</color>");
            }
        }

        string newSymbols = string.Join(";", symbolList) + ";";
        if (changed && newSymbols != currentSymbols)
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, newSymbols);
            AssetDatabase.Refresh();
            Debug.Log($"<color=yellow>Odin Define 동기화 완료: {newSymbols}</color>");
        }
    }
}
#endif
