using System.Collections.Generic;
using System.Linq;

using BlackPearl.Prism.Core.WPF.CodeGenerator.Extensions;
using BlackPearl.Prism.Core.WPF.CodeGenerator.Model;

using Microsoft.CodeAnalysis;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator.Inspectors
{
    internal static class ViewModelPropertyAttributeInspector
    {
        internal static void InspectPropertyAttribute(this ViewModelToGenerate viewModelToGenerate)
        {
            IEnumerable<IFieldSymbol> viewModelMembers = viewModelToGenerate.Symbols.OfType<IFieldSymbol>();
            foreach (IFieldSymbol fieldSymbol in viewModelMembers)
            {
                PropertyToGenerate? p = GenerateProperty(fieldSymbol);
                if (p != null)
                {
                    viewModelToGenerate.PropertiesToGenerate.Add(p);
                }
            }
        }

        private static PropertyToGenerate? GenerateProperty(IFieldSymbol fieldSymbol)
        {
            AttributeData? propertyAttributeData = fieldSymbol.GetAttributeData<PropertyAttribute>()?.FirstOrDefault();
            if (propertyAttributeData == null)
            {
                return null;
            }

            string? propertyType = fieldSymbol.Type.ToString();
            string? fieldName = fieldSymbol.Name;
            string propertyName = (propertyAttributeData.GetNamedArgumentValue(nameof(PropertyAttribute.PropertyName))
                                    ?? propertyAttributeData.ConstructorArguments.FirstOrDefault().Value?.ToString()
                                    ?? fieldSymbol.Name).ConvertToPropertyName();
            bool generatePropertyChangeHandler = propertyAttributeData.GetNamedArgumentValue<bool>(nameof(PropertyAttribute.GeneratePropertyChangeHandler));
            MethodToCall? methodsToCall = (generatePropertyChangeHandler) ? new MethodToCall($"On{propertyName}ChangedHandler") : null;

            return new PropertyToGenerate(propertyName, propertyType, fieldName)
            {
                MethodsToCall = methodsToCall
            };
        }
    }
}