using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gpm.AssetManagement.AssetFind
{
    public class HierarchyPrefabFindModule : FindModule
    {
        public int handle;
        public PrefabStageRoot prefabRoot;

        public HierarchyPrefabFindModule(PrefabStageRoot prefabRoot)
        {
            this.prefabRoot = prefabRoot;
            handle = prefabRoot.handle;

            guid = AssetDatabase.AssetPathToGUID(prefabRoot.path);
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
            return prefabRoot.handle != handle;
        }

        public void ChangedRoot()
        {
            handle = prefabRoot.handle;
        }

        public override bool IsValid()
        {
            return prefabRoot.handle == handle;
        }

        public override string GetName()
        {
            string name = prefabRoot.prefab.scene.name;
            if (string.IsNullOrEmpty(name) == true)
            {
                name = Strings.NAME_UNTITLED;
            }

            return name;
        }

        public override int GetID()
        {
            return prefabRoot.handle;
        }

        public override Texture2D GetTypeIcon()
        {
            return AssetPreview.GetMiniThumbnail(prefabRoot.prefab.prefabContentsRoot);
        }

        public override void Find(bool bForce = false)
        {
            FindProperty(prefabRoot.prefab.prefabContentsRoot);
        }

        public override bool CheckFindAble()
        {
            return true;
        }

    }
}