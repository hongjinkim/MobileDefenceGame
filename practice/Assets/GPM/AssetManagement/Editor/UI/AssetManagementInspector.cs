using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Gpm.AssetManagement.Ui
{
    [InitializeOnLoad]
    static class AssetManagementInspectorGUI
    {
        static AssetManagementInspectorGUI()
        {
            Editor.finishedDefaultHeaderGUI += OnPostHeaderGUI;
        }

        private static void OnPostHeaderGUI(Editor editor)
        {
            if(editor.targets.Length == 1)
            {

                if (EditorUtility.IsPersistent(editor.target) == true)
                {
                    if (AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(editor.target))==true)
                    {
                    }
                    else
                    {
                        if (GUILayout.Button(Strings.TEXT_SHOW_ASSETMAP) == true)
                        {
                            if(editor.target is AssetImporter importer)
                            {
                                AssetManagementWindow.Show(AssetDatabase.LoadMainAssetAtPath(importer.assetPath));
                            }
                            else
                            {
                                AssetManagementWindow.Show(editor.target);
                            }
                            
                        }
                    }
                }
            }
        }
    }
}