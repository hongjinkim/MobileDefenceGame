using UnityEditor;
using UnityEngine;

namespace Gpm.AssetManagement.AssetFind
{
    using Gpm.AssetManagement.Const;

    public class HierarchySceneFindModule : FindModule
    {
        public int handle;
        public SceneRoot sceneRoot;

        public HierarchySceneFindModule(SceneRoot sceneRoot)
        {
            this.sceneRoot = sceneRoot;
            handle = sceneRoot.handle;

            guid = AssetDatabase.AssetPathToGUID(sceneRoot.path);
            if (string.IsNullOrEmpty(guid) == false)
            {
                isPersistent = true;
            }
            else
            {
                isPersistent = false;
            }
        }

        public bool IsChangeRoot()
        {
            return sceneRoot.handle != handle;
        }

        public void ChangedRoot()
        {
            handle = sceneRoot.handle;
        }

        public override bool IsValid()
        {
            if (sceneRoot.scene.isLoaded == false)
            {
                return false;
            }
            return sceneRoot.handle == handle;
        }

        public override string GetName()
        {
            string name = sceneRoot.scene.name;
            if (string.IsNullOrEmpty(name) == true)
            {
                name = Strings.NAME_UNTITLED;
            }

            return name;
        }

        public override int GetID()
        {
            return sceneRoot.handle;
        }

        public override Texture2D GetTypeIcon()
        {
            return EditorGUIUtility.FindTexture(Constants.ICON_UNITYLOGO) as Texture2D;
        }

        public override bool CheckFindAble()
        {
            if (IsValid() == true &&
                sceneRoot.scene.isLoaded == true)
            {
                return true;
            }

            return false;
        }

        public override void Find(bool bForce = false)
        {
            if(CheckFindAble() == true)
            {
                Find(sceneRoot.scene.GetRootGameObjects());
            }
            
        }

    }
}