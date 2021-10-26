using System;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAttribute"/> class.
        /// </summary>
        public CommandAttribute() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAttribute"/> class.
        /// </summary>
        /// <param name="canExecuteMethod">The name of the method with the can-execute logic</param>
        public CommandAttribute(string canExecuteMethod)
        {
            CanExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        /// Gets or sets the name of the method with the can-execute logic.
        /// </summary>
        public string? CanExecuteMethod { get; set; }

        /// <summary>
        /// Gets or sets the name of the command property.
        /// </summary>
        public string? PropertyName { get; set; }
    }
}