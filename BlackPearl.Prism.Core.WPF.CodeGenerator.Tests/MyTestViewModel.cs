using System.ComponentModel.Design;

using BlackPearl.Prism.Core.WPF;
using BlackPearl.Prism.Core.WPF.CodeGenerator;

namespace MyCustomNamespace
{
    [ViewModel]
    public partial class MyTestViewModel
    {
        [Property(propertyName: "ABC", GeneratePropertyChangeHandler = true)]
        private int _abc;
        [Property]
        private string unknown;

        [PropertyInvalidate("ABC")]
        public int NewProperty => _abc + 2;

        [CommandInvalidate("ABC", "Unknown")]
        [CommandInvalidate("UnknownProperty")]
        [Command(PropertyName = "ABCCommand", CanExecuteMethod = nameof(CanExecuteABCCommand))]
        private void OnABCChanged(MyCustomData data)
        {

        }

        [ComplexProperty("FirstName", "string", "ModelFirstName", GeneratePropertyChangeHandler = true)]
        private MyCustomData model;

        public bool CanExecuteABCCommand(MyCustomData data) => true;
    }

    public class MyCustomData
    {
        public string FirstName { get; set; }
    }
}
