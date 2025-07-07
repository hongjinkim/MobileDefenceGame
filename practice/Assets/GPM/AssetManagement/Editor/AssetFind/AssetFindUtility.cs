using UnityEngine;

namespace Gpm.AssetManagement.AssetFind
{
    public static class AssetFindUtility
    {
        public static FindModule FindMissingProperty(Object target)
        {
            FindModule module = PropertyFinder.GetModule(target);
            module.SetMissingCondition();

            module.Find();
            
            return module;
        }
    }
}