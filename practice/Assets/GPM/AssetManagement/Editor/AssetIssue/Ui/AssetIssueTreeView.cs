using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

namespace Gpm.AssetManagement.AssetIssue.Ui
{
    using Gpm.AssetManagement.Const;
    using Gpm.AssetManagement.AssetMap;
    using Gpm.AssetManagement.AssetFind;
    using Gpm.AssetManagement.AssetFind.Ui.PropertyTreeView;

    class AssetIssueTreeViewItem : TreeViewItem
    {
        public string name;
        public Object asset;

        public Texture2D typeIcon;
        public string path;

        private FindModule module = null;
        public AssetIssueTreeViewItem(FindModule module) : base(module == null ? 0 : module.GetID(), 0)
        {
            this.module = module;

            this.asset = null;
            this.name = module.GetName();

            this.typeIcon = module.GetTypeIcon();

            foreach (var result in module.result)
            {
                AssetIssueTreeViewItem findRootObjectItem = new AssetIssueTreeViewItem(result.rootObject, 1);
                AddChild(findRootObjectItem);

                AssetIssueTreeViewItem pathItem = new AssetIssueTreeViewItem(result.rootObject, result.path, 2);
                findRootObjectItem.AddChild(pathItem);
            }
        }

        public AssetIssueTreeViewItem(Object asset, int d) : base(asset == null ? 0 : asset.GetInstanceID(), d)
        {
            this.asset = asset;
            this.name = asset.name;

            typeIcon = AssetPreview.GetMiniThumbnail(asset);
        }
        
        public AssetIssueTreeViewItem(Object asset, string path, int d) : base(asset == null ? 0 : asset.GetInstanceID(), d)
        {
            this.asset = asset;
            this.name = path;
            this.path = path;
        }
    }

    public class AssetIssueTreeView : TreeView
    {
        private enum ColumnId
        {
            NAME,
            TYPE,
            FUNCTION,
        }

        public AssetIssueTreeView(TreeViewState state, MultiColumnHeaderState multiColumnHeader) : base(state, new MultiColumnHeader(multiColumnHeader))
        {
            showBorder = true;
            columnIndexForTreeFoldouts = 0;
        }

        private PropertyFinder moduleFinder = new PropertyFinder();
        public void Setting(PropertyFinder value)
        {
            moduleFinder = value;
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem(-1, -1);

            for (int i = 0; i < moduleFinder.moduleList.Count; i++)
            {
                AssetIssueTreeViewItem objectRoot = new AssetIssueTreeViewItem(moduleFinder.moduleList[i]);
                root.AddChild(objectRoot);
            }

            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = args.item as AssetIssueTreeViewItem;
            for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
            {
                CellGUI(args.GetCellRect(i), item, args.GetColumn(i), ref args);
            }
        }

        private void CellGUI(Rect cellRect, AssetIssueTreeViewItem item, int column, ref RowGUIArgs args)
        {
            CenterRectUsingSingleLineHeight(ref cellRect);

            switch ((ColumnId)column)
            {
                case ColumnId.NAME:
                    {
                        float indent = GetContentIndent(item) + extraSpaceBeforeIconAndLabel;
                        cellRect.xMin += indent;

                        if (item.typeIcon != null)
                        {
                            UnityEngine.GUI.Label(cellRect, new GUIContent(item.name, item.typeIcon));
                        }
                        else
                        {
                            UnityEngine.GUI.Label(cellRect, item.name);
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
                case ColumnId.FUNCTION:
                    {
                        if (string.IsNullOrEmpty(item.path) == false)
                        {
                            if (item.asset != null)
                            {
                                SerializedObject serialized_object = new SerializedObject(item.asset);
                                SerializedProperty serialized_property = serialized_object.FindProperty(item.path);
                                if(serialized_property != null)
                                {
                                    EditorGUI.ObjectField(cellRect, serialized_property, new GUIContent());

                                    if (serialized_object.ApplyModifiedProperties() == true)
                                    {
                                        serialized_object.Update();
                                    }
                                }
                            }
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
            new MultiColumnHeaderState.Column(),
        };

            int counter = 0;

            retVal[counter].headerContent = UiContent.TreeTabNameContents;
            retVal[counter].minWidth = 100;
            retVal[counter].width = 260;
            retVal[counter].maxWidth = 1000;
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

        protected override void SingleClickedItem(int id)
        {
            if (id != 0)
            {
                EditorGUIUtility.PingObject(id);
            }
        }

        protected override void DoubleClickedItem(int id)
        {
            if (id != 0)
            {
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
    }

    public class AssetIssueTreeGUI
    {
        private bool hasIssue = false;

        private TreeViewState m_TreeState;
        private PropertyTreeView m_EntryTree;
        private SearchField m_SearchField;
        private MultiColumnHeaderState m_Mchs;

        public void Init()
        {
            if (m_EntryTree == null)
            {
                if (m_TreeState == null)
                {
                    m_TreeState = new TreeViewState();
                }

                var headerState = PropertyTreeView.CreateDefaultMultiColumnHeaderState();
                if (MultiColumnHeaderState.CanOverwriteSerializedFields(m_Mchs, headerState) == true)
                {
                    MultiColumnHeaderState.OverwriteSerializedFields(m_Mchs, headerState);
                }
                m_Mchs = headerState;

                m_SearchField = new SearchField();
                m_EntryTree = new PropertyTreeView(m_TreeState, m_Mchs);

                m_EntryTree.enableReplace = true;
            }
        }

        public void CheckIssue(Object obj)
        {
            FindModule module = PropertyFinder.GetModule(obj);
            module.SetMissingCondition();
            module.Find();

            PropertyFinder moduleFinder = new PropertyFinder();
            moduleFinder.moduleList.Add(module);

            m_EntryTree.Setting(moduleFinder);
            m_EntryTree.Reload();

            if (module.result.Count > 0)
            {
                m_EntryTree.ExpandAll();
            }

            hasIssue = true;
        }

        public void CheckIssueAll()
        {
            PropertyFinder moduleFinder = new PropertyFinder();

            int index = 0;
            foreach (string guid in GpmAssetManagementManager.cache.unKnownMissingGuid.ToArray())
            {
                if (Ui.CheckPassByTime(Constants.CHECK_TIME_PROGRESS) == true)
                {
                    float rate = (float)index / (float)GpmAssetManagementManager.cache.unKnownMissingGuid.Count;
                    EditorUtility.DisplayProgressBar(Constants.SERVICE_NAME, string.Format(Constants.FORMAT_UNKNOWN_CHECK, index, GpmAssetManagementManager.cache.unKnownMissingGuid.Count), rate);
                }
                AssetMapData data = GpmAssetManagementManager.GetAssetDataFromGUID(guid);

                if (data != null)
                {
                    if (data.hasMissing == AssetMapData.MissingState.UNKNOWN)
                    {
                        data.ReImport(true);
                    }
                }
                index++;
            }
            EditorUtility.ClearProgressBar();

            if (GpmAssetManagementManager.cache.bDirty == true)
            {
                GpmAssetManagementManager.cache.Save();
            }

            index = 0 ;
            foreach (string guid in GpmAssetManagementManager.cache.hasMissingAsset)
            {
                if (Ui.CheckPassByTime(Constants.CHECK_TIME_PROGRESS) == true)
                {
                    float rate = (float)index / (float)GpmAssetManagementManager.cache.hasMissingAsset.Count;
                    EditorUtility.DisplayProgressBar(Constants.SERVICE_NAME, string.Format(Constants.FORMAT_MISSING_CHECK, index, GpmAssetManagementManager.cache.hasMissingAsset.Count), rate);
                }

                string path = AssetDatabase.GUIDToAssetPath(guid);

                var obj = AssetDatabase.LoadMainAssetAtPath(path);

                if (obj != null)
                {
                    FindModule missingProperty = AssetFindUtility.FindMissingProperty(obj);

                    moduleFinder.moduleList.Add(missingProperty);
                    index++;
                }
            }
            EditorUtility.ClearProgressBar();

            m_EntryTree.Setting(moduleFinder);

            if (moduleFinder.moduleList.Count > 0)
            {
                m_EntryTree.Reload();
                //m_EntryTree.ExpandAll();

                hasIssue = true;
            }
            else
            {
                hasIssue = false;
            }
        }

        public void OnGUI()
        {
            if(hasIssue == true)
            {
                Rect controllRect = EditorGUILayout.GetControlRect(GUILayout.ExpandHeight(true));

                if (m_EntryTree != null)
                {
                    m_EntryTree.OnGUI(controllRect);
                }
            }
            else
            {
                Ui.Label(Strings.KEY_NO_ISSUE);
            }
            
        }
    }
}