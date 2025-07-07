namespace Gpm.AssetManagement.AssetFind
{
    public class HierarchyPrefabFinder : PropertyFinder
    {
        public HierarchyPrefabFinder()
        {
            PrefabStageRootManager.changePrefabRoot += changePrefabRoot;
        }

        ~HierarchyPrefabFinder()
        {
            PrefabStageRootManager.changePrefabRoot -= changePrefabRoot;

        }
        public void Find()
        {
            PrefabStageRoot prefabStageRoot = PrefabStageRootManager.GetCurrentPrefabStageRoot();
            if (prefabStageRoot != null)
            {
                AddPrefabStageModule(prefabStageRoot);
            }
        }

        private void changePrefabRoot(PrefabStageRoot addPrefabRoot, PrefabStageRoot removePrefabRoot)
        {
            if (removePrefabRoot != null)
            {
                RemovePrefabStageModule(removePrefabRoot);
            }

            if (addPrefabRoot != null)
            {
                AddPrefabStageModule(addPrefabRoot);
            }
        }

        public void AddPrefabStageModule(PrefabStageRoot addPrefabRoot)
        {
            FindModule module = new HierarchyPrefabFindModule(addPrefabRoot);
            module.SetCondition(propertyCondition);
            module.Find();

            AddModule(module);
        }

        public void RemovePrefabStageModule(PrefabStageRoot removePrefabRoot)
        {
            foreach (var module in moduleList)
            {
                if (module is HierarchyPrefabFindModule prefabStageFindModule)
                {
                    if (prefabStageFindModule.prefabRoot == removePrefabRoot)
                    {
                        RemoveModule(module);

                        break;

                    }

                }
            }
        }
    }
}
