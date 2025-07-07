using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Gpm.AssetManagement.AssetFind
{
    public class ObjectCondition : ICondition
    {
        public Object targetObject;

        public bool containSubObject = true;
             
        public bool Check(SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference &&
                property.propertyPath.Equals("m_GameObject") == false)
            {
                Object objectReferenceValue = property.objectReferenceValue;

                if (objectReferenceValue != null)
                {
                    if (containSubObject == true)
                    {
                        /// <summary>
                        /// 서브 오브젝트 체크
                        /// </summary>
                        if (AssetDatabase.IsSubAsset(objectReferenceValue) == true)
                        {
                            string mainAssetPath = AssetDatabase.GetAssetPath(objectReferenceValue);
                            Object mainAsset = AssetDatabase.LoadMainAssetAtPath(mainAssetPath);
                            return Check(mainAsset);
                        }
                    }

                    return Check(objectReferenceValue);
                }
            }

            return false;
        }

        public bool Check(Object checkObject)
        {
            /// <summary>
            /// 프리팹이고 연결되어있는 경우 체크
            /// </summary>
            if (targetObject is GameObject &&
                checkObject is GameObject &&
                PrefabUtility.GetPrefabInstanceStatus(checkObject) == PrefabInstanceStatus.Connected)
            {
                if (PrefabUtility.GetCorrespondingObjectFromSource(checkObject) == targetObject)
                {
                    return true;
                }
            }
            else
            {
                if (targetObject == checkObject)
                {
                    return true;
                }
            }

            return false;
        }
    }
}