﻿using System;
using System.Linq;
using System.ComponentModel;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator
{
    /// <summary>
    /// Set this attribute once or multiple times on a readonly property that depends on another property.
    /// Typical case is you define a FullName property in your ViewModel that depends on FirstName and LastName.
    /// You would create it like this:
    /// <code>
    /// [PropertyInvalidate(nameof(LastName))]
    /// [PropertyInvalidate(nameof(FirstName))]
    /// public string FullName => $"{FirstName} {LastName}";
    /// </code>
    /// Then the <see cref="INotifyPropertyChanged.PropertyChanged"/> event is raised for the FullName property in the setters of the properties FirstName and LastName.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class PropertyInvalidateAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyInvalidateAttribute"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property in which the <see cref="INotifyPropertyChanged.PropertyChanged"/> event is raised.</param>
        /// /// <param name="morePropertyNames">More properties in which the <see cref="INotifyPropertyChanged.PropertyChanged"/> event is raised.</param>
        public PropertyInvalidateAttribute(string propertyName, params string[] morePropertyNames)
        {
            PropertyNames = new[] { propertyName }.Concat(morePropertyNames).ToArray();
        }

        /// <summary>
        /// Gets the property names in which the <see cref="INotifyPropertyChanged.PropertyChanged"/> event is raised.
        /// </summary>
        public string[] PropertyNames { get; }
    }
}
