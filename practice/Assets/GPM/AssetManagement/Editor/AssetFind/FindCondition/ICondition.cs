using UnityEngine;
using UnityEditor;

namespace Gpm.AssetManagement.AssetFind
{
    public interface ICondition
    {
        bool Check(SerializedProperty property);

        bool Check(Object checkObject);
    }
}
