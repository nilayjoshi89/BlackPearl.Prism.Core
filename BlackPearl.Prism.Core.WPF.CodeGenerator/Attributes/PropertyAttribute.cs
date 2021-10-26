using System;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class PropertyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAttribute"/> class.
        /// </summary>
        public PropertyAttribute() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAttribute"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property to generate</param>
        /// /// <param name="generatePropertyChangeHandler">Generate property change handler method</param>
        public PropertyAttribute(string propertyName, bool generatePropertyChangeHandler = false)
        {
            PropertyName = propertyName;
            GeneratePropertyChangeHandler = generatePropertyChangeHandler;
        }

        /// <summary>
        /// Gets or sets the name of property to generate.
        /// </summary>
        public string? PropertyName { get; set; }
        public bool GeneratePropertyChangeHandler { get; set; }
    }
}