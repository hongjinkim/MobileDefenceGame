using UnityEditor;
using NhnCloud.GamebaseTools.SettingTool;

public static class UnitySDKExport
{
    [MenuItem("Export/Gamebase SettingTool v" + SettingTool.VERSION)]
    public static void ExportGamebaseSettingTool()
    {
        PackageExportHelper package = new PackageExportHelper();
        package.IncludeDirectoriesAndFiles("Assets/NhnCloud/GamebaseTools");
        package.ExportPackage(string.Format("GamebaseSettingTool_v{0}", SettingTool.VERSION), "../../dist");
    }
}