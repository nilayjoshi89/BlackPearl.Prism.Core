using System.Collections.Generic;
using System.Linq;

using BlackPearl.Prism.Core.WPF.CodeGenerator.Model;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator
{
    internal static class CommandPropertyGenerator
    {
        internal static void GenerateCommandProperties(this ViewModelDocumentBuilder vmBuilder, IEnumerable<CommandToGenerate>? commandsToGenerate)
        {
            if (commandsToGenerate?.Any() != true)
            {
                return;
            }

            vmBuilder.AppendLine();
            vmBuilder.AppendLine("#region Commands");
            foreach (CommandToGenerate? commandToGenerate in commandsToGenerate)
            {
                string genericParameterString = commandToGenerate.ExecuteMethod.ArgumentType == null
                                                ? ""
                                                : "<" + commandToGenerate.ExecuteMethod.ArgumentType + ">";
                vmBuilder.AppendLine($"public DelegateCommand{genericParameterString} {commandToGenerate.PropertyName} {{ get; private set; }}");
            }
            vmBuilder.AppendLine("#endregion Commands");
        }
    }
}
