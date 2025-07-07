using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

namespace Gpm.AssetManagement.AssetFind.Ui
{
    using Gpm.AssetManagement.AssetMap;
    using Gpm.AssetManagement.Const;

    public class ProjectPropertyTreeGUI
    {
        public class TreeGUI
        {
            private TreeViewState m_TreeState;
            private SearchField m_SearchField;
            private MultiColumnHeaderState m_Mchs;

            internal PropertyTreeView.PropertyTreeView m_EntryTree;

            public void Init(bool scene)
            {
                if (m_EntryTree == null)
                {
                    if (m_TreeState == null)
                    {
                        m_TreeState = new TreeViewState();
                    }

                    var headerState = PropertyTreeView.PropertyTreeView.CreateDefaultMultiColumnHeaderState();
                    if (MultiColumnHeaderState.CanOverwriteSerializedFields(m_Mchs, headerState) == true)
                    {
                        MultiColumnHeaderState.OverwriteSerializedFields(m_Mchs, headerState);
                    }
                    m_Mchs = headerState;

                    m_SearchField = new SearchField();
                    m_EntryTree = new PropertyTreeView.PropertyTreeView(m_TreeState, m_Mchs);
                }
            }
        }

        private TreeGUI scene_tree;
        private TreeGUI asset_tree;

        private bool bLoad = false;

        public void Init()
        {
            if (scene_tree == null)
            {
                scene_tree = new TreeGUI();
                scene_tree.Init(true);
            }

            if (asset_tree == null)
            {
                asset_tree = new TreeGUI();
                asset_tree.Init(false);
            }

            bLoad = false;
        }

        public void FindProject(Object findTarget, bool containSubObject)
        {
            if (EditorUtility.IsPersistent(findTarget) == true)
            {
                PropertyFinder sceneModuleFinder = new PropertyFinder();
                PropertyFinder moduleFinder = new PropertyFinder();

                string findPath = AssetDatabase.GetAssetPath(findTarget);
                int index = 0;
                var node = GpmAssetManagementManager.GetAssetDataFromPath(findPath);
                if (node != null)
                {
                    foreach (AssetMapLink link in node.referenceLinks)
                    {
                        if (Ui.CheckPassByTime(Constants.CHECK_TIME_PROGRESS) == true)
                        {
                            float rate = (float)index / (float)node.referenceLinks.Count;
                            EditorUtility.DisplayProgressBar(Constants.SERVICE_NAME, string.Format(Constants.FORMAT_REFERENCE_CHECK, index, node.referenceLinks.Count), rate);
                        }

                        Object obj = AssetDatabase.LoadAssetAtPath<Object>(link.GetPath());
                        if (obj == null)
                        {
                            continue;
                        }

                        bool sceneAsset = obj is SceneAsset;

                        FindModule module = PropertyFinder.GetModule(obj);
                        module.SetCondition(findTarget, containSubObject);

                        module.Find();

                        if (module.IsFind() == false ||
                            module.result.Count > 0)
                        {
                            if(sceneAsset == true)
                            {
                                sceneModuleFinder.moduleList.Add(module);
                            }
                            else
                            {
                                moduleFinder.moduleList.Add(module);
                            }
                        }

                        index++;
                    }

                    EditorUtility.ClearProgressBar();

                    if (sceneModuleFinder.moduleList.Count > 0)
                    {
                        scene_tree.m_EntryTree.Setting(sceneModuleFinder);
                        scene_tree.m_EntryTree.Reload();
                    }
                    else
                    {
                        scene_tree.m_EntryTree.Clear();
                        scene_tree.m_EntryTree.Reload();
                    }

                    if (moduleFinder.moduleList.Count > 0)
                    {
                        asset_tree.m_EntryTree.Setting(moduleFinder);
                        asset_tree.m_EntryTree.Reload();
                    }
                    else
                    {
                        asset_tree.m_EntryTree.Clear();
                        asset_tree.m_EntryTree.Reload();
                    }
                    bLoad = true;
                }
            }
            else
            {
                scene_tree.m_EntryTree.Clear();
                scene_tree.m_EntryTree.Reload();

                asset_tree.m_EntryTree.Clear();
                asset_tree.m_EntryTree.Reload();

                bLoad = true;
            }
        }
        
        public void SetEnableReplace(bool enableReplace)
        {
            if (bLoad == true)
            {
                if (scene_tree != null)
                {
                    scene_tree.m_EntryTree.enableReplace = enableReplace;
                }

                if (asset_tree != null)
                {
                    asset_tree.m_EntryTree.enableReplace = enableReplace;
                }
            }
        }

        public void OnGUI()
        {
            if (bLoad == true)
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    Ui.Label(Strings.KEY_SCENE, EditorStyles.boldLabel);

                    if (scene_tree.m_EntryTree != null)
                    {
                        if (scene_tree.m_EntryTree.HasData() == true)
                        {
                            Rect rt = EditorGUILayout.GetControlRect(false, GUILayout.ExpandHeight(true));
                            scene_tree.m_EntryTree.OnGUI(rt);
                        }
                        else
                        {
                            Ui.Label(Strings.KEY_NOT_FOUND_REFERENCE);
                        }
                    }

                    Ui.Label(Strings.KEY_ASSET, EditorStyles.boldLabel);

                    if (asset_tree.m_EntryTree != null)
                    {
                        if (asset_tree.m_EntryTree.HasData() == true)
                        {
                            Rect rt = EditorGUILayout.GetControlRect(false, GUILayout.ExpandHeight(true));
                            asset_tree.m_EntryTree.OnGUI(rt);
                        }
                        else
                        {
                            Ui.Label(Strings.KEY_NOT_FOUND_REFERENCE);
                        }
                    }
                }
            }

        }
    }
}