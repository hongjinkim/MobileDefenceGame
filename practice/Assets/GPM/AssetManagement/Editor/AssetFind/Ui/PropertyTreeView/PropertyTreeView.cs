using UnityEngine;
using UnityEditor.IMGUI.Controls;

namespace Gpm.AssetManagement.AssetFind.Ui.PropertyTreeView
{
    using Gpm.AssetManagement.Const;

    public class PropertyTreeView : TreeView
    {
        public enum ColumnId
        {
            NAME,
            TYPE,
            FUNCTION,
        }

        public PropertyTreeView(TreeViewState state, MultiColumnHeaderState multiColumnHeader) : base(state, new MultiColumnHeader(multiColumnHeader))
        {
            showBorder = true;
            columnIndexForTreeFoldouts = 0;
        }

        private bool hasData = false;
        public bool enableReplace = false;
        private PropertyFinder moduleFinder = new PropertyFinder();
        
        public void Setting(PropertyFinder value)
        {
            if(moduleFinder != null)
            {
                moduleFinder.moduleAdded -= OnAddModule;
                moduleFinder.moduleRemoved -= OnRemoveModule;

                moduleFinder = null;
            }
            moduleFinder = value;

            moduleFinder.moduleAdded += OnAddModule;
            moduleFinder.moduleRemoved += OnRemoveModule;

            Reload();
        }

        public void OnAddModule(FindModule module)
        {
            if (isInitialized == true)
            {
                TreeItem.ObjectRootTreeItem objectRoot = CreateRootItem(module);
                rootItem.AddChild(objectRoot);

                SetExpanded(objectRoot.id, true);

                if(hasData == false)
                {
                    hasData = true;
                    Reload();
                }
                    
            }
        }

        public void OnRemoveModule(FindModule module)
        {
            if (isInitialized == true)
            {
                foreach (TreeViewItem item in rootItem.children)
                {
                    if (item.id == module.GetID())
                    {
                        SetExpandedRecursive(item.id, false);
                        SetExpanded(item.id, false);

                        rootItem.children.Remove(item);

                        SetExpanded(-1, false);
                        break;
                    }
                }

                if (rootItem.children.Count == 0)
                {
                    hasData = false;
                    Reload();
                }
            }
        }

        private TreeItem.ObjectRootTreeItem CreateRootItem(FindModule finder)
        {
            if(finder is HierarchySceneFindModule sceneFindModule)
            {
                return new TreeItem.HierarchySceneRootTreeItem(this, sceneFindModule);
            }
            else if (finder is ProjectSceneFindModule sceneAssetModule)
            {
                return new TreeItem.ProjectSceneRootTreeItem(this, sceneAssetModule);
            }
            else if(finder is HierarchyPrefabFindModule prefabStageModule)
            {
                return new TreeItem.HierarchyPrefabRootTreeItem(this, prefabStageModule);
            }
            else
            {
                return new TreeItem.ObjectRootTreeItem(this, finder);
            }
        }

        public void Clear()
        {
            moduleFinder.moduleList.Clear();
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem(-1, -1);

            if(moduleFinder.moduleList.Count > 0)
            {
                for (int i = 0; i < moduleFinder.moduleList.Count; i++)
                {
                    TreeItem.ObjectRootTreeItem objectRoot = CreateRootItem(moduleFinder.moduleList[i]);
                    root.AddChild(objectRoot);

                    if(IsExpanded(objectRoot.id) == true)
                    {
                        objectRoot.CheckChildren();
                    }
                    
                }
                hasData = true;
            }
            else
            {
                root.AddChild(new TreeItem.TextViewItem(Strings.KEY_NOT_FOUND_REFERENCE));
                hasData = false;
            }

            return root;
        }

        protected override void BeforeRowsGUI()
        {
            if(IsExpanded(-1) == false)
            {
                SetExpanded(-1, true);
                Repaint();
                return;
            }

            var itemList = GetRows();
            if (itemList != null)
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    var item = itemList[i];
                    if (item is PropertyTreeItem propertyItem)
                    {
                        propertyItem.RowGUI();
                    }
                }
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            if(args.item is PropertyTreeItem propertyItem)
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

        private void CellGUI(Rect cellRect, PropertyTreeItem propertyItem, int column, ref RowGUIArgs args)
        {
            CenterRectUsingSingleLineHeight(ref cellRect);

            if(column == 0)
            {
                float indent = GetContentIndent(args.item) + extraSpaceBeforeIconAndLabel;
                cellRect.xMin += indent;
            }

            propertyItem.CellGUI(cellRect, (ColumnId)column);
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
        };

            int counter = 0;

            retVal[counter].headerContent = UiContent.TreeTabNameContents;
            retVal[counter].minWidth = 100;
            retVal[counter].width = 180;
            retVal[counter].maxWidth = 300;
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

            retVal[counter].headerContent = UiContent.TreeTabFunctionContents;
            retVal[counter].minWidth = 200;
            retVal[counter].width = 250;
            retVal[counter].maxWidth = 1000;
            retVal[counter].headerTextAlignment = TextAlignment.Left;
            retVal[counter].canSort = true;
            retVal[counter].autoResize = true;
            counter++;
            
            return retVal;
        }

        public bool HasData()
        {
            return hasData;
        }

        protected override bool CanChangeExpandedState(TreeViewItem item)
        {
            if (item is PropertyTreeItem propertyItem)
            {
                return propertyItem.CanExpanded();
            }

            if (item.hasChildren)
            {
                return true;
            }

            return false;
        }
        
        protected override void ExpandedStateChanged()
        {
            var itemList = GetRows();
            if (itemList != null)
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    var item = itemList[i];
                    if (item is PropertyTreeItem propertyItem)
                    {
                        propertyItem.OnExpanded(IsExpanded(item.id));
                    }
                }
            }   
        }

        protected override void SingleClickedItem(int id)
        {
            if (FindItem(id, rootItem) is PropertyTreeItem propertyItem)
            {
                propertyItem.OnClick();
            }
        }

        protected override void DoubleClickedItem(int id)
        {
            if (FindItem(id, rootItem) is PropertyTreeItem propertyItem)
            {
                propertyItem.OnDoubleClick();
            }
        }
    }
}