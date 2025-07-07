using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;

namespace Gpm.AssetManagement.AssetFind.Ui.PropertyTreeView.TreeItem
{
    using Gpm.AssetManagement.Const;

    class HierarchyPrefabRootTreeItem : ObjectRootTreeItem, IPropertyItemGUI
    {
        internal HierarchyPrefabFindModule findModule;
        public HierarchyPrefabRootTreeItem(PropertyTreeView rootTree, HierarchyPrefabFindModule findModule) : base(rootTree, findModule)
        {
            this.findModule = findModule;
        }

        public bool IsAutoReload()
        {
            if (findModule != null)
            {
                if (findModule.IsChangeRoot() == true)
                {
                    return true;
                }
            }

            return false;
        }

        new public void RowGUI()
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
                if (IsAutoReload() == true)
                {
                    bool before = rootTree.IsExpanded(id);
                    if (before == true)
                    {
                        rootTree.SetExpanded(id, false);
                    }

                    findModule.ChangedRoot();
                    id = findModule.GetID();
                    if (children != null)
                    {
                        children.Clear();
                    }
                    checkedForChildren = false;

                    if (before == true)
                    {
                        rootTree.SetExpanded(id, true);
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

        }

        new public void CellGUI(Rect cellRect, PropertyTreeView.ColumnId column)
        {
            switch (column)
            {
                case PropertyTreeView.ColumnId.NAME:
                    {
                        if (IsValid() == true)
                        {
                            if (typeIcon != null)
                            {
                                GUI.Label(cellRect, new GUIContent(name, typeIcon));
                            }
                            else
                            {
                                GUI.Label(cellRect, name);
                            }
                        }
                        else
                        {
                            using (new EditorGUI.DisabledGroupScope(true))
                            {
                                string invalidName = string.Format(Constants.FORMAT_ITEM_INVALID_NAME, name);
                                if (typeIcon != null)
                                {
                                    GUI.Label(cellRect, new GUIContent(invalidName, typeIcon));
                                }
                                else
                                {
                                    GUI.Label(cellRect, invalidName);
                                }
                            }
                        }

                    }
                    break;
                case PropertyTreeView.ColumnId.TYPE:
                    {
                        if (typeIcon != null)
                        {
                            GUI.DrawTexture(cellRect, typeIcon, ScaleMode.ScaleToFit, true);
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