using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

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


        }

        #endregion

        #region Properties



        #endregion

        #region Methods



        #endregion
    }
}
