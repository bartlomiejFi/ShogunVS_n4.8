using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using ShogunVS.Settings;

namespace ShogunVS.ViewModels
{
    public abstract class CommonViewModel : BindableBase, INavigationAware
    {
        #region Fields

        protected IRegionNavigationService _NavigationService;

        #endregion

        #region Constructors

        public CommonViewModel(IEventAggregator eventAggregator, IContainerExtension container)
        {
            // Types.
            EventAggregator = eventAggregator;
            Container = container;

            FiltersSettings = Container.Resolve<FiltersSettings>();


            // Events.
            //EventAggregator.GetEvent<PermissionChangedEvent>().Subscribe(PermissionChanged);

            // Commands.
            NavigateCommand = new DelegateCommand<string>(Navigate);
            GoBackCommand = new DelegateCommand(GoBack);
        }

        #endregion

        #region Properties

        protected IEventAggregator EventAggregator { get; set; }
        protected IContainerExtension Container { get; set; }

        protected FiltersSettings FiltersSettings { get; set; }


        public DelegateCommand<string> NavigateCommand { get; protected set; }
        public DelegateCommand GoBackCommand { get; protected set; }

        #endregion

        #region Methods


        /// <summary>
        /// Navigates to specified View.
        /// </summary>
        /// <param name="navigatePath">Path to View.</param>
        protected virtual void Navigate(string navigatePath)
        {
            if (string.IsNullOrEmpty(navigatePath))
                return;

            _NavigationService.RequestNavigate(navigatePath);
        }

        protected virtual void GoBack()
        {
            if (!_NavigationService.Journal.CanGoBack)
                return;

            _NavigationService.Journal.GoBack();
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            _NavigationService = navigationContext.NavigationService;
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _NavigationService = navigationContext.NavigationService;
        }

        #endregion
    }
}
