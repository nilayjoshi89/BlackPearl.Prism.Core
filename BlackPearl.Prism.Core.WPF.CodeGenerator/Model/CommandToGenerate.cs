using System;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator.Model
{
    internal class CommandToGenerate
    {
        public CommandToGenerate(string propertyName, MethodInfo executeMethod)
        {
            ExecuteMethod = executeMethod;
            PropertyName = propertyName;
        }

        public MethodInfo ExecuteMethod { get; }

        public string PropertyName { get; }

        public MethodInfo? CanExecuteMethod { get; set; }

        public string[]? CanExecuteAffectingProperties { get; set; }
    }

    internal struct MethodInfo
    {
        public MethodInfo(string name) : this()
        {
            Name = name;
        }

        public string Name { get; set; }
        public string? ArgumentType { get; set; }
    }
}
