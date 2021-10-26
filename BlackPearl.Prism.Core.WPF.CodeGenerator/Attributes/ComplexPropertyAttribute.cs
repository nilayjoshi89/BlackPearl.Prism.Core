using System;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator
{
    [AttributeUsage((AttributeTargets.Field | AttributeTargets.Property), AllowMultiple = false)]
    public class ComplexPropertyAttribute : Attribute
    {
        public ComplexPropertyAttribute(string propertyPath, string propertyName, bool generatePropertyChangeHandler = false)
        {
            PropertyPath = propertyPath;
            PropertyName = propertyName;
            GeneratePropertyChangeHandler = generatePropertyChangeHandler;
        }

        /// <summary>
        /// Gets or sets the name of property to generate.
        /// </summary>
        public string PropertyName { get; set; }
        public bool GeneratePropertyChangeHandler { get; set; }
        public string PropertyPath { get; }
    }
}