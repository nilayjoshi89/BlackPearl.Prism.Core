using System;
using System.Threading.Tasks;
using System.Windows.Input;

using BlackPearl.Prism.Core.WPF;

using Prism.Commands;
using Prism.Regions;

namespace Test.WPF
{
    public class AViewModel : BlackPearlViewModelBase
    {
        private string myProperty;
        private MyData myData;

        public string MyProperty
        {
            get => myProperty; set
            {
                myProperty = value;
                RaisePropertyChanged();
            }
        }

        public ICommand NavigateToBCommand => new DelegateCommand(NavigateToBAction);

        private void NavigateToBAction()
        {
            var nagParam = new NavigationParameters();
            nagParam.UpdateNavigationParameter(myData);
            navigationService.RequestNavigate("NavigateToB", nagParam);
        }

        protected override Task LoadAction()
        {
            return Task.Run(() =>
            {
                MyProperty += " " + DateTime.Now.ToString();
            });
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            myData = new MyData();
            navigationContext.Parameters.MapNavigationValueToObject(myData);

            MyProperty = myData.FirstName + ":" + myData.LastName;
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext) => true;
    }
}
