using System.Collections.Generic;

namespace Gpm.AssetManagement.AssetFind
{
    public class HierarchySceneFinder : PropertyFinder
    {
        public HierarchySceneFinder()
        {
            SceneRootManager.changeSceneRoot += changeSceneRoot;
        }

        ~HierarchySceneFinder()
        {
            SceneRootManager.changeSceneRoot += changeSceneRoot;
        }

        public void Find()
        {
            List<SceneRoot> activeSceneList = SceneRootManager.GetSceneRootList();
            for (int i = 0; i < activeSceneList.Count; i++)
            {
                AddSceneModule(activeSceneList[i]);
            }
        }

        private void changeSceneRoot(SceneRoot addSceneRoot, SceneRoot removeSceneRoot)
        {
            if (removeSceneRoot != null)
            {
                RemoveSceneModule(removeSceneRoot);
            }

            if (addSceneRoot != null)
            {
                AddSceneModule(addSceneRoot);
            }
        }

        public void AddSceneModule(SceneRoot addSceneRoot)
        {
            FindModule module = new HierarchySceneFindModule(addSceneRoot);
            module.SetCondition(propertyCondition);
            module.Find();

            AddModule(module);

        }

        public void RemoveSceneModule(SceneRoot removeSceneRoot)
        {
            foreach (var module in moduleList)
            {
                if (module is HierarchySceneFindModule sceneFindModule)
                {
                    if (sceneFindModule.sceneRoot == removeSceneRoot)
                    {
                        RemoveModule(module);

                        break;
                    }
                }
            }
        }
    }
}
