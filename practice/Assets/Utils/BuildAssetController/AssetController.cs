#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Compilation;
using System.IO;

[InitializeOnLoad]
public static class AssetChecker
{
    static AssetChecker()
    {
        bool odinExists = Directory.Exists("Assets/Plugins/Sirenix");

        var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
        if (odinExists && !symbols.Contains("ODIN_INSPECTOR"))
        {
            symbols += ";ODIN_INSPECTOR";
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbols);
        }
        else if (!odinExists && symbols.Contains("ODIN_INSPECTOR"))
        {
            symbols = symbols.Replace("ODIN_INSPECTOR", "");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbols);
        }
    }
}
#endif
