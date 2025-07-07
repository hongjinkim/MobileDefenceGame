using UnityEngine;
using UnityEditor;

namespace Gpm.AssetManagement.AssetFind
{
    using Gpm.AssetManagement.AssetMap;

    public class ProjectObjectFinder : PropertyFinder
    {
        public void Find(Object findTarget)
        {

            string findPath = AssetDatabase.GetAssetPath(findTarget);

            var node = GpmAssetManagementManager.GetAssetDataFromPath(findPath);
            if (node != null)
            {
                foreach (AssetMapLink link in node.referenceLinks)
                {
                    Object obj = AssetDatabase.LoadAssetAtPath<Object>(link.GetPath());
                    if (obj == null)
                    {
                        continue;
                    }

                    if (PropertyFinder.GetModule(obj) is ObjectFinderModule module)
                    {
                        module.SetCondition(propertyCondition);

                        module.Find();

                        AddModule(module);
                    }
                }
            }
        }
    }
}
