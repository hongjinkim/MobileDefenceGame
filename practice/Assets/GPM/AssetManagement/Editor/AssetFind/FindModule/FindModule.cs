using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gpm.AssetManagement.AssetFind
{
    public abstract class FindModule
    {
        protected bool isPersistent = false;
        protected string guid;

        private List<ICondition> propertyCondition = new List<ICondition>();

        private List<Object> findObj = new List<Object>();

        public List<PropertyPath> result = new List<PropertyPath>();

        public abstract string GetName();

        public abstract int GetID();

        public abstract Texture2D GetTypeIcon();
        public abstract bool IsValid();

        public virtual bool Reload() { return false; }

        public abstract void Find(bool bForce = false);

        public abstract bool CheckFindAble();


        public bool IsFind()
        {
            return findObj.Count > 0;
        }

        public void Clear()
        {
            findObj.Clear();
            result.Clear();
        }

        public void SetCondition(List<ICondition> propertyCondition)
        {
            this.propertyCondition = propertyCondition;
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
                /// 서브인경우 메인 추가
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

        private bool HasCondition(SerializedProperty property)
        {
            for (int i = 0; i < propertyCondition.Count; i++)
            {
                if (propertyCondition[i].Check(property) == true)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasConditionObj(Object checkObject)
        {
            /// <summary>
            /// 서브 오브젝트 체크
            /// </summary>
            if (AssetDatabase.IsSubAsset(checkObject) == true)
            {
                string mainAssetPath = AssetDatabase.GetAssetPath(checkObject);
                Object mainAsset = AssetDatabase.LoadMainAssetAtPath(mainAssetPath);
                return HasConditionObj(mainAsset);
            }

            for (int i = 0; i < propertyCondition.Count; i++)
            {
                if (propertyCondition[i].Check(checkObject) == true)
                {
                    return true;
                }
            }

            return false;
        }

        protected void Find(Object checkObject)
        {
            FindProperty(checkObject);

        }

        protected void Find(Object[] checkObjectList)
        {
            for (int i = 0; i < checkObjectList.Length; i++)
            {
                FindProperty(checkObjectList[i]);
            }
        }

        protected virtual bool IsChildObject(Object objectReferenceValue)
        {
            if (isPersistent == true)
            {
                string guid;
                long localid;
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(objectReferenceValue, out guid, out localid) == true)
                {
                    if (guid.Equals(this.guid) == true)
                    {
                        return true;
                    }
                }
            }

            if (EditorUtility.IsPersistent(objectReferenceValue) == false)
            {
                return true;
            }

            return false;
        }

        public void FindProperty(Object checkObject)
        {
            if (checkObject == null)
            {
                return;
            }

            if (findObj.Contains(checkObject) == true)
            {
                return;
            }

            findObj.Add(checkObject);

            /// <summary>
            ///  프래팹 내부까지 찾는 로직 경우(하이라키, 프래팹 인스턴스 자체)
            /// </summary>
            if (checkObject is GameObject go)
            {
                /// <summary>
                ///  Prefab 인경우 prefabSearch 로 전부 찾는다.
                ///  Hierachy에 있는 Scene 인경우 prefabSearch 로 전부 찾는게 좋아보인다
                ///  Project에서 Scene을 검색하는 경우 prefabSearch를 하지 말자.(이것만 depenecny가 연결되있지 않다)
                /// </summary>
                bool prefabSearch = true;

                /// <summary>
                /// 대상이 프리팹인경우(본인은 체크하지 않는다)
                /// </summary>
                if (PrefabUtility.GetPrefabInstanceStatus(checkObject) == PrefabInstanceStatus.Connected)
                {
                    if (HasConditionObj(checkObject) == true)
                    {
                        PropertyPath addPath = new PropertyPath();
                        addPath.rootObject = checkObject;
                        addPath.path = "";

                        result.Add(addPath);
                    }

                    /// <summary>
                    /// 프리팹 내부까지 찾지 않는 경우 바뀐것만 찾아준다.
                    /// </summary>
                    if (prefabSearch == false)
                    {
                        var overrrides = PrefabUtility.GetObjectOverrides(go);
                        foreach (var overrride in overrrides)
                        {
                            var modifications = PrefabUtility.GetPropertyModifications(overrride.instanceObject);
                            foreach (var modification in modifications)
                            {
                                if (modification.objectReference != null)
                                {
                                    if (HasConditionObj(modification.objectReference) == true)
                                    {
                                        if (modification.target == overrride.GetAssetObject())
                                        {
                                            PropertyPath info = new PropertyPath();
                                            info.rootObject = overrride.instanceObject;
                                            info.path = modification.propertyPath;

                                            result.Add(info);
                                        }
                                    }
                                }
                            }
                        }

                        var addObjects = PrefabUtility.GetAddedGameObjects(checkObject as GameObject);
                        foreach (var addObject in addObjects)
                        {
                            Find(addObject.instanceGameObject);
                        }

                        return;
                    }
                }
            }

            if (checkObject is Transform)
            {
                Transform transform = checkObject as Transform;
                FindProperty(transform.gameObject);

                foreach (Transform child in transform)
                {
                    FindProperty(child);
                }
            }
            else
            {
                SerializedObject serialized_object = new SerializedObject(checkObject);
                SerializedProperty property;
                if (checkObject is GameObject)
                {
                    property = serialized_object.FindProperty("m_Component");
                }
                else
                {
                    property = serialized_object.GetIterator();
                }

                while (property.Next(true) == true)
                {
                    if (HasCondition(property) == true)
                    {
                        PropertyPath addPath = new PropertyPath();
                        addPath.rootObject = checkObject;
                        addPath.path = property.propertyPath;
                        result.Add(addPath);
                    }

                    if (property.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        Object objectReferenceValue = property.objectReferenceValue;
                        if (objectReferenceValue != null)
                        {
                            if (!findObj.Contains(objectReferenceValue) == true)
                            {
                                if (property.propertyPath.Equals("m_PrefabInstance") == false &&
                                    IsChildObject(objectReferenceValue) == true)
                                {
                                    FindProperty(objectReferenceValue);
                                }
                            }

                        }
                    }
                }
            }
        }
    }
}