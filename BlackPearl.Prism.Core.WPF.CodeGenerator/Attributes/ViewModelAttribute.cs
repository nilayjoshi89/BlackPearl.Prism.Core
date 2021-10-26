// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

#nullable enable

using System;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator
{
    /// <summary>
    /// Specifies that a class is a ViewModel. With this attribute set on a class, a partial class definition is generated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ViewModelAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelAttribute"/> class.
        /// </summary>
        public ViewModelAttribute() { }
    }
}
