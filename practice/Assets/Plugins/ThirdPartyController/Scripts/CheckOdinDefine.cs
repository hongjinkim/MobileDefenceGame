#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

[InitializeOnLoad]
public static class CheckOdinDefine
{
    private const string DefineSymbol = "ODIN_INSPECTOR";
    private const string OdinFolder = "Assets/Plugins/Sirenix";

    static CheckOdinDefine()
    {
        EditorApplication.delayCall += () =>
        {
            bool odinExists = Directory.Exists(OdinFolder);

            ApplyToAllBuildTargetGroups(odinExists);
        };
    }

    private static void ApplyToAllBuildTargetGroups(bool enableDefine)
    {
        foreach (BuildTargetGroup group in System.Enum.GetValues(typeof(BuildTargetGroup)))
        {
            if (group == BuildTargetGroup.Unknown) continue;

            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
            var defineList = defines.Split(';').Where(d => !string.IsNullOrWhiteSpace(d)).ToList();

            bool hasDefine = defineList.Contains(DefineSymbol);

            if (enableDefine && !hasDefine)
            {
                defineList.Add(DefineSymbol);
                SetDefines(group, defineList);
                Debug.Log($"[OdinDefine] ? '{DefineSymbol}' added to {group}");
            }
            else if (!enableDefine && hasDefine)
            {
                defineList.Remove(DefineSymbol);
                SetDefines(group, defineList);
                Debug.Log($"[OdinDefine] ? '{DefineSymbol}' removed from {group}");
            }
        }
    }

    private static void SetDefines(BuildTargetGroup group, System.Collections.Generic.List<string> defineList)
    {
        string newDefine = string.Join(";", defineList.Distinct());
        PlayerSettings.SetScriptingDefineSymbolsForGroup(group, newDefine);
    }
}
#endif
