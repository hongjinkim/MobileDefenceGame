#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEditor;
using System.Collections.Generic;

public static class OdinStubGenerator
{
    [MenuItem("Tools/Generate Odin Attribute Stubs")]
    public static void Generate()
    {
        const string odinNamespace = "Sirenix.OdinInspector";
        var sb = new StringBuilder();
        var enumSet = new HashSet<Type>();
        var processedTypes = new HashSet<string>();

        sb.AppendLine("#if !ODIN_INSPECTOR");
        sb.AppendLine("using System;");
        sb.AppendLine();
        sb.AppendLine("namespace Sirenix.OdinInspector");
        sb.AppendLine("{");

        // Odin Inspector 어셈블리 찾기
        var odinAsm = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name.StartsWith("Sirenix.OdinInspector"));

        if (odinAsm == null)
        {
            EditorUtility.DisplayDialog("Odin Not Found", "Odin Inspector Assembly를 찾을 수 없습니다.", "OK");
            return;
        }

        // 모든 Attribute 타입 + 생성자 파라미터 타입에서 enum, class, struct 추출
        var attrTypes = odinAsm.GetTypes()
            .Where(t => t.IsClass && t.IsPublic && t.Namespace == odinNamespace && typeof(Attribute).IsAssignableFrom(t))
            .OrderBy(t => t.Name);

        foreach (var attr in attrTypes)
        {
            if (!processedTypes.Add(attr.FullName))
                continue;

            // Attribute 정의
            sb.AppendLine($"    [AttributeUsage(AttributeTargets.All)]");
            sb.AppendLine($"    public class {attr.Name} : Attribute");
            sb.AppendLine("    {");

            // 생성자(오버로드) 정의
            var ctors = attr.GetConstructors();
            bool hasParameterless = false;
            foreach (var ctor in ctors)
            {
                var parameters = ctor.GetParameters();
                if (parameters.Length == 0) hasParameterless = true;
                var paramList = string.Join(", ", parameters
                    .Select(p => $"{GetTypeKeyword(p.ParameterType)} {p.Name}"));
                sb.AppendLine($"        public {attr.Name}({paramList}) {{ }}");

                // 파라미터 타입에 Enum/class면 기록
                foreach (var p in parameters)
                {
                    var type = p.ParameterType;
                    if (type.Namespace == odinNamespace)
                    {
                        if (type.IsEnum || (type.IsClass && type != attr && type.IsPublic) || (type.IsValueType && !type.IsPrimitive && !type.IsEnum))
                        {
                            enumSet.Add(type);
                        }
                    }
                }
            }
            if (!hasParameterless)
                sb.AppendLine($"        public {attr.Name}() {{ }}");
            sb.AppendLine("    }");
        }

        // Enum/class stub
        foreach (var type in enumSet)
        {
            if (!processedTypes.Add(type.FullName))
                continue;

            if (type.IsEnum)
            {
                sb.AppendLine($"    public enum {type.Name}");
                sb.AppendLine("    {");
                var values = Enum.GetNames(type);
                foreach (var v in values)
                {
                    sb.AppendLine($"        {v},");
                }
                sb.AppendLine("    }");
            }
            else if (type.IsClass)
            {
                sb.AppendLine($"    public class {type.Name} {{ }}");
            }
            else if (type.IsValueType && !type.IsPrimitive)
            {
                sb.AppendLine($"    public struct {type.Name} {{ }}");
            }
        }

        sb.AppendLine("}");
        sb.AppendLine("#endif");

        // 저장 경로
        var outputPath = "Assets/ThirdPartyStub/Sirenix/OdinInspector/OdinInspectorStubs.cs";
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
        File.WriteAllText(outputPath, sb.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("완료!", $"Odin Inspector Stub 생성 완료!\n{outputPath}", "OK");
    }

    // 기본형 이름 맞춰줌
    static string GetTypeKeyword(Type t)
    {
        if (t == typeof(int)) return "int";
        if (t == typeof(float)) return "float";
        if (t == typeof(bool)) return "bool";
        if (t == typeof(string)) return "string";
        if (t.IsEnum) return t.Name;
        if (t.IsByRef) return GetTypeKeyword(t.GetElementType());
        if (t.IsArray) return GetTypeKeyword(t.GetElementType()) + "[]";
        return t.Name;
    }
}
#endif
