using Microsoft.CodeAnalysis;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator
{
    internal static class NamespaceGenerator
    {
        internal static void GenerateNamespace(this ViewModelDocumentBuilder vmBuilder, string? classNamespace)
        {
            vmBuilder.AppendLine();

            if (classNamespace is null)
            {
                return;
                // TODO: Show an error here. ViewModel class must be top-level within a namespace
            }

            vmBuilder.AppendLine($"namespace {classNamespace}");
            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();
        }
    }
}
