using System.Collections.Generic;
using System.Linq;

using BlackPearl.Prism.Core.WPF.CodeGenerator.Extensions;
using BlackPearl.Prism.Core.WPF.CodeGenerator.Model;

using Microsoft.CodeAnalysis;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator.Inspectors
{
    internal static class ViewModelPropertyInvalidateAttribute
    {
        internal static void InspectPropertyInvalidateAttribute(this ViewModelToGenerate viewModelToGenerate)
        {
            IEnumerable<IPropertySymbol> viewModelMembers = viewModelToGenerate.Symbols.OfType<IPropertySymbol>();
            foreach (IPropertySymbol propertySymbol in viewModelMembers)
            {
                viewModelToGenerate.PopulatePropertyInvalidate(propertySymbol);
            }
        }

        private static void PopulatePropertyInvalidate(this ViewModelToGenerate viewModelToGenerate, IPropertySymbol propertySymbol)
        {
            IEnumerable<AttributeData> attributeDatas = propertySymbol.GetAttributeData<PropertyInvalidateAttribute>();

            if (attributeDatas?.Any() != true)
            {
                return;
            }

            foreach (AttributeData? attr in attributeDatas)
            {
                string propertyNameWithAttributes = propertySymbol.Name;
                IEnumerable<string>? propertyNames = attr.GetStringValueFromAttributeArgument();

                if (propertyNames == null)
                {
                    continue;
                }

                foreach (string? propertyName in propertyNames)
                {
                    PropertyToGenerate? p = viewModelToGenerate.PropertiesToGenerate.FirstOrDefault(p => p.PropertyName == propertyName);
                    if (p == null)
                    {
                        continue;
                    }

                    if (!p.PropertiesToInvalidate.Contains(propertyNameWithAttributes))
                    {
                        p.PropertiesToInvalidate.Add(propertyNameWithAttributes);
                    }
                }
            }
        }
    }
}