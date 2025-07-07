using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Linq;

namespace Gpm.AssetManagement.Optimize.Ui
{
    using Gpm.AssetManagement.Const;
    using Gpm.AssetManagement.AssetMap;

    public class UnusedAssetTreeView : TreeView
    {
        public enum ColumnId
        {
            CHECK,
            NAME,
            TYPE,
            SIZE,
            PATH,
            FUNCTION,
        }

        ColumnId[] m_SortOptions =
        {
            ColumnId.CHECK,
            ColumnId.NAME,
            ColumnId.TYPE,
            ColumnId.SIZE,
            ColumnId.PATH,
            ColumnId.FUNCTION
        };

        public UnusedAssetFilter filter;

        public UnusedAssetTreeView(TreeViewState state, MultiColumnHeaderState mchs, UnusedAssetFilter filter) : base(state, new MultiColumnHeader(mchs))
        {
            showBorder = true;
            columnIndexForTreeFoldouts = 0;

            this.filter = filter;

            this.multiColumnHeader.sortingChanged += OnSortingChanged;
        }

        private void OnSortingChanged(MultiColumnHeader mch)
        {
            SortChildren(rootItem);
            Reload();
        }

        private bool hasData = false;

        public void Find()
        {
            hasData = false;
            pathList.Clear();
            Reload();
        }
        
        List<string> pathList = new List<string>();
        void CreateList()
        {
            if(filter != null)
            {
                filter.Init();
            }   

            Dictionary<string, AssetMapData> dicAssetMapData = GpmAssetManagementManager.GetAssetDataDictionary();

            int index = 0;
            foreach (var pair in dicAssetMapData)
            {
                if (pair.Value.referenceLinks.Count == 0)
                {
                    if (Ui.CheckPassByTime(Constants.CHECK_TIME_PROGRESS) == true)
                    {
                        float rate = (float)index / (float)dicAssetMapData.Count;
                        EditorUtility.DisplayProgressBar(Constants.SERVICE_NAME, string.Format(Constants.FORMAT_UNUSEDASSET_CHECK, index, dicAssetMapData.Count), rate);
                    }

                    string path = pair.Value.GetPath();


                    if (string.IsNullOrEmpty(path) == false)
                    {
                        if (path.StartsWith(Constants.PATH_ASSET) == false)
                        {
                            continue;
                        }

                        if (filter != null)
                        {
                            if (filter.IsFilter(path) == false)
                            {
                                continue;
                            }
                        }

                        pathList.Add(path);
                    }
                }

                index++;
            }
            EditorUtility.ClearProgressBar();
        }

        protected override TreeViewItem BuildRoot()
        {
            if (hasData == false)
            {
                CreateList();
            }

            hasData = false;

            var root = new TreeViewItem(-1, -1);            
            
            for (int i =0;i<pathList.Count;i++)
            {
                root.AddChild(new OptimizeTreeItem(this, pathList[i], i, 1));

                hasData = true;
            }

            SetupDepthsFromParentsAndChildren(root);

            return root;
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            if (string.IsNullOrEmpty(searchString) == false)
            {
                var rows = base.BuildRows(root);
                SortHierarchical(rows);
                return rows;
            }

            SortChildren(root);
            return base.BuildRows(root);
        }

        private void SortChildren(TreeViewItem root)
        {
            if (root.hasChildren == false)
            {
                return;
            }

            SortHierarchical(root.children);
        }

        private void SortHierarchical(IList<TreeViewItem> children)
        {
            if (children == null)
            {
                return;
            }

            var sortedColumns = multiColumnHeader.state.sortedColumns;
            if (sortedColumns.Length == 0)
            {
                return;
            }

            List<OptimizeTreeItem> kids = new List<OptimizeTreeItem>();
            List<TreeViewItem> copy = new List<TreeViewItem>(children);
            children.Clear();
            foreach (var c in copy)
            {
                var child = c as OptimizeTreeItem;
                if (child != null)
                {
                    kids.Add(child);
                }
                else
                {
                    children.Add(c);
                }
            }

            ColumnId col = m_SortOptions[sortedColumns[0]];
            bool ascending = multiColumnHeader.IsSortedAscending(sortedColumns[0]);

            IEnumerable<OptimizeTreeItem> orderedKids = kids;
            switch (col)
            {
                case ColumnId.NAME:
                    {
                        if (ascending == true)
                        {
                            orderedKids = kids.OrderBy(l =>
                            {
                                return l.name;
                            });
                        }
                        else
                        {
                            orderedKids = kids.OrderByDescending(l =>
                            {
                                return l.name;
                            });
                        }
                    }
                    break;
                case ColumnId.TYPE:
                    {
                        if (ascending == true)
                        {
                            orderedKids = kids.OrderBy(l =>
                            {
                                return l.ext;
                            });

                        }
                        else
                        {
                            orderedKids = kids.OrderByDescending(l =>
                            {
                                return l.ext;
                            });
                        }
                    }
                    break;
                case ColumnId.SIZE:
                    {
                        if (ascending == true)
                        {
                            orderedKids = kids.OrderByDescending(l =>
                            {
                                return l.size;
                            });
                            
                        }
                        else
                        {
                            orderedKids = kids.OrderBy(l =>
                            {
                                return l.size;
                            });
                        }
                    }
                    break;
                case ColumnId.PATH:
                    {
                        if (ascending == true)
                        {
                            orderedKids = kids.OrderBy(l =>
                            {
                                return l.path;
                            });
                        }
                        else
                        {
                            orderedKids = kids.OrderByDescending(l =>
                            {
                                return l.path;
                            });
                        }
                    }
                    break;
                default:
                    {
                        if(ascending == true)
                        {
                            orderedKids = kids.OrderBy(l =>
                            {
                                return l.displayName;
                            });
                        }
                        else
                        {
                            orderedKids = kids.OrderByDescending(l =>
                            {
                                return l.displayName;
                            });
                        }
                        
                    }
                    break;
            }

            foreach (var o in orderedKids)
            {
                children.Add(o);
            }


            foreach (var child in children)
            {
                if (child != null)
                {
                    SortHierarchical(child.children);
                }
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            if (args.item is OptimizeTreeItem propertyItem)
            {
                for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
                {
                    CellGUI(args.GetCellRect(i), propertyItem, args.GetColumn(i), ref args);
                }
            }
            else
            {
                UnityEngine.GUI.Label(args.GetCellRect(0), Ui.GetString(args.item.displayName));
            }
        }
        
        private void CellGUI(Rect cellRect, OptimizeTreeItem propertyItem, int column, ref RowGUIArgs args)
        {
            CenterRectUsingSingleLineHeight(ref cellRect);

            if (column == 0)
            {
                float indent = GetContentIndent(args.item) + extraSpaceBeforeIconAndLabel;
                cellRect.xMin += indent;
            }

            propertyItem.CellGUI(cellRect, (ColumnId)column);
        }

        protected override void ContextClickedItem(int id)
        {
            bool checkAble = false;
            bool unCheckAble = false;

            bool removeAble = false;
            bool restoreAble = false;

            bool filterAble = false;
            bool unFilterAble = false;

            List<OptimizeTreeItem> selectedNodes = new List<OptimizeTreeItem>();
            foreach (var nodeId in GetSelection())
            {
                var item = FindItem(nodeId, rootItem) as OptimizeTreeItem;
                if (item != null)
                {
                    if(item.check == true)
                    {
                        unCheckAble = true;
                    }
                    else
                    {
                        checkAble = true;
                    }

                    if(item.removed == true)
                    {
                        restoreAble = true;
                    }
                    else
                    {
                        removeAble = true;
                    }

                    if (item.filter == true)
                    {
                        unFilterAble = true;
                    }
                    else
                    {
                        filterAble = true;
                    }

                    selectedNodes.Add(item);
                }
            }

            if (selectedNodes.Count == 0)
            {
                return;
            }

            GenericMenu menu = new GenericMenu();

            if (checkAble == true)
            {
                menu.AddItem(new GUIContent(Ui.GetString(Strings.KEY_CHECK_ASSETS)), false, CheckAssets, selectedNodes);
            }

            if (unCheckAble == true)
            {
                menu.AddItem(new GUIContent(Ui.GetString(Strings.KEY_UNCHECK_ASSETS)), false, UnCheckAssets, selectedNodes);
            }

            if (filterAble == true)
            {
                menu.AddItem(new GUIContent(Ui.GetString(Strings.KEY_FILTER_PATHS)), false, FilterPaths, selectedNodes);
            }

            if (unFilterAble == true)
            {
                menu.AddItem(new GUIContent(Ui.GetString(Strings.KEY_UNFILTER_PATHS)), false, UnFilterPaths, selectedNodes);
            }

            if (removeAble == true)
            {
                menu.AddItem(new GUIContent(Ui.GetString(Strings.KEY_REMOVE_ASSETS)), false, RemoveAssets, selectedNodes);
            }

            if (restoreAble == true)
            {
                menu.AddItem(new GUIContent(Ui.GetString(Strings.KEY_RESTORE_ASSETS)), false, RestoreAssets, selectedNodes);
            }

            menu.ShowAsContext();
        }

        public void CheckAssets(object context)
        {
            List<OptimizeTreeItem> itemList = context as List<OptimizeTreeItem>;
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].check = true;
            }
        }

        public void UnCheckAssets(object context)
        {
            List<OptimizeTreeItem> itemList = context as List<OptimizeTreeItem>;
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].check = false;
            }
        }

        public void FilterPaths(object context)
        {
            List<OptimizeTreeItem> itemList = context as List<OptimizeTreeItem>;
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].AddFilter();
                itemList[i].filter = true;
            }

            AssetDatabase.Refresh();
        }

        public void UnFilterPaths(object context)
        {
            List<OptimizeTreeItem> itemList = context as List<OptimizeTreeItem>;
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].UnFilter();
                itemList[i].filter = false;
            }

            AssetDatabase.Refresh();
        }
        public void RemoveAssets(object context)
        {
            List<OptimizeTreeItem> itemList = context as List<OptimizeTreeItem>;
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].Remove();
                itemList[i].removed = true;
            }

            AssetDatabase.Refresh();
        }

        public void RestoreAssets(object context)
        {
            List<OptimizeTreeItem> itemList = context as List<OptimizeTreeItem>;
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].Restore();
                itemList[i].removed = false;

            }

            AssetDatabase.Refresh();
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
                new MultiColumnHeaderState.Column(),
                new MultiColumnHeaderState.Column(),
                new MultiColumnHeaderState.Column(),
                new MultiColumnHeaderState.Column(),
            };

            int counter = 0;

            retVal[counter].headerContent = new GUIContent("Check", "Check");
            retVal[counter].minWidth = 40;
            retVal[counter].width = 40;
            retVal[counter].maxWidth = 40;
            retVal[counter].headerTextAlignment = TextAlignment.Left;
            retVal[counter].canSort = false;
            retVal[counter].autoResize = false
            ;
            counter++;

            retVal[counter].headerContent = UiContent.TreeTabNameContents;
            retVal[counter].minWidth = 130;
            retVal[counter].width = 230;
            retVal[counter].maxWidth = 400;
            retVal[counter].headerTextAlignment = TextAlignment.Left;
            retVal[counter].canSort = true;
            retVal[counter].autoResize = true;
            counter++;

            retVal[counter].headerContent = UiContent.TreeTabTypeContents;
            retVal[counter].minWidth = 20;
            retVal[counter].width = 20;
            retVal[counter].maxWidth = 20;
            retVal[counter].headerTextAlignment = TextAlignment.Left;
            retVal[counter].canSort = true;
            retVal[counter].autoResize = true;
            counter++;

            retVal[counter].headerContent = new GUIContent("Size", "Asset Size");
            retVal[counter].minWidth = 80;
            retVal[counter].width = 80;
            retVal[counter].maxWidth = 100;
            retVal[counter].headerTextAlignment = TextAlignment.Left;
            retVal[counter].canSort = true;
            retVal[counter].sortedAscending = true;
            retVal[counter].autoResize = true;
            counter++;

            retVal[counter].headerContent = new GUIContent("Path", "Path");
            retVal[counter].minWidth = 100;
            retVal[counter].width = 310;
            retVal[counter].maxWidth = 1000;
            retVal[counter].headerTextAlignment = TextAlignment.Left;
            retVal[counter].canSort = true;
            retVal[counter].autoResize = true;
            counter++;

            retVal[counter].headerContent = UiContent.TreeTabFunctionContents;
            retVal[counter].minWidth = 100;
            retVal[counter].width = 160;
            retVal[counter].maxWidth = 200;
            retVal[counter].headerTextAlignment = TextAlignment.Left;
            retVal[counter].canSort = false;
            retVal[counter].autoResize = true;
            counter++;

            return retVal;
        }

        public bool HasData()
        {
            return hasData;
        }

        protected override void DoubleClickedItem(int id)
        {
            if (FindItem(id, rootItem) is OptimizeTreeItem propertyItem)
            {
                propertyItem.OnDoubleClick();
            }
        }

        public void AddFilter(string path)
        {
            filter.filterList.Add(new FilterPath(path));
            filter.SetDirty();
        }

        public void RemoveFilter(string path)
        {
            filter.filterList.RemoveAll(filterOption =>
            {
                return filterOption.filterPath.Equals(path);
            });
            filter.SetDirty();
        }
    }
}