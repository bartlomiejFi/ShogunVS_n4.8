using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using ShogunVS.Models;
using ShogunVS.Modules;
using ShogunVS.Services;
using ShogunVS.Settings;
using ShogunVS.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ShogunVS
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
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
