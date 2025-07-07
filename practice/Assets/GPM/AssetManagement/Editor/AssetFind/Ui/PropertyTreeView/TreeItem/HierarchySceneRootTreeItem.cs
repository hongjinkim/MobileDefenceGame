using UnityEditor;
using UnityEngine;

namespace Gpm.AssetManagement.AssetFind.Ui.PropertyTreeView.TreeItem
{
    using Gpm.AssetManagement.Const;

    class HierarchySceneRootTreeItem : ObjectRootTreeItem
    {
        internal HierarchySceneFindModule findModule;
        public HierarchySceneRootTreeItem(PropertyTreeView rootTree, HierarchySceneFindModule findModule) : base(rootTree, findModule)
        {
            this.findModule = findModule;
        }

        ~HierarchySceneRootTreeItem()
        {
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

        public override void RowGUI()
        {
            if (IsAutoReload() == true)
            {
                bool before = rootTree.IsExpanded(id);
                if (before == true)
                {
                    rootTree.SetExpanded(id, false);
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

            if (IsValid() == false)
            {
                rootTree.SetExpanded(id, false);   
                expand = false;
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
                        if (findModule != null)
                        {
                            if (IsValid() == true)
                            {
#if UNITY_2022_2_OR_NEWER
                                if (UnityEngine.SceneManagement.SceneManager.loadedSceneCount > 1)
#else
                                if (UnityEditor.SceneManagement.EditorSceneManager.loadedSceneCount > 1)
#endif
                                {
                                    if (Ui.Button(cellRect, Strings.KEY_CLOSESCENE) == true)
                                    {
                                        UnityEditor.SceneManagement.EditorSceneManager.CloseScene(findModule.sceneRoot.scene, true);
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