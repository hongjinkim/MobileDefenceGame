using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

namespace Gpm.AssetManagement.Optimize.Ui
{
    using Gpm.AssetManagement.Const;

    internal class OptimizeTreeItem : TreeViewItem
    {
        protected UnusedAssetTreeView rootTree;

        public bool check = true;
        public string path;
        public string name;
        public string ext;
        public Texture typeIcon;
        public long size;
        public string sizeText;

        public bool filter = false;
        public bool removed = false;

        public OptimizeTreeItem(UnusedAssetTreeView rootTree, string path, int id, int d) : base(id, d)
        {
            this.rootTree = rootTree;

            this.path = path;
            this.name = System.IO.Path.GetFileName(path);
            this.ext = System.IO.Path.GetExtension(path);
            this.typeIcon = AssetDatabase.GetCachedIcon(path);

            System.IO.FileInfo fileInfo = new System.IO.FileInfo(path);
            size = fileInfo.Length;
            double kb = size / 1024.0;
            double mb = (double)((kb / 1024.0));

            
            if (mb > 1)
            {
                this.sizeText = string.Format("{0:0.0} mb", mb);
            }
            else if(kb > 1)
            {
                this.sizeText = string.Format("{0:0.0} kb", kb);
            }
            else
            {
                this.sizeText = string.Format("{0} bytes", size);
            }
        }

        public virtual bool CanExpanded()
        {
            return hasChildren;
        }

        public void OnDoubleClick()
        {
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(path);
        }

        public void RowGUI()
        {
        }

        public void CellGUI(Rect cellRect, UnusedAssetTreeView.ColumnId column)
        {
            switch (column)
            {
                case UnusedAssetTreeView.ColumnId.CHECK:
                    {
                        using (new EditorGUI.DisabledGroupScope(filter || removed))
                        {
                            bool change = UnityEngine.GUI.Toggle(cellRect, check, "");
                            if (check != change)
                            {
                                check = change;

                                if (rootTree.IsSelected(id) == true)
                                {
                                    var itemList = rootTree.GetRows();
                                    if (itemList != null)
                                    {
                                        for (int i = 0; i < itemList.Count; i++)
                                        {
                                            var item = itemList[i];
                                            if (rootTree.IsSelected(item.id) == true)
                                            {
                                                if (item is OptimizeTreeItem optimizeItem)
                                                {
                                                    optimizeItem.check = check;
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                        }
                        
                    }
                    break;
                case UnusedAssetTreeView.ColumnId.NAME:
                    {
                        using (new EditorGUI.DisabledGroupScope(filter || removed))
                        {
                            string displayName = name;
                            if (filter == true)
                            {
                                displayName += Ui.GetString(Strings.KEY_TAG_FILTER);
                            }
                            if (removed == true)
                            {
                                displayName += Ui.GetString(Strings.KEY_TAG_REMOVED);
                            }
                            UnityEngine.GUI.Label(cellRect, new GUIContent(displayName, typeIcon));
                        }   
                    }
                    break;
                case UnusedAssetTreeView.ColumnId.TYPE:
                    {
                        if (typeIcon != null)
                        {
                            UnityEngine.GUI.DrawTexture(cellRect, typeIcon, ScaleMode.ScaleToFit, true);
                        }
                    }

                    break;
                case UnusedAssetTreeView.ColumnId.SIZE:
                    {
                        using (new EditorGUI.DisabledGroupScope(filter || removed))
                        {
                            UnityEngine.GUI.Label(cellRect, new GUIContent(sizeText));
                        }
                    }
                    break;
                case UnusedAssetTreeView.ColumnId.PATH:
                    {
                        using (new EditorGUI.DisabledGroupScope(filter || removed))
                        {
                            UnityEngine.GUI.Label(cellRect, path);
                        }
                    }
                    break;
                case UnusedAssetTreeView.ColumnId.FUNCTION:
                    {
                        Rect btnRect = cellRect;
                        btnRect.width = cellRect.width * 0.5f;
                        if (filter == false)
                        {
                            if (GUI.Button(btnRect, Ui.GetString(Strings.KEY_FILTER)) == true)
                            {
                                AddFilter();
                            }
                        }
                        else
                        {
                            if (GUI.Button(btnRect, Ui.GetString(Strings.KEY_UNFILTER)) == true)
                            {
                                UnFilter();
                            }
                        }
                           
                        btnRect.x += btnRect.width;
                        if(removed == false)
                        {
                            if (GUI.Button(btnRect, Ui.GetString(Strings.KEY_REMOVE)) == true)
                            {
                                bool check = EditorUtility.DisplayDialog(Ui.GetString(Strings.KEY_REMOVE), Ui.GetString(Strings.KEY_CHECK_REMOVE), Ui.GetString(Strings.KEY_OK), Ui.GetString(Strings.KEY_CANCEL));
                                if(check == true)
                                {
                                    Remove();

                                    AssetDatabase.Refresh();
                                }
                            }
                        }
                        else
                        {
                            if (GUI.Button(btnRect, Ui.GetString(Strings.KEY_RESTORE)) == true)
                            {
                                Restore();
                                
                                AssetDatabase.Refresh();
                            }
                        }
                        
                    }
                    break;
            }
        }

        public void AddFilter()
        {
            rootTree.AddFilter(path);
            filter = true;
        }

        public void UnFilter()
        {
            rootTree.RemoveFilter(path);
            filter = false;
        }

        public void Remove()
        {
            if (System.IO.File.Exists(path) == true)
            {
                string trashPath = path.Replace(Constants.PATH_ASSET, Constants.PATH_TRASH);

                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(trashPath));

                if (System.IO.File.Exists(trashPath) == true)
                {
                    System.IO.File.Delete(trashPath);
                }
                System.IO.File.Move(path, trashPath);

                string metaPath = path + ".meta";
                if (System.IO.File.Exists(metaPath) == true)
                {
                    trashPath = metaPath.Replace(Constants.PATH_ASSET, Constants.PATH_TRASH);
                    if (System.IO.File.Exists(trashPath) == true)
                    {
                        System.IO.File.Delete(trashPath);
                    }
                    System.IO.File.Move(metaPath, trashPath);
                }
            }

            removed = true;
        }

        public void Restore()
        {
            string trashPath = path.Replace(Constants.PATH_ASSET, Constants.PATH_TRASH);
            if (System.IO.File.Exists(trashPath) == true)
            {
                if (System.IO.File.Exists(path) == true)
                {
                    System.IO.File.Delete(path);
                }
                System.IO.File.Move(trashPath, path);

                string trashMetaPath = trashPath + ".meta";
                if (System.IO.File.Exists(trashMetaPath) == true)
                {
                    string metaPath = trashMetaPath.Replace(Constants.PATH_TRASH, Constants.PATH_ASSET);
                    if (System.IO.File.Exists(metaPath) == true)
                    {
                        System.IO.File.Delete(metaPath);
                    }
                    System.IO.File.Move(trashMetaPath, metaPath);
                }
            }
            removed = false;
        }
    }

}