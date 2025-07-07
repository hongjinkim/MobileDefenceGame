using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Gpm.AssetManagement.AssetFind
{
    public class MissingCondition : ICondition
    {
        public bool Check(SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference &&
                property.objectReferenceValue == null)
            {
                if (property.objectReferenceInstanceIDValue != 0)
                {
                    return true;
                }

                if (property.hasChildren)
                {
                    var fileId = property.FindPropertyRelative("m_FileID");
                    if (fileId != null)
                    {
                        if (fileId.intValue != 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool Check(Object checkObject)
        {
            return false;
        }
    }
}