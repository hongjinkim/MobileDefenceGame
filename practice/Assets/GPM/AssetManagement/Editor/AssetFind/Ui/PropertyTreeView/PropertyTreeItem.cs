

using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;

namespace Gpm.AssetManagement.AssetFind.Ui.PropertyTreeView
{
    internal abstract class PropertyTreeItem : TreeViewItem
    {
        protected PropertyTreeView rootTree;

        public PropertyTreeItem(PropertyTreeView rootTree, int id, int d) : base(id, d)
        {
            this.rootTree = rootTree;
        }

        public virtual bool CanExpanded()
        {
            return hasChildren;
        }

        public virtual void OnExpanded(bool expand)
        {
        }

        public virtual void OnClick()
        {
            if (id != 0)
            {
                EditorGUIUtility.PingObject(id);
            }
        }

        public virtual void OnDoubleClick()
        {
            if (id != 0)
            {
                if (rootTree.IsExpanded(id) == false)
                {
                    rootTree.SetExpanded(id, true);
                }

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

        public virtual void RowGUI()
        {
        }

        public abstract void CellGUI(Rect cellRect, PropertyTreeView.ColumnId column);
    }

}