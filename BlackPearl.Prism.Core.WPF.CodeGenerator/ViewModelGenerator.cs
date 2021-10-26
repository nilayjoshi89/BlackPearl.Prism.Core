
using System.Collections.Generic;
using System.Linq;

using BlackPearl.Prism.Core.WPF.CodeGenerator.Inspectors;
using BlackPearl.Prism.Core.WPF.CodeGenerator.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator
{
    [Generator]
    public class ViewModelGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is SyntaxReceiver generatorContext))
            {
                return;
            }

            context.AddSource("text.g.cs", "//Test Data");
        }

        public void Initialize(GeneratorInitializationContext context) => context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    /// <summary>
    /// Receives all the classes that have the MvvmGen.ViewModelAttribute set.
    /// </summary>
    internal class SyntaxReceiver : ISyntaxContextReceiver
    {
        public SyntaxReceiver()
        {
            ViewModelsToGenerate = new List<ViewModelToGenerate>();
        }

        public List<ViewModelToGenerate> ViewModelsToGenerate { get; }

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (!(context.Node is ClassDeclarationSyntax classDeclarationSyntax)
                || classDeclarationSyntax.AttributeLists.Count < 1)
            {
                return;
            }
            INamedTypeSymbol? viewModelClassSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);
            AttributeData? viewModelAttributeData = viewModelClassSymbol?.GetAttributes()
                                                    .SingleOrDefault(x => x.AttributeClass?.ToDisplayString() == typeof(ViewModelAttribute).FullName);

            if (viewModelClassSymbol == null || viewModelAttributeData == null)
            {
                return;
            }

            var viewModelToGenerate = new ViewModelToGenerate(viewModelClassSymbol);

            ViewModelMemberInspector.Inspect(viewModelToGenerate);

            SetCommandsToInvalidatePropertyOnPropertiesToGenerate(viewModelToGenerate.PropertiesToGenerate, viewModelToGenerate.CommandsToGenerate);

            ViewModelsToGenerate.Add(viewModelToGenerate);
        }

        private static void SetCommandsToInvalidatePropertyOnPropertiesToGenerate(
            IEnumerable<PropertyToGenerate> propertiesToGenerate,
            IEnumerable<CommandToGenerate> commandsToGenerate)
        {
            IEnumerable<CommandToGenerate>? commandsWithInvalidationProperties = commandsToGenerate.Where(x => x.CanExecuteAffectingProperties != null);

            foreach (PropertyToGenerate? propertyToGenerate in propertiesToGenerate)
            {
                propertyToGenerate.CommandsToInvalidate = commandsWithInvalidationProperties
                    .Where(x => x.CanExecuteAffectingProperties.Contains(propertyToGenerate.PropertyName))
                    .ToList();
            }
        }
    }
}
