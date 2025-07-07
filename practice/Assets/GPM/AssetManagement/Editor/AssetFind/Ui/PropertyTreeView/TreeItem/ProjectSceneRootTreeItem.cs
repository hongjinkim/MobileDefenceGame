using UnityEditor;
using UnityEngine;

namespace Gpm.AssetManagement.AssetFind.Ui.PropertyTreeView.TreeItem
{
    using Gpm.AssetManagement.Const;

    class ProjectSceneRootTreeItem : ObjectRootTreeItem
    {
        internal ProjectSceneFindModule findModule;

        public ProjectSceneRootTreeItem(PropertyTreeView rootTree, ProjectSceneFindModule findModule) : base(rootTree, findModule)
        {
            this.findModule = findModule;

            this.checkedForChildren = false;
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

        public bool IsLoadable()
        {
            if (findModule != null)
            {
                return findModule.IsLoadable();
            }

            return false;
        }

        public override  bool CanExpanded()
        {
            if (IsValid() == false &&
                findModule.IsLoadable() == false)
            {
                return false;
            }

            if (findModule != null)
            {
                return true;
            }

            if (hasChildren == true)
            {
                return true;
            }

            return true;
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
                        if (IsValid() == false ||
                            (findModule.IsFind() == false && findModule.CheckFindAble() == false))
                        { 
                            if (findModule.Reload() == true)
                            {
                                findModule.Find();

                                CheckChildren();
                            }
                        }
                    }
                }
            }
        }

        public override void OnDoubleClick()
        {
            if (rootTree.IsExpanded(id) == false)
            {
                rootTree.SetExpanded(id, true);
            }
        }

        public override void RowGUI()
        {
            if (IsValid() == false)
            {
                rootTree.SetExpandedRecursive(id, false);
                rootTree.SetExpanded(id, false);
                expand = false;

                checkedForChildren = false;
                if (findModule.IsFind() == true)
                {
                    findModule.Clear();
                }
            }
            else
            {
                if (findModule.sceneRoot != null &&
                    findModule.sceneRoot.removed == true)
                {
                    checkedForChildren = false;
                    rootTree.SetExpanded(id, false);

                    return;
                }

                if (IsAutoReload() == true)
                {
                    bool before = rootTree.IsExpanded(id);
                    if (before == true)
                    {
                        rootTree.SetExpanded(id, false);
                        this.expand = false;
                    }

                    findModule.ChangedRoot();
                    findModule.Clear();

                    id = findModule.GetID();
                    if (children != null)
                    {
                        children.Clear();
                    }
                    checkedForChildren = false;

                    if (before == true)
                    {
                        CheckChildren();
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

        public override void CellGUI(Rect cellRect, PropertyTreeView.ColumnId column)
        {
            switch (column)
            {
                case PropertyTreeView.ColumnId.NAME:
                    {
                        if (IsValid() == true || IsLoadable() == true)
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
                        if (findModule != null)
                        {
                            if (IsValid() == true)
                            {
                                if (findModule.CheckFindAble() == false)
                                {
                                    if (Ui.Button(cellRect, Strings.KEY_OPENSCENE) == true)
                                    {
                                        rootTree.SetExpanded(id, true);
                                    }
                                    else
                                    {
                                        if (checkedForChildren == true)
                                        {
                                            findModule.Clear();
                                            checkedForChildren = false;
                                        }

                                        rootTree.SetExpanded(id, false);
                                    }
                                }
                            }
                            else
                            {
                                if (IsValid() == false &&
                                    IsLoadable() == true)
                                {
                                    rootTree.SetExpanded(id, false);
                                    if (Ui.Button(cellRect, Strings.KEY_OPENSCENE) == true)
                                    {
                                        rootTree.SetExpanded(id, true);
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