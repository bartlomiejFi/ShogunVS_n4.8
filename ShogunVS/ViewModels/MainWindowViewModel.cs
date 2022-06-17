using ShogunVS.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShogunVS.ViewModels
{
    public class MainWindowViewModel : CommonViewModel
    {
        #region Fields

        private IRegionManager _RegionManager;
        private IModuleManager _ModuleManager;

        #endregion

        #region Constructors

        public MainWindowViewModel(IEventAggregator eventAggregator, IContainerExtension container, IRegionManager regionManager, IModuleManager moduleManager, IModuleCatalog moduleCatalog)
            : base(eventAggregator, container)
        {
            _RegionManager = regionManager;
            _ModuleManager = moduleManager;

            InitializeModulesCommand = new DelegateCommand(InitializeModules);

            //AlertService.OnUpdate += AlertService_OnUpdate;
            //AlertService_OnUpdate(this, new EventArgs());
        }

        #endregion

        #region Properties

        public DelegateCommand InitializeModulesCommand { get; private set; }
        public DelegateCommand GoToAlertScreenCommand { get; private set; }
        public DelegateCommand DisplaySherpaCommand { get; private set; }

        //public string AlertButtonContent
        //{
        //    get { return _AlertButtonContent; }
        //    set { SetProperty(ref _AlertButtonContent, value); }
        //}

        //public Brush AlertButtonColor
        //{
        //    get { return _AlertButtonColor; }
        //    set { SetProperty(ref _AlertButtonColor, value); }
        //}

        #endregion

        #region Methods

        //private void AlertService_OnUpdate(object sender, EventArgs e)
        //{
        //    int count = AlertService.ActiveAlerts.Count;
        //    if (count >= 4)
        //    {
        //        AlertButtonContent = $"{count} błędów";
        //        AlertButtonColor = Brushes.DarkRed;
        //    }
        //    else if (count >= 2 && count <= 3)
        //    {
        //        AlertButtonContent = $"{count} błędy";
        //        AlertButtonColor = Brushes.DarkRed;
        //    }
        //    else if (AlertService.ActiveAlerts.Count == 1)
        //    {
        //        AlertButtonContent = $"{count} błąd";
        //        AlertButtonColor = Brushes.DarkRed;
        //    }
        //    else
        //    {
        //        AlertButtonContent = "Status OK";
        //        AlertButtonColor = Brushes.ForestGreen;
        //    }
        //}

        private void InitializeModules()
        {
            //  var plc = Container.Resolve<PlcService>();
            // plc.Open();

        }

        //private void GoToAlertScreen()
        //{
        //    const string target = "AlertScreen";
        //    var navigationService = _RegionManager.Regions["ContentRegion"].NavigationService;

        //    var currentView = navigationService.Journal.CurrentEntry.Uri.OriginalString;
        //    if (currentView == target)
        //        return;

        //    navigationService.RequestNavigate(target);
        //}

        #endregion
    }
}
