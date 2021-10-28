using System.Collections.Generic;
using System.Linq;

using BlackPearl.Prism.Core.WPF.CodeGenerator.Model;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator
{
    internal static class PropertyGenerator
    {
        internal static void GenerateProperties(this ViewModelDocumentBuilder vmBuilder, IEnumerable<PropertyToGenerate>? propertiesToGenerate, IEnumerable<ComplexPropertyToGenerate>? complexPropertiesToGenerate)
        {
            if (propertiesToGenerate?.Any() != true && complexPropertiesToGenerate?.Any() != true)
            {
                return;
            }

            vmBuilder.AppendLine();
            vmBuilder.AppendLine("#region Properties");
            vmBuilder.GenerateProperties(propertiesToGenerate);
            vmBuilder.GenerateComplexProperties(complexPropertiesToGenerate);
            vmBuilder.AppendLine("#endregion Properties");
        }
        private static void GenerateProperties(this ViewModelDocumentBuilder vmBuilder, IEnumerable<PropertyToGenerate>? propertiesToGenerate)
        {
            if (propertiesToGenerate?.Any() != true)
            {
                return;
            }

            foreach (PropertyToGenerate propertyToGenerate in propertiesToGenerate)
            {
                vmBuilder.AppendLineBeforeMember();
                GenerateProperty(vmBuilder, propertyToGenerate);
            }
        }
        private static void GenerateProperty(ViewModelDocumentBuilder vmBuilder, PropertyToGenerate p)
        {
            vmBuilder.Append($"public {p.PropertyType} {p.PropertyName}");

            if (p.IsReadOnly)
            {
                vmBuilder.AppendLine($" => {p.BackingField};");
                return;
            }
            else
            {
                vmBuilder.AppendLine();
            }

            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();
            vmBuilder.AppendLine($"get => {p.BackingField};");
            vmBuilder.AppendLine("set");
            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();
            vmBuilder.AppendLine($"if ({p.BackingField} == value)");
            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();
            vmBuilder.AppendLine("return;");
            vmBuilder.DecreaseIndent();
            vmBuilder.AppendLine("}");
            if (p.MethodsToCall != null)
            {
                vmBuilder.AppendLine($"PropertyChangeArg<{p.PropertyType}> changeArgument = new PropertyChangeArg<{p.PropertyType}>({p.BackingField}, value);");
            }
            vmBuilder.AppendLine($"{p.BackingField} = value;");
            vmBuilder.AppendLine($"RaisePropertyChanged(nameof({p.PropertyName}));");
            if (p.PropertiesToInvalidate != null)
            {
                foreach (string? propertyToInvalidate in p.PropertiesToInvalidate)
                {
                    vmBuilder.AppendLine($"RaisePropertyChanged(\"{propertyToInvalidate}\");");
                }
            }
            if (p.CommandsToInvalidate != null)
            {
                foreach (CommandToGenerate? commandToInvalidate in p.CommandsToInvalidate)
                {
                    vmBuilder.AppendLine($"{commandToInvalidate.PropertyName}.RaiseCanExecuteChanged();");
                }
            }
            if (p.MethodsToCall != null)
            {
                vmBuilder.AppendLine($"{p.MethodsToCall.MethodName}(changeArgument);");
            }
            vmBuilder.DecreaseIndent();
            vmBuilder.AppendLine("}");
            vmBuilder.DecreaseIndent();
            vmBuilder.AppendLine("}");
        }
        private static void GenerateComplexProperties(this ViewModelDocumentBuilder vmBuilder, IEnumerable<ComplexPropertyToGenerate>? propertiesToGenerate)
        {
            if (propertiesToGenerate?.Any() != true)
            {
                return;
            }

            vmBuilder.AppendLineBeforeMember();
            foreach (ComplexPropertyToGenerate propertyToGenerate in propertiesToGenerate)
            {
                GenerateComplexProperty(vmBuilder, propertyToGenerate);
            }
        }
        private static void GenerateComplexProperty(ViewModelDocumentBuilder vmBuilder, ComplexPropertyToGenerate p)
        {
            vmBuilder.Append($"public {p.PropertyType} {p.PropertyName}");

            if (p.IsReadOnly)
            {
                vmBuilder.AppendLine($" => {p.BackingField}.{p.Path};");
                return;
            }
            else
            {
                vmBuilder.AppendLine();
            }

            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();
            vmBuilder.AppendLine($"get => {p.BackingField}.{p.Path};");
            vmBuilder.AppendLine("set");
            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();
            vmBuilder.AppendLine($"if ({p.BackingField}?.{p.Path} == value)");
            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();
            vmBuilder.AppendLine("return;");
            vmBuilder.DecreaseIndent();
            vmBuilder.AppendLine("}");
            if (p.MethodsToCall != null)
            {
                vmBuilder.AppendLine($"PropertyChangeArg<{p.PropertyType}> changeArgument = new PropertyChangeArg<{p.PropertyType}>({p.BackingField}.{p.Path}, value);");
            }
            vmBuilder.AppendLine($"{p.BackingField}.{p.Path} = value;");
            vmBuilder.AppendLine($"RaisePropertyChanged(nameof({p.PropertyName}));");
            if (p.MethodsToCall != null)
            {
                vmBuilder.AppendLine($"{p.MethodsToCall.MethodName}(changeArgument);");
            }
            vmBuilder.DecreaseIndent();
            vmBuilder.AppendLine("}");
            vmBuilder.DecreaseIndent();
            vmBuilder.AppendLine("}");
        }
        internal static void GeneratePropertyChangeHandlers(this ViewModelDocumentBuilder vmBuilder, IEnumerable<PropertyToGenerate>? propertiesToGenerate, IEnumerable<ComplexPropertyToGenerate>? complexPropertiesToGenerate)
        {
            if (propertiesToGenerate?.Any() != true && complexPropertiesToGenerate?.Any() != true)
            {
                return;
            }

            vmBuilder.AppendLine();
            vmBuilder.AppendLine("#region Property handlers");
            vmBuilder.GeneratePropertyChangeHandlers(propertiesToGenerate);
            vmBuilder.GenerateComplexPropertyChangeHandlers(complexPropertiesToGenerate);
            vmBuilder.AppendLine("#endregion Property handlers");
        }
        private static void GeneratePropertyChangeHandlers(this ViewModelDocumentBuilder vmBuilder, IEnumerable<PropertyToGenerate>? propertiesToGenerate)
        {
            IEnumerable<PropertyToGenerate>? propertiesWithChangeHandler = propertiesToGenerate?.Where(p => p.MethodsToCall != null);
            if (propertiesWithChangeHandler?.Any() != true)
            {
                return;
            }

            foreach (PropertyToGenerate handler in propertiesWithChangeHandler)
            {
                vmBuilder.AppendLine($"partial void {handler?.MethodsToCall?.MethodName}(PropertyChangeArg<{handler?.PropertyType}> arg);");
            }
        }
        private static void GenerateComplexPropertyChangeHandlers(this ViewModelDocumentBuilder vmBuilder, IEnumerable<ComplexPropertyToGenerate>? propertiesToGenerate)
        {
            IEnumerable<ComplexPropertyToGenerate>? propertiesWithChangeHandler = propertiesToGenerate?.Where(p => p.MethodsToCall != null);
            if (propertiesWithChangeHandler?.Any() != true)
            {
                return;
            }

            foreach (PropertyToGenerate handler in propertiesWithChangeHandler)
            {
                vmBuilder.AppendLineBeforeMember();
                vmBuilder.AppendLine($"partial void {handler?.MethodsToCall?.MethodName}(PropertyChangeArg<{handler?.PropertyType}> arg);");
            }
        }
    }
}
