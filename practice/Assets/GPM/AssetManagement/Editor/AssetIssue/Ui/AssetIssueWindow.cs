using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Gpm.AssetManagement.AssetIssue.Ui
{
    using Gpm.AssetManagement.Const;

    public class AssetIssueWindow : EditorWindow
    {
        private Object checkObject;

        private AssetIssueTreeGUI issueGUI;

        [System.NonSerialized]
        private bool bInit = false;

        static public void Open(Object checkObject)
        {
            AssetManagementLanguage.Load(
                   () =>
                   {
                       AssetIssueWindow w = EditorWindow.GetWindow<AssetIssueWindow>(false, Ui.GetString(Strings.KEY_ISSUEWINDOW_TITLE), true);

                       w.Init(checkObject);
                       w.Show();
                   });
        }

        public void Init(Object checkObject)
        {
            if (issueGUI == null)
            {
                issueGUI = new AssetIssueTreeGUI();
                issueGUI.Init();
            }

            this.checkObject = checkObject;
            issueGUI.CheckIssue(checkObject);

            bInit = true;
        }

        private void OnGUI()
        {
            if(bInit == false)
            {
                Init(checkObject);
            }
                
            issueGUI.OnGUI();
        }
    }
}