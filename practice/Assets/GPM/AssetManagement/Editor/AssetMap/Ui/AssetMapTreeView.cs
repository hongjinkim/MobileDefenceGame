using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;

namespace Gpm.AssetManagement.AssetMap.Ui
{
    using Gpm.AssetManagement.Const;

    class AssetMapTreeViewItem : TreeViewItem
    {
        private AssetMapTreeView treeView;

        public AssetMapData data;
        public Object assetObject;

        public Texture typeIcon;

        public bool checkedForChildren = false;

        public AssetMapTreeViewItem(AssetMapTreeView treeView, AssetMapData data, int depth)
        {
            this.treeView = treeView;

            this.data = data;
            this.assetObject = AssetDatabase.LoadMainAssetAtPath(data.GetPath());

            this.typeIcon = AssetDatabase.GetCachedIcon(data.GetPath());
            if (assetObject != null)
            {
                this.id = assetObject.GetInstanceID();   
            }
            this.depth = depth;

        }

        internal void CheckChildren()
        {
            if(checkedForChildren == false)
            {
                if(treeView.isReference)
                {
                    SettingReference();
                }
                else
                {
                    SettingDependency();
                }
                
                checkedForChildren = true;
            }
        }

        public void SettingDependency()
        {
            for (int i=0;i<data.dependencyLinks.Count;i++)
            {
                AssetMapTreeViewItem addItem = new AssetMapTreeViewItem(treeView, data.dependencyLinks[i].GetData(), depth+1);

                AddChild(addItem);
            }
        }

        public void SettingReference()
        {
            for (int i = 0; i < data.referenceLinks.Count; i++)
            {
                AssetMapTreeViewItem addItem = new AssetMapTreeViewItem(treeView, data.referenceLinks[i].GetData(), depth + 1);

                AddChild(addItem);
            }
        }

    }

    public class AssetMapTreeView : TreeView
    {
        private enum ColumnId
        {
            NAME,
            TYPE,
        }

        private AssetMapGUI assetMap;
        private AssetMapData rootAssetData;
        internal bool isReference = false;

        public AssetMapTreeView(AssetMapGUI assetMap, TreeViewState state, MultiColumnHeaderState multiColumnHeader, bool isReference = false) : base(state, new MultiColumnHeader(multiColumnHeader))
        {
            showBorder = true;
            columnIndexForTreeFoldouts = 0;

            this.assetMap = assetMap;
            this.isReference = isReference;
        }

        public void SetRoot(AssetMapData rootAssetData)
        {
            this.rootAssetData = rootAssetData;
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem(-1, -1);

            AssetMapTreeViewItem rootItem = new AssetMapTreeViewItem(this, rootAssetData, 0);
            root.AddChild(rootItem);

            rootItem.CheckChildren();
            SetExpanded(rootItem.id, true);

            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = args.item as AssetMapTreeViewItem;
            for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
            {
                CellGUI(args.GetCellRect(i), item, args.GetColumn(i), ref args);
            }
        }

        private void CellGUI(Rect cellRect, AssetMapTreeViewItem item, int column, ref RowGUIArgs args)
        {
            CenterRectUsingSingleLineHeight(ref cellRect);

            switch ((ColumnId)column)
            {
                case ColumnId.NAME:
                    {
                        float indent = GetContentIndent(item) + extraSpaceBeforeIconAndLabel;
                        cellRect.xMin += indent;

                        if (item.assetObject != null)
                        {
                            UnityEngine.GUI.Label(cellRect, item.assetObject.name);
                        }
                    }
                    break;
                case ColumnId.TYPE:
                    {

                        if (item.typeIcon != null)
                        {
                            UnityEngine.GUI.DrawTexture(cellRect, item.typeIcon, ScaleMode.ScaleToFit, true);
                        }
                    }

                    break;
            }
        }

        public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState()
        {
            return new MultiColumnHeaderState(GetColumns());
        }

        private static MultiColumnHeaderState.Column[] GetColumns()
        {
            var retVal = new[]
            {
                new MultiColumnHeaderState.Column(),
                new MultiColumnHeaderState.Column(),
            };

            int counter = 0;

            retVal[counter].headerContent = UiContent.TreeTabNameContents;
            retVal[counter].minWidth = 100;
            retVal[counter].width = 160;
            retVal[counter].maxWidth = 10000;
            retVal[counter].headerTextAlignment = TextAlignment.Left;
            retVal[counter].canSort = true;
            retVal[counter].autoResize = true;
            counter++;

            retVal[counter].headerContent = UiContent.TreeTabTypeContents;
            retVal[counter].minWidth = 20;
            retVal[counter].width = 20;
            retVal[counter].maxWidth = 20;
            retVal[counter].headerTextAlignment = TextAlignment.Left;
            retVal[counter].canSort = false;
            retVal[counter].autoResize = true;
            counter++;

            return retVal;
        }

        protected override bool CanBeParent(TreeViewItem item)
        {
            return true;
        }

        protected override bool CanChangeExpandedState(TreeViewItem item)
        {
            if (item.hasChildren)
            {
                return true;
            }

            if (item is AssetMapTreeViewItem assetMapItem)
            {
                AssetMapData data = assetMapItem.data;
                if (isReference == true)
                {
                    return data.referenceLinks.Count > 0;
                }
                else
                {
                    return data.dependencyLinks.Count > 0;
                }
            }

            return false;
        }

        protected override void ExpandedStateChanged()
        {
            foreach (var id in state.expandedIDs)
            {
                var item = FindItem(id, rootItem);
                if (item is AssetMapTreeViewItem assetMapItem)
                {
                    if (!assetMapItem.checkedForChildren)
                    {
                        assetMapItem.CheckChildren();
                    }
                }
            }
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            if(selectedIds.Count > 0)
            {
                int id = selectedIds[0];

                string guid;
                long localid;
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(id, out guid, out localid) == true)
                {
                    assetMap.OnClick_AssetData(guid);

                    EditorGUIUtility.PingObject(id);
                }
            }
        }
        
        protected override void SingleClickedItem(int id)
        {
            string guid;
            long localid;
            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(id, out guid, out localid) == true)
            {
                assetMap.OnClick_AssetData(guid);

                EditorGUIUtility.PingObject(id);
            }
        }
    }

    public class AssetMapTreeGUI
    {
        private TreeViewState m_TreeState;
        private AssetMapTreeView m_EntryTree;
        private SearchField m_SearchField;
        private MultiColumnHeaderState m_Mchs;

        public void Init(AssetMapGUI assetMap, bool isReference)
        {
            if (m_EntryTree == null)
            {
                if (m_TreeState == null)
                {
                    m_TreeState = new TreeViewState();
                }

                var headerState = AssetMapTreeView.CreateDefaultMultiColumnHeaderState();
                if (MultiColumnHeaderState.CanOverwriteSerializedFields(m_Mchs, headerState) == true)
                {
                    MultiColumnHeaderState.OverwriteSerializedFields(m_Mchs, headerState);
                }
                m_Mchs = headerState;

                m_SearchField = new SearchField();
                m_EntryTree = new AssetMapTreeView(assetMap, m_TreeState, m_Mchs, isReference);
            }
        }

        public void SetRootObject(Object rootObject)
        {
            string guid;
            long localID;
            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(rootObject, out guid, out localID) == true)
            {
                m_EntryTree.SetRoot(GpmAssetManagementManager.GetAssetDataFromGUID(guid));
                m_EntryTree.Reload();
            }
        }

        public void Reload()
        {
            if (m_EntryTree != null)
            {
                m_EntryTree.Reload();
            }
        }

        public void OnGUI(Rect rt)
        {
            if (m_EntryTree != null)
            {
                m_EntryTree.OnGUI(rt);
            }
        }
    }
}