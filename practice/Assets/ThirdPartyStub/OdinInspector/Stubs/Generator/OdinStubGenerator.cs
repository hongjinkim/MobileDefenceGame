#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEditor;

public static class OdinStubGenerator
{
    [MenuItem("Tools/Generate Odin Attribute Stubs")]
    public static void Generate()
    {
        // Odin Inspector Assembly 네임스페이스
        const string odinNamespace = "Sirenix.OdinInspector";
        var sb = new StringBuilder();
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

        // Attribute 타입만 뽑기
        var attrs = odinAsm.GetTypes()
            .Where(t => t.IsClass && t.IsPublic && t.Namespace == odinNamespace && typeof(Attribute).IsAssignableFrom(t))
            .OrderBy(t => t.Name);

        foreach (var attr in attrs)
        {
            sb.AppendLine($"    [AttributeUsage(AttributeTargets.All)]");
            sb.AppendLine($"    public class {attr.Name} : Attribute");
            sb.AppendLine("    {");
            // 오버로드된 생성자 지원
            foreach (var ctor in attr.GetConstructors())
            {
                var paramList = string.Join(", ", ctor.GetParameters()
                    .Select(p => $"{p.ParameterType.Name} {p.Name}"));
                sb.AppendLine($"        public {attr.Name}({paramList}) {{ }}");
            }
            // 기본 생성자가 없는 경우라도 빈 생성자 하나 추가
            if (!attr.GetConstructors().Any(c => c.GetParameters().Length == 0))
                sb.AppendLine($"        public {attr.Name}() {{ }}");

            sb.AppendLine("    }");
        }

        sb.AppendLine("}");
        sb.AppendLine("#endif");

        // 저장 경로
        var outputPath = "Assets/ThirdPartyStub/OdinInspector/Stubs/Scripts/OdinInspectorStubs.cs";
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
        File.WriteAllText(outputPath, sb.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("완료!", $"Odin Inspector Stub 생성 완료!\n{outputPath}", "OK");
    }
}
#endif
