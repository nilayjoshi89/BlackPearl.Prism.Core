
using BlackPearl.Prism.Core.WPF.CodeGenerator.Model;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator.Inspectors
{
    internal static class ViewModelInspector
    {
        internal static void Inspect(ViewModelToGenerate viewModelToGenerate)
        {
            viewModelToGenerate.InspectForCommandAttribute();
            viewModelToGenerate.InspectPropertyAttribute();
            viewModelToGenerate.ProcessCommandInvalidate();
            viewModelToGenerate.InspectPropertyInvalidateAttribute();
            viewModelToGenerate.InspectComplexPropertyAttribute();
        }
    }
}