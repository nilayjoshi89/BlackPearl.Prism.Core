using System.Collections.Generic;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator.Model
{
    internal class PropertyToGenerate
    {
        public PropertyToGenerate(string propertyName, string propertyType, string backingField, bool isReadOnly = false)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
            BackingField = backingField;
            IsReadOnly = isReadOnly;
            CommandsToInvalidate = new List<CommandToGenerate>();
            PropertiesToInvalidate = new List<string>();
        }

        public string PropertyName { get; }

        public string PropertyType { get; }

        public string BackingField { get; }

        public bool IsReadOnly { get; }

        public List<CommandToGenerate>? CommandsToInvalidate { get; set; }

        public MethodToCall? MethodsToCall { get; set; }

        public List<string> PropertiesToInvalidate { get; set; }
    }

    internal class MethodToCall
    {
        public MethodToCall(string methodName)
        {
            MethodName = methodName;
        }

        public string MethodName { get; }
    }
}
