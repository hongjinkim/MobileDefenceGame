using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Gpm.AssetManagement.AssetFind.Ui
{
    using Gpm.AssetManagement.Const;

    public class AssetFindWindow : EditorWindow
    {
        static public void Find(Object findTarget)
        {
            AssetManagementLanguage.Load(
                   () =>
                   {
                        AssetFindWindow w = EditorWindow.GetWindow<AssetFindWindow>(false, Ui.GetString(Strings.KEY_FIND_TITLE_BAR), true);
                        w.SetTarget(findTarget);
                        w.Show();
                   });
        }

        private HierachyPropertyTreeGUI m_HierarchyTreeGUI;

        private ProjectPropertyTreeGUI m_ProjectTreeGUI;


        private Object findTarget;
        private bool containSubObject = true;

        private bool find = true;

        private bool enableReplace = false;
        private bool enableReplace_project = false;

        private void OnEnable()
        {
            AssetManagementLanguage.Load(() =>
            {
                if (m_HierarchyTreeGUI == null)
                {
                    m_HierarchyTreeGUI = new HierachyPropertyTreeGUI();
                    m_HierarchyTreeGUI.Init(false);

                    find = true;
                }

                if (m_ProjectTreeGUI == null)
                {
                    m_ProjectTreeGUI = new ProjectPropertyTreeGUI();
                    m_ProjectTreeGUI.Init();

                    find = true;
                }
            });

            
        }

        public void SetTarget(Object target)
        {
            findTarget = target;
            find = true;
        }

        private Object replaceTarget;

        private void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope(UiStyle.Toolbar))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    Ui.Label(Strings.KEY_REFERENCE_FIND);

                    AssetManagementLanguage.OnGUI(() =>
                    {
                        titleContent = Ui.GetContent(Strings.KEY_FIND_TITLE_BAR);
                    });
                }
            }
     
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    using (new EditorGUILayout.VerticalScope(UiStyle.GroupBox, GUILayout.Height(60)))
                    {
                        Ui.Label(Strings.KEY_FIND_TARGET, EditorStyles.boldLabel);
                        Object changefindTarget = EditorGUILayout.ObjectField(findTarget, typeof(Object), true);
                        if (changefindTarget != findTarget)
                        {
                            SetTarget(changefindTarget);
                        }

                        containSubObject = Ui.Toggle(containSubObject, Strings.KEY_FIND_CONTAIN_SUBOBJECT);

                        if (Ui.Button(Strings.KEY_FIND) == true)
                        {
                            find = true;
                        }
                    }
                }


                /*
                /// <summary>
                ///  전체 Replace부분 임의로 주석처리
                /// </summary>
                //GUILayout.Label("Replace", EditorStyles.boldLabel);
                using (new EditorGUILayout.VerticalScope())
                {
                    GUILayout.Label("Replace", EditorStyles.boldLabel);
                    using (new EditorGUILayout.VerticalScope(UiStyle.GroupBox, GUILayout.Height(60)))
                    {
                        enableReplace = GUILayout.Toggle(enableReplace, "Enable");
                        using (new EditorGUI.DisabledGroupScope(enableReplace == false))
                        {
                            replaceTarget = EditorGUILayout.ObjectField(replaceTarget, typeof(Object), false);

                            if (GUILayout.Button("Replace All") == true)
                            {

                            }
                        }
                    }
                }*/
            }


            if (m_HierarchyTreeGUI == null)
            {
                m_HierarchyTreeGUI = new HierachyPropertyTreeGUI();
                m_HierarchyTreeGUI.Init(false);

                find = true;
            }

            if (GpmAssetManagementManager.Enable == true)
            {
                if (m_ProjectTreeGUI == null)
                {
                    m_ProjectTreeGUI = new ProjectPropertyTreeGUI();
                    m_ProjectTreeGUI.Init();

                    find = true;
                }
            }

            if (findTarget != null)
            {
                if (find == true)
                {
                    m_HierarchyTreeGUI.FindHierarchy(findTarget, containSubObject);
                    if (GpmAssetManagementManager.Enable == true)
                    {
                        m_ProjectTreeGUI.FindProject(findTarget, containSubObject);
                    }
                    find = false;
                }
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        Ui.Label(Strings.KEY_HIERARCHY_VIEW, EditorStyles.boldLabel);

                        GUILayout.FlexibleSpace();

                        enableReplace = Ui.Toggle(enableReplace, Strings.KEY_ENABLE_REPLACE);
                        m_HierarchyTreeGUI.SetEnableReplace(enableReplace);
                    }
                    using (new EditorGUILayout.VerticalScope())
                    {
                        m_HierarchyTreeGUI.OnGUI();
                    }
                }

                using (new EditorGUILayout.VerticalScope())
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        Ui.Label(Strings.KEY_PROJECT_VIEW, EditorStyles.boldLabel);

                        GUILayout.FlexibleSpace();

                        enableReplace_project = Ui.Toggle(enableReplace_project, Strings.KEY_ENABLE_REPLACE);
                        m_ProjectTreeGUI.SetEnableReplace(enableReplace_project);
                    }

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GpmAssetManagementManager.Enable == true)
                        {
                            using (new EditorGUILayout.VerticalScope())
                            {
                                m_ProjectTreeGUI.OnGUI();
                            }
                        }
                        else
                        {
                            using (new EditorGUILayout.VerticalScope(GUILayout.ExpandHeight(true)))
                            {
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
                            }

                        }
                    }

                    
                }
            }
        }
    }
}