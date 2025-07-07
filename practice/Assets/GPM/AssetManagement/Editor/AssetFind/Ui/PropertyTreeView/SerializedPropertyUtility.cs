using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Gpm.AssetManagement.AssetFind.Ui.PropertyTreeView
{
    public static class SerializedPropertyUtility
    {
        /// < summary >
        /// SerializedProperty에서 FieldInfo를 얻기
        /// </ summary >
        public static System.Type GetFieldType(this SerializedProperty property)
        {
            System.Type type = GetPropertyObjectType(property);
            if (type != null)
            {
                return type;
            }

            if (property.type == "PPtr<MonoScript>" ||
                property.type == "PPtr<$MonoScript>")
            {
                return typeof(UnityEditor.MonoScript);
            }

            if (property.type == "PPtr<Sprite>" ||
                property.type == "PPtr<$Sprite>")
            {
                return typeof(UnityEngine.Sprite);
            }

            if (property.type == "PPtr<Material>" ||
                property.type == "PPtr<$Material>")
            {
                return typeof(UnityEngine.Material);
            }

            FieldInfo fi = property.GetFieldInfo();
            if (fi != null)
            {
                return fi.FieldType;
            }

            return typeof(UnityEngine.Object);
        }

        public static string GetPropertyType(SerializedProperty property)
        {
            var type = property.type;

            var match = System.Text.RegularExpressions.Regex.Match(type, @"PPtr<(.*?)>");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            match = System.Text.RegularExpressions.Regex.Match(type, @"PPtr<\$(.*?)>");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return type;
        }

        public static Type GetPropertyObjectType(SerializedProperty property)
        {
            return System.Type.GetType(string.Format("UnityEngine.{0}, UnityEngine", GetPropertyType(property)));
        }

        public static FieldInfo GetFieldInfo(this SerializedProperty property)
        {
            FieldInfo GetField(Type type, string path)
            {
                return type.GetField(path, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            }

            var parentType = property.serializedObject.targetObject.GetType();
            var splits = property.propertyPath.Split('.');
            var fieldInfo = GetField(parentType, splits[0]);
            if (fieldInfo == null)
            {
                return null;
            }
            for (var i = 1; i < splits.Length; i++)
            {
                if (splits[i] == "Array")
                {
                    i += 2;
                    if (i >= splits.Length)
                    {
                        continue;
                    }

                    var type = fieldInfo.FieldType.IsArray
                        ? fieldInfo.FieldType.GetElementType()
                        : fieldInfo.FieldType.GetGenericArguments()[0];

                    fieldInfo = GetField(type, splits[i]);
                }
                else
                {
                    fieldInfo = i + 1 < splits.Length && splits[i + 1] == "Array"
                        ? GetField(parentType, splits[i])
                        : GetField(fieldInfo.FieldType, splits[i]);
                }

                if (fieldInfo == null)
                {
                    return null;
                }

                parentType = fieldInfo.FieldType;
            }

            return fieldInfo;
        }

        /// < summary >
        /// SerializedProperty에서 Field의 Type을 가져 오기 
        /// </ summary >
        /// < param name = "property" > SerializedProperty </ param > 
        /// < param name = "isArrayListType" > array 또는 List 경우 요소의 Type을 가져오기 </ param > 
        public static Type GetPropertyType(this SerializedProperty property, bool isArrayListType = false)
        {
            var fieldInfo = property.GetFieldInfo();
            /// <summary>
            /// 배열의 경우 배열의 Type을 반환
            /// </summary>
            if (isArrayListType == true && property.isArray && property.propertyType != SerializedPropertyType.String)
            {
                return fieldInfo.FieldType.IsArray
                    ? fieldInfo.FieldType.GetElementType()
                    : fieldInfo.FieldType.GetGenericArguments()[0];
            }
            return fieldInfo.FieldType;
        }
    }
}