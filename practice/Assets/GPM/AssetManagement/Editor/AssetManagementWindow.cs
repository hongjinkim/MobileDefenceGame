using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Gpm.AssetManagement.Ui
{
    using Gpm.AssetManagement.Const;
    using Gpm.AssetManagement.AssetMap.Ui;
    using Gpm.AssetManagement.AssetIssue.Ui;
    using Gpm.AssetManagement.Optimize.Ui;
    using Gpm.AssetManagement.AssetFind.Ui;

    public class AssetManagementWindow : EditorWindow
    {
        private enum Menu
        {
            ASSET_MAP,
            ISSUE_CHECK,
            UNUSED_ASSET,
        }
        private Menu menu = Menu.ASSET_MAP;
        private AssetMapGUI assetMap;
        private AssetIssueTreeGUI issueGUI;
        private UnusedAssetGUI UnusedAssetGUI;
        

        static public void Show(Object select)
        {
            AssetManagementLanguage.Load(
                () =>
                {
                    AssetManagementWindow w = EditorWindow.GetWindow<AssetManagementWindow>(false, Ui.GetString(Strings.KEY_TITLE_BAR), true);

                    w.assetMap = new AssetMapGUI(w);
                    w.assetMap.SetRootObject(select);

                    w.Show();
                });
        }

        private void OnEnable()
        {
            AssetManagementLanguage.Load(() =>
            {
                titleContent = Ui.GetContent(Strings.KEY_TITLE_BAR);
            });
        }

        private void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope(UiStyle.Toolbar))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    Ui.Label(Strings.KEY_TITLE_LIST);

                    AssetManagementLanguage.OnGUI( () =>
                    {
                        titleContent = Ui.GetContent(Strings.KEY_TITLE_BAR);
                    });
                }
            }

            using (new EditorGUILayout.VerticalScope(UiStyle.GroupBox, GUILayout.ExpandHeight(true)))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new EditorGUILayout.VerticalScope(GUILayout.Width(200)))
                    {
                        string style = menu == Menu.ASSET_MAP ? "SelectionRect" : "TextArea";
                        using (new EditorGUILayout.HorizontalScope(style, GUILayout.MinHeight(24f)))
                        {
                            if (Ui.Button(Strings.KEY_ASSETMAP, EditorStyles.label) == true)
                            {
                                menu = Menu.ASSET_MAP;
                            }
                        }
                        
                        style = menu == Menu.ISSUE_CHECK ? "SelectionRect" : "TextArea";
                        using (new EditorGUILayout.HorizontalScope(style, GUILayout.MinHeight(24f)))
                        {
                            if (Ui.Button(Strings.KEY_ISSUECHECK, EditorStyles.label) == true)
                            {
                                menu = Menu.ISSUE_CHECK;

                                if (issueGUI == null)
                                {
                                    issueGUI = new AssetIssueTreeGUI();
                                    issueGUI.Init();
                                }
                                issueGUI.CheckIssueAll();
                            }
                        }

                        style = menu == Menu.UNUSED_ASSET ? "SelectionRect" : "TextArea";
                        using (new EditorGUILayout.HorizontalScope(style, GUILayout.MinHeight(24f)))
                        {
                            if (Ui.Button(Strings.KEY_UNUSEDASSET, EditorStyles.label) == true)
                            {
                                menu = Menu.UNUSED_ASSET;

                                if(UnusedAssetGUI == null)
                                {
                                    UnusedAssetGUI = new UnusedAssetGUI();
                                    UnusedAssetGUI.Init();
                                }
                            }
                        }

                        GUILayout.Space(20);

                        if (Ui.Button(Strings.KEY_REFERENCE_FIND) == true)
                        {
                            AssetFindWindow.Find(null);
                        }
                    }

                    if (GpmAssetManagementManager.Enable == false)
                    {
                        using (new EditorGUILayout.VerticalScope())
                        {
                            GUILayout.FlexibleSpace();

                            using (new EditorGUILayout.HorizontalScope())
                            {
                                GUILayout.FlexibleSpace();

                                using (new EditorGUILayout.VerticalScope())
                                {
                                    Ui.Label(Strings.KEY_ASSETMANAGEMENT_ENABLE_DESC);

                                    if (Ui.Button(Strings.KEY_ASSETMANAGEMENT_ENABLE) == true)
                                    {
                                        GpmAssetManagementManager.Enable = true;
                                    }
                                }

                                GUILayout.FlexibleSpace();
                            }
                            GUILayout.FlexibleSpace();
                        }
                    }
                    else
                    {
                        switch (menu)
                        {
                            case Menu.ASSET_MAP:
                                {
                                    if (assetMap == null)
                                    {
                                        assetMap = new AssetMapGUI(this);
                                    }

                                    if (assetMap.window == null)
                                    {
                                        assetMap.window = this;
                                    }
                                    
                                    using (new EditorGUILayout.HorizontalScope())
                                    {
                                        assetMap.OnGUI();
                                    }
                                }
                                break;

                            case Menu.ISSUE_CHECK:
                                {
                                    if (issueGUI == null)
                                    {
                                        issueGUI = new AssetIssueTreeGUI();
                                        issueGUI.Init();


                                        issueGUI.CheckIssueAll();
                                    }

                                    using (new EditorGUILayout.VerticalScope())
                                    {
                                        Ui.Label(Strings.KEY_ISSUECHECK, EditorStyles.boldLabel);
                                        if (Ui.Button(Strings.KEY_FIND, GUILayout.Width(120)) == true)
                                        {
                                            issueGUI.CheckIssueAll();
                                        }
                                        issueGUI.OnGUI();
                                    }
                                }
                                break;
                            case Menu.UNUSED_ASSET:
                                {
                                    if (UnusedAssetGUI == null)
                                    {
                                        UnusedAssetGUI = new UnusedAssetGUI();
                                        UnusedAssetGUI.Init();
                                    }
                                    UnusedAssetGUI.OnGUI();
                                }
                                break;
                                
                        }
                    }
                }

                    
            }
        }
    }
}
