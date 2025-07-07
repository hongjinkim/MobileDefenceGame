using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;

#if UNITY_2021_2_OR_NEWER
using UnityEditor.SceneManagement;
#else
using UnityEditor.Experimental.SceneManagement;
#endif

namespace Gpm.AssetManagement.AssetFind.Ui.PropertyTreeView.TreeItem
{
    using Gpm.AssetManagement.Const;

    class ObjectTreeItem : PropertyTreeItem
    {
        public string name;
        public Object asset;

        public Texture2D typeIcon;
        
        public ObjectTreeItem(PropertyTreeView rootTree, Object asset, int d) : base(rootTree, asset == null ? 0 : asset.GetInstanceID(), d)
        {
            this.asset = asset;
            this.name = asset.name;

            typeIcon = AssetPreview.GetMiniThumbnail(asset);
        }
        

        public override bool CanExpanded()
        {   
            if(asset == null)
            {
                return false;
            }

            if (hasChildren == true)
            {
                return true;
            }

            return false;
        }

        public override void OnDoubleClick()
        {
            if (rootTree.IsExpanded(id) == false)
            {
                rootTree.SetExpandedRecursive(id, true);
            }

            GoToContents();
        }

        private void GoToContents()
        {
            if (AssetDatabase.OpenAsset(asset) == true)
            {
                string guid;
                long localId;
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(asset, out guid, out localId) == true)
                {
                    var stage = PrefabStageUtility.GetCurrentPrefabStage();
                    if (stage != null)
                    {
                        if (stage.scene.isDirty == true)
                        {
                            return;
                        }

#if UNITY_2020_1_OR_NEWER
                        string prefabPath = stage.assetPath;
#else
                        string prefabPath = stage.prefabAssetPath;
#endif

                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        if (path.Equals(prefabPath) == true)
                        {
                            Object targetObject = GetObjectFromLocalID(stage.prefabContentsRoot, localId);
                            EditorGUIUtility.PingObject(targetObject);
                            Selection.activeObject = targetObject;
                            UnityEditor.SceneView.lastActiveSceneView.FrameSelected();
                        }
                    }
                }

            }
            else
            {
                Selection.activeInstanceID = id;
                UnityEditor.SceneView.lastActiveSceneView.FrameSelected();
            }
        }

        private Object GetObjectFromLocalID(GameObject root, long localID)
        {
            if (localID == GetLocalIdentifierInFile(root))
            {
                return root;
            }

            SerializedObject serializedObject = new SerializedObject(root);
            SerializedProperty property = serializedObject.FindProperty("m_Component");
            
            while (property.Next(true) == true)
            {
                if (property.propertyType == SerializedPropertyType.ObjectReference)
                {
                    if (property.objectReferenceValue is Component comp)
                    {
                        if (comp != null)
                        {
                            if (localID == GetLocalIdentifierInFile(comp))
                            {
                                return comp;
                            }

                        }
                    }
                }
            }

            for (int i = 0; i < root.transform.childCount; i++)
            {
                Transform childTrans = root.transform.GetChild(i);
                Object ret = GetObjectFromLocalID(childTrans.gameObject, localID);
                if (ret != null)
                {
                    return ret;
                }
            }

            return null;
        }

        private long GetLocalIdentifierInFile(UnityEngine.Object o)
        {
            var inspectorModeInfo = typeof(SerializedObject).GetProperty("inspectorMode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (inspectorModeInfo != null)
            {
                // so.inspectorMode = InspectorMode.Debug;  // Reflection
                var so = new SerializedObject(o);
                inspectorModeInfo.SetValue(so, InspectorMode.Debug, null);
                var prop = so.FindProperty("m_LocalIdentfierInFile");
                return prop.longValue;
            }

            return 0;
        }

        public override void RowGUI()
        {
            if (asset == null)
            {
                if(rootTree.IsExpanded(id) == true)
                {
                    rootTree.SetExpandedRecursive(id, false);
                }
                    
            }
        }

        public override void CellGUI(Rect cellRect, PropertyTreeView.ColumnId column)
        {
            switch (column)
            {
                case PropertyTreeView.ColumnId.NAME:
                    {
                        if (asset != null)
                        {
                            if (typeIcon != null)
                            {
                                UnityEngine.GUI.Label(cellRect, new GUIContent(name, typeIcon));
                            }
                            else
                            {
                                UnityEngine.GUI.Label(cellRect, name);
                            }
                        }
                        else
                        {
                            using (new EditorGUI.DisabledGroupScope(true))
                            {
                                string invalidName = string.Format(Constants.FORMAT_ITEM_INVALID_NAME, name);
                                if (typeIcon != null)
                                {
                                    UnityEngine.GUI.Label(cellRect, new GUIContent(invalidName, typeIcon));
                                }
                                else
                                {
                                    UnityEngine.GUI.Label(cellRect, invalidName);
                                }
                            }
                        }

                    }
                    break;
                case PropertyTreeView.ColumnId.TYPE:
                    {
                        if (typeIcon != null)
                        {
                            UnityEngine.GUI.DrawTexture(cellRect, typeIcon, ScaleMode.ScaleToFit, true);
                        }
                    }

                    break;
                case PropertyTreeView.ColumnId.FUNCTION:
                    {
                    }
                    break;
            }
        }
    }

}