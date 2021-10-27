using System.Linq;

using BlackPearl.Prism.Core.WPF.CodeGenerator.Extensions;
using BlackPearl.Prism.Core.WPF.CodeGenerator.Model;

using Microsoft.CodeAnalysis;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator
{
    internal static class ClassGenerator
    {
        internal static void GenerateClass(this ViewModelDocumentBuilder vmBuilder, ViewModelToGenerate viewModelToGenerate, INamedTypeSymbol viewModelBaseSymbol)
        {
            bool inheritFromViewModelBaseClass = !InheritsFromViewModelBase(viewModelToGenerate.BaseType, viewModelBaseSymbol);

            vmBuilder.AppendLine($"partial class {viewModelToGenerate.Name}" + (inheritFromViewModelBaseClass ? " : BlackPearlViewModelBase" : ""));
            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();
        }

        private static bool InheritsFromViewModelBase(INamedTypeSymbol? currentBaseType, INamedTypeSymbol viewModelBaseSymbol)
        {
            bool inherits = false;

            while (currentBaseType != null)
            {
                if (currentBaseType.Equals(viewModelBaseSymbol, SymbolEqualityComparer.Default)
                    || currentBaseType.GetAttributeData<ViewModelAttribute>().Any())
                {
                    inherits = true;
                    break;
                }
                currentBaseType = currentBaseType.BaseType;
            }

            return inherits;
        }
    }
}
