// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;

using BlackPearl.Prism.Core.WPF.CodeGenerator.Model;

using Microsoft.CodeAnalysis;

namespace MvvmGen.Inspectors
{
    internal static class ModelMemberInspector
    {
        internal static string? Inspect(AttributeData viewModelAttributeData, IList<PropertyToGenerate> propertiesToGenerate)
        {
            string? wrappedModelType = null;

            var modelTypedConstant = (TypedConstant?)viewModelAttributeData.ConstructorArguments.FirstOrDefault();

            foreach (var arg in viewModelAttributeData.NamedArguments)
            {
                if (arg.Key == "ModelType")
                {
                    modelTypedConstant = arg.Value;
                }
            }

            if (modelTypedConstant?.Value != null)
            {
                if (modelTypedConstant.Value.Value is INamedTypeSymbol model)
                {
                    wrappedModelType = $"{model}";
                    var members = model.GetMembers();
                    foreach (var member in members)
                    {
                        if (member is IMethodSymbol { MethodKind: MethodKind.PropertyGet } methodSymbol)
                        {
                            var propertySymbol = (IPropertySymbol?)methodSymbol.AssociatedSymbol;
                            if (propertySymbol != null)
                            {
                                propertiesToGenerate.Add(new PropertyToGenerate(
                                  propertySymbol.Name, propertySymbol.Type.ToString(), $"Model.{propertySymbol.Name}", propertySymbol.IsReadOnly));
                            }
                        }
                    }
                }
            }

            return wrappedModelType;
        }
    }
}
