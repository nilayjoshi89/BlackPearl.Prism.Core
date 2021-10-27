namespace BlackPearl.Prism.Core.WPF.CodeGenerator.Model
{
    internal class ComplexPropertyToGenerate : PropertyToGenerate
    {
        public ComplexPropertyToGenerate(string propertyName, string propertyType, string path, string backingField, bool isReadOnly = false)
            :base(propertyName, propertyType, backingField, isReadOnly)
        {
            Path = path;
        }

        public string? Path { get; set; }
    }
}
