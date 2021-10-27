namespace BlackPearl.Prism.Core.WPF.CodeGenerator
{
    internal static class UsingDirectivesGenerator
    {
        internal static void GenerateUsingDirectives(this ViewModelDocumentBuilder vmBuilder)
        {
            vmBuilder.AppendLine("using BlackPearl.Prism.Core.WPF;");
            vmBuilder.AppendLine("using Prism.Commands;");
            vmBuilder.AppendLine();
        }
    }
}
