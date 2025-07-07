using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Gpm.AssetManagement.Optimize.Ui
{
    using Gpm.AssetManagement.Const;

    public class UnusedAssetGUI
    {
        private TreeViewState m_TreeState;
        private UnusedAssetTreeView m_EntryTree;
        private SearchField m_SearchField;
        private MultiColumnHeaderState m_Mchs;

        private UnusedAssetFilter filter;

        private UnusedAssetFilterGUI filterGUI;

        private int toolbarSel = 0;

        public void Init()
        {
            if (filter == null)
            {
                filter = UnusedAssetFilter.Load();
                if (filter == null)
                {
                    filter = new UnusedAssetFilter();
                }
            }

            if (m_EntryTree == null)
            {
                if (m_TreeState == null)
                {
                    m_TreeState = new TreeViewState();
                }

                var headerState = UnusedAssetTreeView.CreateDefaultMultiColumnHeaderState();
                if (MultiColumnHeaderState.CanOverwriteSerializedFields(m_Mchs, headerState) == true)
                {
                    MultiColumnHeaderState.OverwriteSerializedFields(m_Mchs, headerState);
                }
                m_Mchs = headerState;

                m_SearchField = new SearchField();
                m_EntryTree = new UnusedAssetTreeView(m_TreeState, m_Mchs, filter);
                m_EntryTree.Reload();


                filterGUI = new UnusedAssetFilterGUI();
                filterGUI.Init(filter);
            }
        }

        public void OnGUI()
        {
            if (m_EntryTree == null)
            {
                Init();
            }

            if (m_EntryTree != null)
            {
                using (new GUILayout.VerticalScope())
                {
                    string[] toolbar = { Ui.GetString(Strings.KEY_UNUSEDASSET), Ui.GetString(Strings.KEY_FILTER_OPTION) };
                    toolbarSel = GUILayout.Toolbar(toolbarSel, toolbar);
                    if (toolbarSel == 0)
                    {
                        long size = 0;
                        bool checkAll = true;
                        bool removeAble = false;
                        bool restoreAble = false;
                        var itemList = m_EntryTree.GetRows();
                        if (itemList != null)
                        {
                            for (int i = 0; i < itemList.Count; i++)
                            {
                                if (itemList[i] is OptimizeTreeItem optimizeItem)
                                {
                                    if (optimizeItem.check == true &&
                                        optimizeItem.filter == false)
                                    {
                                        if (optimizeItem.removed == false)
                                        {
                                            size += optimizeItem.size;
                                            removeAble = true;
                                        }
                                        else
                                        {
                                            restoreAble = true;
                                        }
                                    }

                                    if (optimizeItem.check == false)
                                    {
                                        checkAll = false;
                                    }
                                }

                            }
                        }

                        using (new GUILayout.HorizontalScope())
                        {
                            if (Ui.Button(Strings.KEY_FIND) == true)
                            {
                                m_EntryTree.Find();
                            }
                        }

                        bool changeCheckAll = EditorGUILayout.Toggle(Ui.GetString(Strings.KEY_CHECK_ALL), checkAll);

                        if (changeCheckAll != checkAll)
                        {
                            for (int i = 0; i < itemList.Count; i++)
                            {
                                if (itemList[i] is OptimizeTreeItem optimizeItem)
                                {
                                    optimizeItem.check = changeCheckAll;
                                }
                            }
                        }

                        m_EntryTree.OnGUI(EditorGUILayout.GetControlRect(false, GUILayout.ExpandHeight(true)));

                        using (new GUILayout.HorizontalScope())
                        {
                            double kb = size / 1024.0;
                            double mb = (double)((kb / 1024.0));

                            string sizeText;
                            if (mb > 1)
                            {
                                sizeText = string.Format("{0:0.0} mb", mb);
                            }
                            else if (kb > 1)
                            {
                                sizeText = string.Format("{0:0.0} kb", kb);
                            }
                            else
                            {
                                sizeText = string.Format("{0} bytes", size);
                            }

                            EditorGUILayout.LabelField(Ui.GetString(Strings.KEY_TOTAL_SIZE), sizeText);

                            using (new EditorGUI.DisabledGroupScope(restoreAble == false))
                            {
                                if (Ui.Button(Strings.KEY_RESTORE_ALL) == true)
                                {
                                    if (itemList != null)
                                    {
                                        for (int i = 0; i < itemList.Count; i++)
                                        {
                                            if (itemList[i] is OptimizeTreeItem optimizeItem)
                                            {
                                                if( optimizeItem.check == true &&
                                                    optimizeItem.filter == false)
                                                {
                                                    optimizeItem.Restore();
                                                    optimizeItem.removed = false;
                                                }
                                            }

                                        }

                                        AssetDatabase.Refresh();
                                    }
                                }
                            }
                            using (new EditorGUI.DisabledGroupScope(removeAble == false))
                            {
                                if (Ui.Button(Strings.KEY_REMOVE_ALL) == true)
                                {
                                    bool check = EditorUtility.DisplayDialog(Ui.GetString(Strings.KEY_REMOVE_ALL), Ui.GetString(Strings.KEY_CHECK_REMOVE), Ui.GetString(Strings.KEY_OK), Ui.GetString(Strings.KEY_CANCEL));
                                    if (check == true)
                                    {
                                        if (itemList != null)
                                        {
                                            for (int i = 0; i < itemList.Count; i++)
                                            {
                                                if (itemList[i] is OptimizeTreeItem optimizeItem)
                                                {
                                                    if (optimizeItem.check == true &&
                                                        optimizeItem.filter == false)
                                                    {
                                                        optimizeItem.Remove();
                                                        optimizeItem.removed = true;
                                                    }
                                                }

                                            }

                                            AssetDatabase.Refresh();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        filterGUI.OnGUI();
                    }
                }

                filter.CheckSaved();

            }
        }
    }
}