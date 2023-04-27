using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using ShogunVS.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShogunVS.Modules
{
    public class MainModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RequestNavigate("ContentRegion", "ServiceScreen");
            regionManager.RequestNavigate("ContentRegion", "Camera");
            regionManager.RegisterViewWithRegion("TopBar", typeof(TopBar));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWindow>();
            containerRegistry.RegisterForNavigation<Camera>();
            containerRegistry.RegisterForNavigation<ServiceScreen>();
        }
    }
}