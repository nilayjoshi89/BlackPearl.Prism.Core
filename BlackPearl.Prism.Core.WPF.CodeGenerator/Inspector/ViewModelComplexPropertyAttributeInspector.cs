using System.Collections.Generic;
using System.Linq;

using BlackPearl.Prism.Core.WPF.CodeGenerator.Extensions;
using BlackPearl.Prism.Core.WPF.CodeGenerator.Model;

using Microsoft.CodeAnalysis;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator.Inspectors
{
    internal static class ViewModelComplexPropertyAttributeInspector
    {
        internal static void InspectComplexPropertyAttribute(this ViewModelToGenerate viewModelToGenerate)
        {
            IEnumerable<ISymbol> viewModelMembers = viewModelToGenerate.Symbols.Where(i => i is IFieldSymbol || i is IPropertySymbol);
            foreach (ISymbol fieldSymbol in viewModelMembers)
            {
                ComplexPropertyToGenerate? p = GenerateComplexProperty(fieldSymbol);
                if (p != null)
                {
                    viewModelToGenerate.ComplexPropertiesToGenerate.Add(p);
                }
            }
        }

        private static ComplexPropertyToGenerate? GenerateComplexProperty(ISymbol fieldSymbol)
        {
            AttributeData? propertyAttributeData = fieldSymbol.GetAttributeData<ComplexPropertyAttribute>()?.FirstOrDefault();
            if (propertyAttributeData == null)
            {
                return null;
            }

            string? fieldName = fieldSymbol.Name;
            string path = (propertyAttributeData.GetNamedArgumentValue(nameof(ComplexPropertyAttribute.PropertyPath)))
                                    ?? propertyAttributeData.ConstructorArguments.FirstOrDefault().Value?.ToString()
                                    ?? "";
            string propertyType = (propertyAttributeData.GetNamedArgumentValue(nameof(ComplexPropertyAttribute.Type)))
                                    ?? propertyAttributeData.ConstructorArguments.Skip(1).FirstOrDefault().Value?.ToString()
                                    ?? "";
            string propertyName = (propertyAttributeData.GetNamedArgumentValue(nameof(PropertyAttribute.PropertyName))
                                    ?? propertyAttributeData.ConstructorArguments.Skip(2).FirstOrDefault().Value?.ToString()
                                    ?? fieldSymbol.Name).ConvertToPropertyName();

            bool generatePropertyChangeHandler = propertyAttributeData.GetNamedArgumentValue<bool>(nameof(ComplexPropertyAttribute.GeneratePropertyChangeHandler));
            MethodToCall? methodsToCall = (generatePropertyChangeHandler) ? new MethodToCall($"On{propertyName}ChangedHandler") : null;

            return new ComplexPropertyToGenerate(propertyName, propertyType, path, fieldName)
            {
                MethodsToCall = methodsToCall
            };
        }
    }
}