using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using ShogunVS.Models;
using ShogunVS.Modules;
using ShogunVS.Services;
using ShogunVS.Settings;
using ShogunVS.Views;
using System.Windows;

namespace ShogunVS
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<CameraDevice>();
            containerRegistry.RegisterSingleton<CameraStreaming>();
            containerRegistry.RegisterSingleton<YeelightControl>();
            containerRegistry.RegisterSingleton<ImageProcessing>();
            containerRegistry.RegisterSingleton<FiltersSettings>();
            containerRegistry.RegisterSingleton<Results>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MainModule>("MainModule");
        }
    }
}
