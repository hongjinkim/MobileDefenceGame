#if UNITY_2021_2_OR_NEWER
using UnityEditor.SceneManagement;
#else
using UnityEditor.Experimental.SceneManagement;
#endif

namespace Gpm.AssetManagement.AssetFind
{
    public class PrefabStageRoot
    {
        public PrefabStageRoot(PrefabStage prefab)
        {
            this.prefab = prefab;
#if UNITY_2020_1_OR_NEWER
            this.path = prefab.assetPath;
#else
            this.path = prefab.prefabAssetPath;
#endif
            this.handle = prefab.scene.handle;

            PrefabStageRootManager.changePrefabRoot += ChangePrefabRoot;
        }

        public void ReOpen(PrefabStage prefab)
        {
            this.prefab = prefab;
            this.handle = prefab.scene.handle;
        }

        public void Remove()
        {
            removed = true;
        }

        public PrefabStage prefab;
        public string path;
        public int handle;
        private bool removed = false;

        private void ChangePrefabRoot(PrefabStageRoot addPrefabRoot, PrefabStageRoot removePrefabRoot)
        {
            if (removed == true)
            {
                if (addPrefabRoot != null)
                {
                    if (path.Equals(addPrefabRoot.path) == true)
                    {
                        ReOpen(addPrefabRoot.prefab);
                        removed = false;
                    }
                }
            }
        }
    }
}