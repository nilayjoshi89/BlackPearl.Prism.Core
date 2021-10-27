
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BlackPearl.Prism.Core.WPF.CodeGenerator.Extensions;
using BlackPearl.Prism.Core.WPF.CodeGenerator.Inspectors;
using BlackPearl.Prism.Core.WPF.CodeGenerator.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator
{
    [Generator]
    public class ViewModelGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxContextReceiver is SyntaxReceiver generatorContext))
            {
                return;
            }

            INamedTypeSymbol? viewModelBaseSymbol = context.Compilation.GetTypeByMetadataName(typeof(BlackPearlViewModelBase).FullName);
            if (viewModelBaseSymbol == null)
            {
                return;
            }

            foreach (ViewModelToGenerate? viewModelToGenerate in generatorContext.ViewModelsToGenerate)
            {
                var vmBuilder = new ViewModelDocumentBuilder();

                vmBuilder.GenerateCommentHeader();

                vmBuilder.GenerateUsingDirectives();

                vmBuilder.GenerateNamespace(viewModelToGenerate.Namespace);

                vmBuilder.GenerateClass(viewModelToGenerate, viewModelBaseSymbol);

                vmBuilder.GenerateCommandInitializeMethod(viewModelToGenerate.CommandsToGenerate);

                vmBuilder.GenerateCommandProperties(viewModelToGenerate.CommandsToGenerate);

                vmBuilder.GenerateProperties(viewModelToGenerate.PropertiesToGenerate, viewModelToGenerate.ComplexPropertiesToGenerate);

                vmBuilder.GeneratePropertyChangeHandlers(viewModelToGenerate.PropertiesToGenerate, viewModelToGenerate.ComplexPropertiesToGenerate);
                
                while (vmBuilder.DecreaseIndent())
                {
                    vmBuilder.AppendLine("}");
                }

                var sourceText = SourceText.From(vmBuilder.ToString(), Encoding.UTF8);
                context.AddSource($"{viewModelToGenerate.Namespace}.{viewModelToGenerate.Name}.g.cs", sourceText);
            }
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
            AttributeData? viewModelAttributeData = viewModelClassSymbol?.GetAttributeData<ViewModelAttribute>().FirstOrDefault();

            if (viewModelClassSymbol == null || viewModelAttributeData == null)
            {
                return;
            }

            var viewModelToGenerate = new ViewModelToGenerate(viewModelClassSymbol);
            ViewModelInspector.Inspect(viewModelToGenerate);

            ViewModelsToGenerate.Add(viewModelToGenerate);
        }
    }
}
