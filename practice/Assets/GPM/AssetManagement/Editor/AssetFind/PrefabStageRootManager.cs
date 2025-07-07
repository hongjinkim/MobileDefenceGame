using UnityEditor;
using UnityEngine;

#if UNITY_2021_2_OR_NEWER
using UnityEditor.SceneManagement;
#else
using UnityEditor.Experimental.SceneManagement;
#endif

namespace Gpm.AssetManagement.AssetFind
{
    static public class PrefabStageRootManager
    {
        private static PrefabStageRoot activePrefabStage = null;

        public delegate void PrefabRootChangeCallback(PrefabStageRoot addPrefabRoot, PrefabStageRoot removePrefabRoot);
        public static event PrefabRootChangeCallback changePrefabRoot;

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                activePrefabStage = new PrefabStageRoot(prefabStage);
            }

            PrefabStage.prefabStageOpened += prefabStageOpened;
            PrefabStage.prefabStageClosing += prefabStageClosing;
            PrefabStage.prefabSaved += prefabSaved;
        }

        public static PrefabStageRoot GetCurrentPrefabStageRoot()
        {
            return activePrefabStage;
        }

        private static void prefabStageOpened(PrefabStage prefabStage)
        {
            if (activePrefabStage != null)
            {
                if (changePrefabRoot != null)
                {
                    changePrefabRoot(null, activePrefabStage);
                }
                activePrefabStage = null;
            }

            activePrefabStage = new PrefabStageRoot(prefabStage);

            if (changePrefabRoot != null)
            {
                changePrefabRoot(activePrefabStage, null);
            }
        }

        private static void prefabStageClosing(PrefabStage prefabStage)
        {
            if (changePrefabRoot != null)
            {
                changePrefabRoot(null, activePrefabStage);
            }

            activePrefabStage = null;
        }

        private static void prefabSaved(GameObject prefab)
        {
        }
    }
}