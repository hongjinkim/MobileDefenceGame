
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class PlayModeStartScene
{
    private const string Key = "PlayModeStartScenePath";

    [MenuItem("Tools/Play Mode/Set Start Scene → Active Scene")]
    public static void SetStartToActive()
    {
        var scene = EditorSceneManager.GetActiveScene();
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
        EditorSceneManager.playModeStartScene = sceneAsset;
        EditorPrefs.SetString(Key, scene.path);
        Debug.Log($"[PlayMode] Start Scene set to: {scene.name}");
    }

    [MenuItem("Tools/Play Mode/Clear Start Scene")]
    public static void ClearStart()
    {
        EditorSceneManager.playModeStartScene = null;
        EditorPrefs.DeleteKey(Key);
        Debug.Log("[PlayMode] Start Scene cleared (uses current scene).");
    }

    // 에디터 실행 시 자동 복원
    [InitializeOnLoadMethod]
    private static void Restore()
    {
        var path = EditorPrefs.GetString(Key, null);
        if (!string.IsNullOrEmpty(path))
        {
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
            if (sceneAsset != null)
                EditorSceneManager.playModeStartScene = sceneAsset;
        }
    }
}
#endif

