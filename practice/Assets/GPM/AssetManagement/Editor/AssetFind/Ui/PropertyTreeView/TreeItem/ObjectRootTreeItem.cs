using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;

namespace Gpm.AssetManagement.AssetFind.Ui.PropertyTreeView.TreeItem
{
    using Gpm.AssetManagement.Const;
    using Gpm.AssetManagement.AssetFind;

    internal class ObjectRootTreeItem : PropertyTreeItem
    {
        public string name;

        public Texture2D typeIcon;

        public bool expand = false;
        public bool checkedForChildren = false;

        protected bool changeExpand = false;

        internal FindModule findModuleBase = null;

        public ObjectRootTreeItem(PropertyTreeView rootTree, FindModule findModuleBase) : base(rootTree, findModuleBase == null ? 0 : findModuleBase.GetID(), 0)
        {
            this.findModuleBase = findModuleBase;

            this.name = findModuleBase.GetName();

            this.typeIcon = findModuleBase.GetTypeIcon();

            this.checkedForChildren = false;
        }
       
        public bool IsValid()
        {
            if (findModuleBase != null)
            {
                return findModuleBase.IsValid();
            }

            return false;
        }

        internal void CheckChildren()
        {
            if (checkedForChildren == false)
            {
                checkedForChildren = true;

                if (findModuleBase != null)
                {
                    if (findModuleBase.IsValid() == false)
                    {
                        if (findModuleBase.Reload() == false)
                        {
                            return;
                        }
                    }

                    if (findModuleBase.IsFind() == false)
                    {
                        findModuleBase.Find();
                    }

                    if (children != null)
                    {
                        children.Clear();
                    }

                    foreach (var result in findModuleBase.result)
                    {
                        if (result.rootObject == null)
                        {
                            continue;
                        }

                        if (id == result.rootObject.GetInstanceID())
                        {
                            PropertyPathTreeItem pathItem = new PropertyPathTreeItem(rootTree, result.rootObject, result.path, 1);

                            AddChild(pathItem);
                        }
                        else
                        {
                            ObjectTreeItem findRootObjectItem = new ObjectTreeItem(rootTree, result.rootObject, 1);
                            AddChild(findRootObjectItem);

                            if (string.IsNullOrEmpty(result.path) == false)
                            {
                                PropertyPathTreeItem pathItem = new PropertyPathTreeItem(rootTree, result.rootObject, result.path, 2);
                                findRootObjectItem.AddChild(pathItem);
                            }

                            changeExpand = true;
                        }
                    }
                    
                    if(changeExpand == true)
                    {
                        rootTree.SetExpanded(id, false);
                        rootTree.Repaint();
                    }
                }
            }
        }

        public override bool CanExpanded()
        {
            if (IsValid() == false)
            {
                return false;
            }

            if (checkedForChildren == false)
            {
                if (findModuleBase != null)
                {
                    return true;
                }
            }

            return hasChildren;
        }

        public override void OnExpanded(bool expand)
        {
            if (this.expand != expand)
            {
                this.expand = expand;
                if (expand == true)
                {
                    if (checkedForChildren == false)
                    {
                        CheckChildren();
                    }
                }
            }
        }

        public override void OnDoubleClick()
        {
            if (id != 0)
            {
                if (rootTree.IsExpanded(id) == false)
                {
                    rootTree.SetExpanded(id, true);
                }

                if (AssetDatabase.OpenAsset(id) == true)
                {

                }
                else
                {
                    Selection.activeInstanceID = id;
                    UnityEditor.SceneView.lastActiveSceneView.FrameSelected();
                }
            }
        }

        public override void RowGUI()
        {
            if (IsValid() == false)
            {
                if (expand == true)
                {
                    rootTree.SetExpandedRecursive(id, false);
                    rootTree.SetExpanded(id, false);
                    expand = false;
                }
            }
            else
            {
                if (checkedForChildren == false &&
                    findModuleBase.IsFind() == true)
                {
                    CheckChildren();
                }

                if (changeExpand == true)
                {
                    rootTree.SetExpanded(id, true);
                    changeExpand = false;
                }
            }
        }

        public override void CellGUI(Rect cellRect, PropertyTreeView.ColumnId column)
        {
            switch (column)
            {
                case PropertyTreeView.ColumnId.NAME:
                    {
                        if (IsValid() == true )
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