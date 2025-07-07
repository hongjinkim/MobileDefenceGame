using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

namespace Gpm.AssetManagement.AssetFind.Ui
{
    using Gpm.AssetManagement.Const;

    public class HierachyPropertyTreeGUI
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
        private TreeGUI prefabStage_tree;
        

        private bool bLoad = false;

        public void Init(bool scene)
        {
            if(scene_tree == null)
            {
                scene_tree = new TreeGUI();
                scene_tree.Init(true);
            }

            if (prefabStage_tree == null)
            {
                prefabStage_tree = new TreeGUI();
                prefabStage_tree.Init(false);
            }
            bLoad = false;
        }

        public void FindHierarchy(Object findTarget, bool containSubObject)
        {
            FindHierarchy_Scene(findTarget, containSubObject);
            FindHierarchy_PrefabStage(findTarget, containSubObject);
        }

        public void FindHierarchy_Scene(Object findTarget, bool containSubObject)
        {
            HierarchySceneFinder moduleFinder = new HierarchySceneFinder();
            moduleFinder.SetCondition(findTarget, containSubObject);
            moduleFinder.Find();

            scene_tree.m_EntryTree.Setting(moduleFinder);
            
            bLoad = true;
        }

        public void FindHierarchy_PrefabStage(Object findTarget, bool containSubObject)
        {
            HierarchyPrefabFinder moduleFinder = new HierarchyPrefabFinder();
            moduleFinder.SetCondition(findTarget, containSubObject);
            moduleFinder.Find();

            prefabStage_tree.m_EntryTree.Setting(moduleFinder);

            bLoad = true;
        }

        public void SetEnableReplace(bool enableReplace)
        {
            if (bLoad == true)
            {
                if (scene_tree != null)
                {
                    scene_tree.m_EntryTree.enableReplace = enableReplace;
                }

                if (prefabStage_tree != null)
                {
                    prefabStage_tree.m_EntryTree.enableReplace = enableReplace;
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

                    if (scene_tree.m_EntryTree.HasData() == true)
                    {
                        Rect rt = EditorGUILayout.GetControlRect(false, GUILayout.ExpandHeight(true));
                        scene_tree.m_EntryTree.OnGUI(rt);
                    }
                    else
                    {
                        GUI.depth++;
                        Ui.Label(Strings.KEY_NOT_FOUND_REFERENCE);
                        GUI.depth--;
                    }


                    Ui.Label(Strings.KEY_PREFAB, EditorStyles.boldLabel);

                    if (prefabStage_tree.m_EntryTree.HasData() == true)
                    {
                        Rect rt = EditorGUILayout.GetControlRect(false, GUILayout.ExpandHeight(true));
                        prefabStage_tree.m_EntryTree.OnGUI(rt);
                    }
                    else
                    {
                        GUI.depth++;
                        Ui.Label(Strings.KEY_NOT_FOUND_REFERENCE);
                        GUI.depth--;
                    }
                }
            }
        }
    }
}