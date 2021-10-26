using System.Collections.Generic;
using System.Linq;

using BlackPearl.Prism.Core.WPF.CodeGenerator.Extensions;
using BlackPearl.Prism.Core.WPF.CodeGenerator.Model;

using Microsoft.CodeAnalysis;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator.Inspectors
{
    internal static class ViewModelCommandAttributeInspector
    {
        internal static void InspectForCommandAttribute(this ViewModelToGenerate viewModelToGenerate)
        {
            IEnumerable<IMethodSymbol> viewModelMembers = viewModelToGenerate.Symbols.OfType<IMethodSymbol>();
            foreach (IMethodSymbol methodSymbol in viewModelMembers)
            {
                viewModelToGenerate.ProcessCommandAttribute(methodSymbol);
            }
        }

        private static void ProcessCommandAttribute(this ViewModelToGenerate viewModelToGenerate, IMethodSymbol methodSymbol)
        {
            if (methodSymbol.Parameters.Count() > 1)
            {
                return;
            }

            CommandToGenerate? cmd = GenerateCommandWithAction(methodSymbol);
            if (cmd == null)
            {
                return;
            }

            ProcessCommandWithPredicate(viewModelToGenerate, methodSymbol, cmd);
            ProcessCommandInvalidate(methodSymbol, cmd);

            viewModelToGenerate.CommandsToGenerate.Add(cmd);
        }

        private static void ProcessCommandInvalidate(IMethodSymbol methodSymbol, CommandToGenerate cmd)
        {
            IEnumerable<AttributeData>? invalidateAttributeDatas = methodSymbol.GetAttributeData<CommandInvalidateAttribute>();
            if (!invalidateAttributeDatas.Any())
            {
                return;
            }

            var tempList = new List<string>();

            foreach (AttributeData? attr in invalidateAttributeDatas)
            {
                IEnumerable<string>? propertyNames = attr.GetStringValueFromAttributeArgument();
                if (propertyNames?.Any() == true)
                {
                    tempList.AddRange(propertyNames);
                }
            }

            cmd.CanExecuteAffectingProperties = tempList.Distinct().ToArray();
        }

        internal static CommandToGenerate? GenerateCommandWithAction(IMethodSymbol methodSymbol)
        {
            AttributeData? commandAttributeData = methodSymbol.GetAttributeData<CommandAttribute>()?.FirstOrDefault();
            if (commandAttributeData == null)
            {
                return null;
            }

            string? commandPropertyName = commandAttributeData.GetNamedArgumentValue(nameof(CommandAttribute.PropertyName))
                                                        ?? $"{methodSymbol.Name}Command";

            var resultCommand = new CommandToGenerate(commandPropertyName, methodSymbol.GetMethodInfo());
            return resultCommand;
        }

        private static void ProcessCommandWithPredicate(ViewModelToGenerate viewModelToGenerate, IMethodSymbol methodSymbol, CommandToGenerate commandToProcess)
        {
            AttributeData? commandAttributeData = methodSymbol.GetAttributeData<CommandAttribute>()?.FirstOrDefault();
            if (commandAttributeData == null)
            {
                return;
            }

            string? canExecuteMethodName = commandAttributeData.GetNamedArgumentValue(nameof(CommandAttribute.CanExecuteMethod))
                                                        ?? commandAttributeData.ConstructorArguments.FirstOrDefault().Value?.ToString();

            if (canExecuteMethodName == null)
            {
                return;
            }

            MethodInfo? canExecuteMethodInfo = viewModelToGenerate.Symbols.OfType<IMethodSymbol>().FirstOrDefault(x => x.Name == canExecuteMethodName)?.GetMethodInfo();
            commandToProcess.CanExecuteMethod = canExecuteMethodInfo;
        }
    }
}