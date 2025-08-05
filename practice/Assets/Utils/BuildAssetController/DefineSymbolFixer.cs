#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;

[InitializeOnLoad]
public static class DefineSymbolFixer
{
    static DefineSymbolFixer()
    {
        EditorApplication.delayCall += EnsureDefineSymbols;
    }

    private static void EnsureDefineSymbols()
    {
        FixDefine(BuildTargetGroup.Android);
        FixDefine(BuildTargetGroup.Standalone); // Windows/Mac/Linux
        // ÇÊ¿äÇÑ ÇÃ·§Æû Ãß°¡
    }

    private static void FixDefine(BuildTargetGroup targetGroup)
    {
        string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

        if (IsOdinInstalled() && !symbols.Contains("ODIN_INSPECTOR"))
        {
            symbols += ";ODIN_INSPECTOR";
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, symbols);
        }
        else if (!IsOdinInstalled() && symbols.Contains("ODIN_INSPECTOR"))
        {
            symbols = symbols.Replace("ODIN_INSPECTOR", "").Replace(";;", ";");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, symbols);
        }
    }

    private static bool IsOdinInstalled()
    {
        return System.IO.Directory.Exists("Assets/Plugins/Sirenix");
    }
}
#endif
