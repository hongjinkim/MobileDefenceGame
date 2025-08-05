#if UNITY_EDITOR
using UnityEditor;

public static class DefineConfig
{
    // 플랫폼별로 필요한 Define 목록
    private static readonly string[] REQUIRED_DEFINES = new[]
    {
        "ODIN_INSPECTOR",
        "DEBUG_ON"
    };

    /// <summary>
    /// 특정 빌드 타겟 그룹에 define을 적용합니다.
    /// </summary>
    public static void ApplyDefines(BuildTargetGroup group)
    {
        string currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
        var defineList = new System.Collections.Generic.HashSet<string>(
            currentDefines.Split(';'),
            System.StringComparer.OrdinalIgnoreCase
        );

        bool changed = false;

        foreach (var def in REQUIRED_DEFINES)
        {
            if (!defineList.Contains(def))
            {
                defineList.Add(def);
                changed = true;
            }
        }

        if (changed)
        {
            string updatedDefines = string.Join(";", defineList);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, updatedDefines);
            UnityEngine.Debug.Log($"[DefineConfig] Updated defines for {group}: {updatedDefines}");
        }
    }

    /// <summary>
    /// 현재 사용 중인 모든 주요 타겟 그룹에 define 적용
    /// </summary>
    [MenuItem("Tools/Apply Define Symbols")]
    public static void ApplyToAllTargets()
    {
        ApplyDefines(BuildTargetGroup.Standalone); // Windows, Mac, Linux
        ApplyDefines(BuildTargetGroup.Android);
        ApplyDefines(BuildTargetGroup.iOS);
        // 필요한 타겟 추가 가능
    }
}
#endif
