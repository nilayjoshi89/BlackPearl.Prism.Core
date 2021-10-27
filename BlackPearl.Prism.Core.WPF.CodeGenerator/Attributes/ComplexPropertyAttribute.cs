using System;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator
{
    [AttributeUsage((AttributeTargets.Field | AttributeTargets.Property), AllowMultiple = false)]
    public class ComplexPropertyAttribute : Attribute
    {
        public ComplexPropertyAttribute(string propertyPath, string type, string propertyName)
        {
            PropertyPath = propertyPath;
            PropertyName = propertyName;
            Type = type;
        }

        /// <summary>
        /// Gets or sets the name of property to generate.
        /// </summary>
        public string PropertyName { get; set; }
        public bool GeneratePropertyChangeHandler { get; set; }
        public string PropertyPath { get; }
        public string Type { get; }
    }
}