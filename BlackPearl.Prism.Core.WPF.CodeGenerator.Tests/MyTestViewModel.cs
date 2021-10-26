
using System;

using Prism.Commands;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator.Tests
{
    [ViewModel]
    public partial class MyTestViewModel : BlackPearlViewModelBase
    {
        DelegateCommand<MyCustomData> Comm = new DelegateCommand<MyCustomData>(MyCommand, CanExecuteMyCommand);

        private static bool CanExecuteMyCommand(MyCustomData arg) => throw new NotImplementedException();
        private static void MyCommand(MyCustomData obj) => throw new NotImplementedException();
    }

    public partial class MyTestViewModel
    {
        [Property(propertyName:"ABC")]
        private int _abc;

        
        [PropertyInvalidate("ABC")]
        public int NewProperty => _abc + 2;

        partial void AnotherPartialMethodCall(int abc, int value);

        [CommandInvalidate("ABC", "Unknown")]
        [CommandInvalidate("UnknownProperty")]
        [Command(PropertyName = "ABCCommand", CanExecuteMethod = nameof(CanExecuteABCCommand))]
        void OnABCChanged(MyCustomData data)
        {

        }

        public bool CanExecuteABCCommand(MyCustomData data)
        {
            return true;
        }
    }

    public class MyCustomData
    {

    }
}
