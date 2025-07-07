using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

namespace Gpm.AssetManagement.AssetFind.Ui.PropertyTreeView.TreeItem
{
    using Gpm.AssetManagement.Const;

    class PropertyPathTreeItem : PropertyTreeItem
    {
        public string name;
        public Object asset;

        public string path;

        internal SerializedProperty replace;
        internal SerializedProperty temp;

        public PropertyPathTreeItem(PropertyTreeView rootTree, Object asset, string path, int d) : base(rootTree, asset == null ? 0 : asset.GetInstanceID(), d)
        {
            this.asset = asset;
            this.name = path;
            this.path = path;
        }
    
        public override bool CanExpanded()
        {
            return false;
        }

        public override void CellGUI(Rect cellRect, PropertyTreeView.ColumnId column)
        {
            switch (column)
            {
                case PropertyTreeView.ColumnId.NAME:
                    {
                        UnityEngine.GUI.Label(cellRect, name);
                    }
                    break;
                case PropertyTreeView.ColumnId.TYPE:
                    {

                    }

                    break;
                case PropertyTreeView.ColumnId.FUNCTION:
                    {
                        if (string.IsNullOrEmpty(path) == false)
                        {
                            if (path.Contains("m_Component.Array") == true)
                            {
                                EditorGUI.LabelField(cellRect, Ui.GetString(Strings.KEY_COMPONENT_MISSING));
                            }
                            else if (asset != null)
                            {
                                SerializedObject serializedObject = new SerializedObject(asset);
                                SerializedProperty serializedProperty = serializedObject.FindProperty(path);
                                if (serializedProperty != null)
                                {
                                    System.Type type = serializedProperty.GetFieldType();

                                    bool isReplaceable = false;
                                    if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference)
                                    {
                                        if (temp == null)
                                        {
                                            temp = serializedProperty.Copy();
                                        }

                                        isReplaceable = true;
                                    }


                                    if (isReplaceable == true)
                                    {
                                        using (new EditorGUI.DisabledGroupScope(rootTree.enableReplace == false))
                                        {
                                            float btnW = 80;
                                            float fieldW = cellRect.width - btnW;
                                            Rect fieldRect = new Rect(cellRect.x, cellRect.y, fieldW, cellRect.height);
                                            Rect btnRect = new Rect(cellRect.x + fieldW, cellRect.y, btnW, cellRect.height);

                                            if (SerializedProperty.DataEquals(serializedProperty, replace) == true)
                                            {
                                                replace = null;
                                            }

                                            if (rootTree.enableReplace == true &&
                                                replace != null)
                                            {
                                                if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference)
                                                {
                                                    if (replace.editable)
                                                    {
                                                        EditorGUI.ObjectField(fieldRect, replace, new GUIContent());
                                                    }
                                                    else
                                                    {
                                                        Object objectReferenceValue = replace.objectReferenceValue;

                                                        Object changeValue = EditorGUI.ObjectField(fieldRect, objectReferenceValue, type, false);

                                                        if (changeValue != objectReferenceValue)
                                                        {
                                                            replace.objectReferenceValue = changeValue;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (serializedProperty.editable == true)
                                                {
                                                    EditorGUI.ObjectField(fieldRect, serializedProperty, new GUIContent());
                                                    if (serializedObject.hasModifiedProperties)
                                                    {
                                                        replace = serializedProperty.Copy();
                                                    }
                                                }
                                                else
                                                {
                                                    if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference)
                                                    {
                                                        Object objectReferenceValue = serializedProperty.objectReferenceValue;

                                                        Object changeValue = EditorGUI.ObjectField(fieldRect, objectReferenceValue, type, false);

                                                        if (changeValue != objectReferenceValue)
                                                        {
                                                            replace = serializedProperty.Copy();
                                                            replace.objectReferenceValue = changeValue;
                                                        }
                                                    }
                                                }
                                            }


                                            using (new EditorGUI.DisabledGroupScope(replace == null))
                                            {
                                                if (Ui.Button(btnRect, Strings.KEY_REPLACE) == true)
                                                {
                                                    if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference)
                                                    {
                                                        serializedProperty.objectReferenceValue = replace.objectReferenceValue;
                                                    }

                                                    if (serializedObject.ApplyModifiedProperties() == true)
                                                    {
                                                        serializedObject.Update();
                                                    }
                                                    replace = null;

                                                    AssetDatabase.SaveAssets();
                                                    AssetDatabase.Refresh();
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (serializedProperty.objectReferenceValue == null)
                                        {
                                        }
                                        else
                                        {
                                            EditorGUI.LabelField(cellRect, EditorGUIUtility.ObjectContent(serializedProperty.objectReferenceValue, serializedProperty.objectReferenceValue.GetType()));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
        }
    }

}