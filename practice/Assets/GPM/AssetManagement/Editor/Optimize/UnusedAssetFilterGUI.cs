using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Gpm.AssetManagement.Optimize.Ui
{
    using Gpm.AssetManagement.Const;
    public class UnusedAssetFilterGUI
    {
        private UnusedAssetFilter filter;
        private ReorderableList reorderableList;

        public void Init(UnusedAssetFilter filter)
        {
            this.filter = filter;

            reorderableList = new ReorderableList(filter.filterList, typeof(FilterPath));

            reorderableList.onAddCallback =
                (list) =>
                {
                    filter.filterList.Add(new FilterPath(""));
                    filter.SetDirty();
                };

            reorderableList.onChangedCallback =
                (list) =>
                {
                    filter.SetDirty();
                };

            reorderableList.drawElementCallback =
                 (Rect rect, int index, bool isActive, bool isFocused) =>
                 {
                     rect.y += 2;

                     bool checkChange = false;
                     if (GUI.changed == false)
                     {
                         checkChange = true;
                     }
                     string value = EditorGUI.TextField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                                         filter.filterList[index].filterPath);

                     if (checkChange == true &&
                        GUI.changed)
                     {
                         if (filter.filterList[index].filterPath.Equals(value) == false)
                         {
                             filter.filterList[index].filterPath = value;

                             filter.SetDirty();
                         }
                         GUI.changed = false;
                     }
                 };
        }

        public void OnGUI()
        {
            using (new GUILayout.VerticalScope())
            {
                bool checkChange = false;
                if (GUI.changed == false)
                {
                    checkChange = true;
                }

                filter.filterBuildIn = Ui.Toggle(filter.filterBuildIn, Strings.KEY_FILTER_BUILTIN);
                filter.filterAssetbundle = Ui.Toggle(filter.filterAssetbundle, Strings.KEY_FILTER_ASSETBUNDLE);
                filter.filterPathList = Ui.Toggle(filter.filterPathList, Strings.KEY_FILTER_PATHLIST);

                if (checkChange == true &&
                    GUI.changed)
                {
                    filter.SetDirty();
                }


                reorderableList.DoLayoutList();
            }
        }
    }
}
