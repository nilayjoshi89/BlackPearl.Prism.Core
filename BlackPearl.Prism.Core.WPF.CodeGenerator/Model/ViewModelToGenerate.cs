using System.Collections.Generic;
using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator.Model
{
    /// <summary>
    /// Contains all the details that must be generated for a class that is decorated with the MvvmGen.ViewModelAttribute.
    /// </summary>
    internal class ViewModelToGenerate
    {
        private readonly INamedTypeSymbol viewModelClassSymbol;
        public ViewModelToGenerate(INamedTypeSymbol viewModelClassSymbol)
        {
            this.viewModelClassSymbol = viewModelClassSymbol;
            CommandsToGenerate = new List<CommandToGenerate>();
            PropertiesToGenerate = new List<PropertyToGenerate>();
        }

        public ImmutableArray<ISymbol> Symbols => viewModelClassSymbol.GetMembers();

        public IList<CommandToGenerate> CommandsToGenerate { get; }

        public IList<PropertyToGenerate> PropertiesToGenerate { get; }
    }
}
