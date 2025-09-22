using UnityEditor;
using UnityEditor.Callbacks;

[InitializeOnLoad]
public static class ForceBuildTarget
{
    static ForceBuildTarget()
    {
        // ¿øÇÏ´Â ÇÃ·§Æû ÁöÁ¤
        BuildTarget target = BuildTarget.Android;

        if (EditorUserBuildSettings.activeBuildTarget != target)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(
                BuildPipeline.GetBuildTargetGroup(target),
                target
            );
            UnityEngine.Debug.Log($"Build Target forced to {target}");
        }
    }
}