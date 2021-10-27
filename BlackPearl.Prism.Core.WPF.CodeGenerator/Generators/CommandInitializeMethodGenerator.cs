using System.Collections.Generic;
using System.Linq;

using BlackPearl.Prism.Core.WPF.CodeGenerator.Model;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator
{
    internal static class CommandInitializeMethodGenerator
    {
        internal static void GenerateCommandInitializeMethod(this ViewModelDocumentBuilder vmBuilder, IEnumerable<CommandToGenerate>? commandsToGenerate)
        {
            if (commandsToGenerate != null && commandsToGenerate.Any())
            {
                vmBuilder.AppendLineBeforeMember();
                vmBuilder.AppendLine("#region Method Overrides");
                vmBuilder.AppendLine("protected override void Initialize()");
                vmBuilder.AppendLine("{");
                vmBuilder.IncreaseIndent();
                foreach (var commandToGenerate in commandsToGenerate)
                {
                    string genericParameterString = commandToGenerate.ExecuteMethod.ArgumentType == null
                                                    ? ""
                                                    : "<" + commandToGenerate.ExecuteMethod.ArgumentType + ">";
                    vmBuilder.Append($"{commandToGenerate.PropertyName} = new DelegateCommand{genericParameterString}({commandToGenerate.ExecuteMethod.Name}");
                    if (commandToGenerate.CanExecuteMethod.HasValue)
                    {
                        vmBuilder.Append($", {commandToGenerate.CanExecuteMethod.Value.Name}");
                    }
                    vmBuilder.AppendLine(");");
                }
                vmBuilder.DecreaseIndent();
                vmBuilder.AppendLine("}");
                vmBuilder.AppendLine("#endregion Method Overrides");
            }
        }
    }
}
