using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gpm.AssetManagement.AssetFind
{
    public class PropertyFinder
    {
        public event FinderModuleUpdateCallback moduleAdded;
        public event FinderModuleUpdateCallback moduleRemoved;


        public List<FindModule> moduleList = new List<FindModule>();
        protected List<ICondition> propertyCondition = new List<ICondition>();


        public void AddModule(FindModule addModule)
        {
            moduleList.Add(addModule);

            moduleAdded?.Invoke(addModule);
        }

        public void RemoveModule(FindModule removeModule)
        {
            moduleList.Remove(removeModule);

            moduleRemoved?.Invoke(removeModule);
        }

        public void SetCondition(Object targetObject, bool containSubObject = true)
        {
            propertyCondition.Clear();
            if (targetObject != null)
            {
                ObjectCondition condtion = new ObjectCondition();
                condtion.targetObject = targetObject;
                condtion.containSubObject = containSubObject;
                propertyCondition.Add(condtion);

                /// <summary>
                // 서브인경우 메인 추가
                /// </summary>
                if (AssetDatabase.IsSubAsset(targetObject) == true)
                {
                    string mainAssetPath = AssetDatabase.GetAssetPath(targetObject);
                    Object mainAsset = AssetDatabase.LoadMainAssetAtPath(mainAssetPath);

                    condtion = new ObjectCondition();
                    condtion.targetObject = mainAsset;
                    condtion.containSubObject = containSubObject;
                    propertyCondition.Add(condtion);
                }
            }
        }

        public void SetMissingCondition()
        {
            propertyCondition.Clear();

            propertyCondition.Add(new MissingCondition());
        }


        static public FindModule GetModule(Object obj)
        {
            if (obj is SceneAsset sceneAsset)
            {
                return new ProjectSceneFindModule(sceneAsset);
            }
            else
            {
                return new ObjectFinderModule(obj);
            }
        }

        static public FindModule GetModule(PrefabStageRoot prefabStageRoot)
        {
            return new HierarchyPrefabFindModule(prefabStageRoot);
        }

        static public FindModule GetModule(SceneRoot sceneRoot)
        {
            return new HierarchySceneFindModule(sceneRoot);
        }

        public delegate void FinderModuleUpdateCallback(FindModule module);
    }
}