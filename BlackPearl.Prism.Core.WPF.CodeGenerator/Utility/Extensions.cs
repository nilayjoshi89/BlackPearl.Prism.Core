using System.Collections.Generic;
using System.Linq;

using BlackPearl.Prism.Core.WPF.CodeGenerator.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator.Extensions
{
    public static class Extensions
    {
        public static string GetStringValueFromAttributeArgument(this AttributeArgumentSyntax attributeArgumentSyntax)
        {
            string? stringValue = attributeArgumentSyntax.Expression switch
            {
                InvocationExpressionSyntax invocationExpressionSyntax => invocationExpressionSyntax.ArgumentList.Arguments[0].ToString(),
                LiteralExpressionSyntax literalExpressionSyntax => literalExpressionSyntax.Token.ValueText,
                _ => attributeArgumentSyntax.Expression.ToString()
            };

            return stringValue;
        }
        public static IEnumerable<string>? GetStringValueFromAttributeArgument(this AttributeData attributeData)
        {
            var attributeSyntax = (AttributeSyntax?)attributeData.ApplicationSyntaxReference?.GetSyntax();
            return attributeSyntax?.ArgumentList?.Arguments
                                        .Select(x => x.GetStringValueFromAttributeArgument())
                                        .Where(s => !string.IsNullOrEmpty(s));
        }
        public static IEnumerable<AttributeData> GetAttributeData<T>(this ISymbol symbol) => symbol.GetAttributes().Where(a => a.AttributeClass?.ToDisplayString() == typeof(T).FullName);
        public static string? GetNamedArgumentValue(this AttributeData attributeData, string propertyName)
            => attributeData.NamedArguments.FirstOrDefault(a => a.Key == propertyName).Value.Value?.ToString();
        public static T GetNamedArgumentValue<T>(this AttributeData attributeData, string propertyName)
        {
            try
            {
                object? argValue = attributeData.NamedArguments.FirstOrDefault(a => a.Key == propertyName).Value.Value;
                if (argValue != null)
                {
                    return (T)argValue;
                }
            }
            catch { }
            return default(T);
        }
        public static IEnumerable<string> GetParameterTypeStrings(this IMethodSymbol methodSymbol)
            => methodSymbol.Parameters.Select(p => p.Type.ToDisplayString());
        internal static MethodInfo GetMethodInfo(this IMethodSymbol methodSymbol)
            => new MethodInfo(methodSymbol.Name) { ArgumentType = methodSymbol.GetParameterTypeStrings().FirstOrDefault() };
        public static string ConvertToPropertyName(this string propertyName)
        {
            if (propertyName.StartsWith("_"))
            {
                propertyName = propertyName.Substring(1);
            }
            else if (propertyName.StartsWith("m_"))
            {
                propertyName = propertyName.Substring(2);
            }

            string? firstCharacter = propertyName.Substring(0, 1).ToUpper();

            propertyName = propertyName.Length > 1
                ? firstCharacter + propertyName.Substring(1)
                : firstCharacter;
            return propertyName;
        }
    }
}
