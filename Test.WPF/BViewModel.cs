using System;
using System.Threading.Tasks;
using System.Windows.Input;

using BlackPearl.Prism.Core.WPF;

using Prism.Commands;
using Prism.Regions;

namespace Test.WPF
{
    public class BViewModel : BlackPearlViewModelBase
    {
        private string myProperty;

        public string MyProperty
        {
            get => myProperty; set
            {
                myProperty = value;
                RaisePropertyChanged();
            }
        }

        public ICommand BackCommand => new DelegateCommand(BackAction);

        private void BackAction() => navigationService.Journal.GoBack();

        protected override Task LoadAction()
        {
            return Task.Run(() =>
            {
                MyProperty += ":" + DateTime.Now.ToString();
            });
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            var myData = new MyData();
            navigationContext.Parameters.MapNavigationValueToObject(myData);

            MyProperty = myData.LastName + " " + myData.FirstName;
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext) => false;
    }
}
