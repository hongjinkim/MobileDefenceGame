using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(ReferenceImageManager))]
public class ReferenceImageManagerEditor : Editor
{
    private ReferenceImageManager manager;
    private bool showImageList = true;

    private void OnEnable()
    {
        manager = (ReferenceImageManager)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Reference Image Manager", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // 폴더 경로 설정
        EditorGUILayout.PropertyField(serializedObject.FindProperty("imageFolderPath"));

        // 컴포넌트 참조
        EditorGUILayout.PropertyField(serializedObject.FindProperty("targetRawImage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("canvasGroup"));

        EditorGUILayout.Space();

        // 폴더 생성 및 로드 버튼
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Create Folder"))
        {
            CreateReferenceFolder();
        }

        if (GUILayout.Button("Load Images"))
        {
            manager.LoadImagesFromFolder();
            EditorUtility.SetDirty(manager);
        }

        if (GUILayout.Button("Open Folder"))
        {
            OpenReferenceFolder();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // 현재 상태 표시
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.IntField("Loaded Images", manager.GetImageCount());
        if (manager.GetImageCount() > 0)
        {
            EditorGUILayout.TextField("Current Image", manager.GetCurrentImageName());
            EditorGUILayout.TextField("Asset Path", manager.GetCurrentImagePath());
        }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();

        // 가시성 및 투명도 제어
        bool showReference = EditorGUILayout.Toggle("Show Reference", serializedObject.FindProperty("showReference").boolValue);
        if (showReference != serializedObject.FindProperty("showReference").boolValue)
        {
            serializedObject.FindProperty("showReference").boolValue = showReference;
            manager.SetVisibility(showReference);
        }

        // Alpha 슬라이더
        float currentAlpha = serializedObject.FindProperty("referenceAlpha").floatValue;
        float newAlpha = EditorGUILayout.Slider("Reference Alpha", currentAlpha, 0f, 1f);
        if (Mathf.Abs(newAlpha - currentAlpha) > 0.001f)
        {
            serializedObject.FindProperty("referenceAlpha").floatValue = newAlpha;
            manager.SetAlpha(newAlpha);
        }

        // 이미지 탐색 컨트롤
        if (manager.GetImageCount() > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Image Navigation", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Previous"))
            {
                manager.PreviousImage();
            }

            // 현재 인덱스 표시 및 직접 설정
            int currentIndex = serializedObject.FindProperty("currentImageIndex").intValue;
            int newIndex = EditorGUILayout.IntSlider(currentIndex, 0, manager.GetImageCount() - 1);
            if (newIndex != currentIndex)
            {
                manager.SetCurrentImage(newIndex);
            }

            if (GUILayout.Button("Next"))
            {
                manager.NextImage();
            }

            EditorGUILayout.EndHorizontal();
        }

        // 이미지 목록 표시
        if (manager.GetImageCount() > 0)
        {
            EditorGUILayout.Space();
            showImageList = EditorGUILayout.Foldout(showImageList, $"Image List ({manager.GetImageCount()})");

            if (showImageList)
            {
                EditorGUI.indentLevel++;

                var imageNames = serializedObject.FindProperty("imageNames");
                var assetPaths = serializedObject.FindProperty("assetPaths");

                for (int i = 0; i < imageNames.arraySize; i++)
                {
                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.BeginHorizontal();

                    bool isCurrent = (i == serializedObject.FindProperty("currentImageIndex").intValue);
                    if (isCurrent)
                    {
                        GUI.backgroundColor = Color.cyan;
                    }

                    EditorGUILayout.LabelField($"{i}: {imageNames.GetArrayElementAtIndex(i).stringValue}");

                    if (GUILayout.Button("Select", GUILayout.Width(60)))
                    {
                        manager.SetCurrentImage(i);
                    }

                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndHorizontal();

                    // 에셋 경로 표시
                    if (i < assetPaths.arraySize)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.LabelField("Path:", assetPaths.GetArrayElementAtIndex(i).stringValue, EditorStyles.miniLabel);
                        EditorGUI.indentLevel--;
                    }

                    EditorGUILayout.EndVertical();
                }

                EditorGUI.indentLevel--;
            }
        }

        // 에디터 모드에서 추가 정보 표시
        if (!Application.isPlaying)
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Reference images are available in Edit Mode for UI design work.", MessageType.Info);

            // 빠른 가시성 토글 버튼
            if (manager.GetImageCount() > 0)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Toggle Visibility"))
                {
                    manager.ToggleVisibility();
                }
                if (GUILayout.Button("Refresh Display"))
                {
                    manager.LoadImagesFromFolder();
                }
                EditorGUILayout.EndHorizontal();

                // 빠른 Alpha 조정 버튼들
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Quick Alpha:", GUILayout.Width(80));
                if (GUILayout.Button("25%"))
                {
                    manager.SetAlpha(0.25f);
                    serializedObject.FindProperty("referenceAlpha").floatValue = 0.25f;
                }
                if (GUILayout.Button("50%"))
                {
                    manager.SetAlpha(0.5f);
                    serializedObject.FindProperty("referenceAlpha").floatValue = 0.5f;
                }
                if (GUILayout.Button("75%"))
                {
                    manager.SetAlpha(0.75f);
                    serializedObject.FindProperty("referenceAlpha").floatValue = 0.75f;
                }
                if (GUILayout.Button("100%"))
                {
                    manager.SetAlpha(1f);
                    serializedObject.FindProperty("referenceAlpha").floatValue = 1f;
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void CreateReferenceFolder()
    {
        string folderPath = serializedObject.FindProperty("imageFolderPath").stringValue;

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            // 상위 폴더들을 순차적으로 생성
            string[] folders = folderPath.Split('/');
            string currentPath = folders[0]; // "Assets"

            for (int i = 1; i < folders.Length; i++)
            {
                string newPath = currentPath + "/" + folders[i];
                if (!AssetDatabase.IsValidFolder(newPath))
                {
                    AssetDatabase.CreateFolder(currentPath, folders[i]);
                    Debug.Log($"Created folder: {newPath}");
                }
                currentPath = newPath;
            }

            AssetDatabase.Refresh();
            Debug.Log($"Reference image folder created: {folderPath}");
        }
        else
        {
            Debug.Log($"Reference image folder already exists: {folderPath}");
        }
    }

    private void OpenReferenceFolder()
    {
        string folderPath = serializedObject.FindProperty("imageFolderPath").stringValue;

        if (AssetDatabase.IsValidFolder(folderPath))
        {
            // Project 창에서 폴더 선택
            UnityEngine.Object folderAsset = AssetDatabase.LoadAssetAtPath(folderPath, typeof(UnityEngine.Object));
            Selection.activeObject = folderAsset;
            EditorGUIUtility.PingObject(folderAsset);
        }
        else
        {
            EditorUtility.DisplayDialog("Folder Not Found",
                $"Reference image folder does not exist:\n{folderPath}\n\nClick 'Create Folder' first.", "OK");
        }
    }
}