using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ShogunVS.ViewModels
{
    public class TopBarViewModel : CommonViewModel
    {
        #region Fields

        private WriteableBitmap _writableBitmap;

        #endregion

        #region Constructors

        public TopBarViewModel(IEventAggregator eventAggregator, IContainerExtension container)
            : base(eventAggregator, container)
        {

            //cameraStreaming = container.Resolve<CameraStreaming>();

        }

        #endregion

        #region Properties


        #endregion

        #region Methods

        #endregion
    }
}
