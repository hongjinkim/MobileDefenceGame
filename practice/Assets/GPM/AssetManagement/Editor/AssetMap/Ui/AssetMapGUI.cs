using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Gpm.AssetManagement.AssetMap.Ui
{
    using Gpm.AssetManagement.Const;

    [System.Serializable]
    public class AssetMapGUI
    {
        internal EditorWindow window;

        internal Object rootObject;

        [System.NonSerialized]
        private AssetMapGraphGUI assetGraph;

        [System.NonSerialized]
        private AssetMapTreeGUI assetReferenceTree;


        [System.NonSerialized]
        private AssetMapTreeGUI assetDependencyTree;

        [System.NonSerialized]
        private bool bInit = false;

        [System.NonSerialized]
        private int updateCount = 0;

        public AssetMapGUI(EditorWindow window)
        {
            this.window = window;
            Init();
        }

        public void Init()
        {
            if (assetGraph == null)
            {
                assetGraph = new AssetMapGraphGUI(this);
            }

            if (assetReferenceTree == null)
            {
                assetReferenceTree = new AssetMapTreeGUI();
                assetReferenceTree.Init(this, true);
            }

            if (assetDependencyTree == null)
            {
                assetDependencyTree = new AssetMapTreeGUI();
                assetDependencyTree.Init(this, false);
            }

            updateCount = 0;

            bInit = true;
        }

        public void SetRootObject(Object rootObject)
        {
            if (GpmAssetManagementManager.Enable &&
               rootObject != null)
            {
                string guid;
                long localId;
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(rootObject, out guid, out localId) == true)
                {
                    this.rootObject = rootObject;
                    assetGraph.SetRootObject(rootObject);

                    assetReferenceTree.SetRootObject(rootObject);
                    assetDependencyTree.SetRootObject(rootObject);
                }
                else
                {
                    this.rootObject = null;
                }
            }
            else
            {
                this.rootObject = null;
            }
        }

        public void Reload()
        {
            assetGraph.Clear();

            assetReferenceTree.Reload();
            assetDependencyTree.Reload();
        }

        public void OnClick_AssetData(string guid)
        {
            FocusGraph(guid);
        }

        public void FocusGraph(string guid)
        {
            var node = assetGraph.GetNode(guid);
            if(node != null)
            {
                node.Ping();
                assetGraph.FocusPostion(node.ZoomRect.center);
                
            }
        }

        public void OnGUI()
        {
            if (bInit == false)
            {
                Init();
                if(rootObject != null)
                {
                    SetRootObject(rootObject);
                }
            }
            
            using (new EditorGUILayout.VerticalScope())
            {                
                Object changeObject = EditorGUILayout.ObjectField(Ui.GetString(Strings.KEY_ROOT_ASSET), rootObject, typeof(Object), false);
                if (rootObject != changeObject)
                {
                    SetRootObject(changeObject);
                    Reload();
                }

                if (rootObject == null)
                {
                    Ui.Label(Strings.KEY_NEED_ROOT_OBJECT);

                    return;
                }   

                if (updateCount != Internal.AssetMapUpdater.updateCount)
                {
                    Reload();
                    updateCount = Internal.AssetMapUpdater.updateCount;
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    OnGUITree(300);

                    Rect GraphRect = EditorGUILayout.GetControlRect(GUILayout.ExpandHeight(true));
                    assetGraph.OnGUI(GraphRect);
                }
            }
        }

        private void OnGUITree(float width)
        {
            using (new EditorGUILayout.VerticalScope(GUILayout.Width(width)))
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    Ui.Label(Strings.KEY_REFERENCE);

                    Rect controllRect = EditorGUILayout.GetControlRect(false, GUILayout.ExpandHeight(true));

                    if (assetReferenceTree != null)
                    {
                        assetReferenceTree.OnGUI(controllRect);
                    }
                }

                GUILayout.Space(20);

                using (new EditorGUILayout.VerticalScope())
                {
                    Ui.Label(Strings.KEY_DEPENDENCY);

                    Rect controllRect = EditorGUILayout.GetControlRect(false, GUILayout.ExpandHeight(true));

                    if (assetDependencyTree != null)
                    {
                        assetDependencyTree.OnGUI(controllRect);
                    }
                }
            }
        }
    }
}