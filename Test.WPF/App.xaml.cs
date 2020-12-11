using System;
using System.Windows;

using BlackPearl.Prism.Core;
using BlackPearl.Prism.Core.WPF;
using BlackPearl.Prism.Core.WPF.Region;

using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Test.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell() => Container.Resolve<MainWindow>();
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            RegisterModuleType(moduleCatalog, typeof(BlackPearlCoreModule));
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();

            IRegionManager regionManager = Container.Resolve<IRegionManager>();
            var navigationParameter = new NavigationParameters();
            navigationParameter.UpdateNavigationParameter(new MyData()
            {
                FirstName = "Nilay",
                LastName = "Joshi"
            });
            regionManager.RequestNavigate("ContentRegion", "NavigateToA", navigationParameter);
        }
        protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            base.ConfigureDefaultRegionBehaviors(regionBehaviors);
            regionBehaviors.AddIfMissing(nameof(DisposeClosedViewsBehavior), typeof(DisposeClosedViewsBehavior));
        }
        private void RegisterModuleType(IModuleCatalog moduleCatalog, Type typeOfModule)
        {
            moduleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = typeOfModule.Name,
                ModuleType = typeOfModule.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable
            });
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AView, AViewModel>("NavigateToA");
            containerRegistry.RegisterForNavigation<BView, BViewModel>("NavigateToB");
        }
    }
}
