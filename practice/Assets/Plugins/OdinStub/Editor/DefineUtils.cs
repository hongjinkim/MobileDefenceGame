#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public static class DefineUtils
{
    public static void AddDefineSymbol(string symbol)
    {
        ModifyDefineSymbols(symbol, true);
    }

    public static void RemoveDefineSymbol(string symbol)
    {
        ModifyDefineSymbols(symbol, false);
    }

    private static void ModifyDefineSymbols(string symbol, bool add)
    {
        var targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
        var defineList = defines.Split(';').ToList();

        if (add && !defineList.Contains(symbol))
        {
            defineList.Add(symbol);
        }
        else if (!add && defineList.Contains(symbol))
        {
            defineList.Remove(symbol);
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, string.Join(";", defineList));
    }
}
#endif
