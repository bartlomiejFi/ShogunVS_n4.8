using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using ShogunVS.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ShogunVS.Services;
using System.Windows;

namespace ShogunVS.ViewModels
{
    public class CameraViewModel : CommonViewModel
    {
        #region Fields

        private CameraDevice _selectedCamera;

        private List<CameraDevice> _cameraList;

        private ConnectionStatus _camStatus;

        private ImageSource _imageSource;

        private WriteableBitmap _writableBitmap;

        private SolidColorBrush _firstColor;

        private SolidColorBrush _secondColor;

        private SolidColorBrush _thirdColor;

        private SolidColorBrush _fourthColor;

        private SolidColorBrush _fifthColor;

        private int _firstArmyNo;

        private int _secondArmyNo;

        private int _thirdArmyNo;

        private int _fourthArmyNo;

        private int _fifthArmyNo;

        private int _neutralArmyNo;

        private int _reusltsNo;

        private Results[] ResultsArray = new Results[3];
        #endregion

        #region Constructors

        public CameraViewModel(IEventAggregator eventAggregator, IContainerExtension container)
            : base(eventAggregator, container)
        {
            results = container.Resolve<Results>();
            imageProcessing = container.Resolve<ImageProcessing>();
            yeelightControl = container.Resolve<YeelightControl>();


            StartStopStreamingCommand = new DelegateCommand(StartStopStreamingInit);
            AzizCommand = new DelegateCommand(yeelightControl.Aziz);
            ExitCommand = new DelegateCommand(Exit);

            CameraList = CamerasDetector.CameraDevices();
            cameraStreaming = container.Resolve<CameraStreaming>();
            // cameraStreaming = new CameraStreaming();
            cameraStreaming.OnFrameUpdate += OnFrameUpdate;
            SelectedCamera = CameraList.LastOrDefault();
            StartStopStreamingInit();

            imageProcessing.OnProcessedFramesUpdate += OnProcessedFramesUpdate;
        }

        #endregion

        #region Properties


        public DelegateCommand StartStopStreamingCommand { get; private set; }

        public DelegateCommand AzizCommand { get; private set; }

        public DelegateCommand ExitCommand { get; private set; }

        private CameraStreaming cameraStreaming { get; set; }

        private ImageProcessing imageProcessing { get; set; }

        private YeelightControl yeelightControl { get; set; }

        private Results results { get; set; }

        public SolidColorBrush FirstColor
        {
            get { return _firstColor; }

            set { SetProperty(ref _firstColor, value); }
        }
   
        public SolidColorBrush SecondColor
        {
            get { return _secondColor; }

            set { SetProperty(ref _secondColor, value); }
        }

        public SolidColorBrush ThirdColor
        {
            get { return _thirdColor; }

            set { SetProperty(ref _thirdColor, value); }
        }

        public SolidColorBrush FourthColor
        {
            get { return _fourthColor; }

            set { SetProperty(ref _fourthColor, value); }
        }

        public SolidColorBrush FifthColor
        {
            get { return _fifthColor; }

            set { SetProperty(ref _fifthColor, value); }
        }

        public int FirstArmyNo
        {
            get { return _firstArmyNo; }

            set { SetProperty(ref _firstArmyNo, value); }
        }

        public int SecondArmyNo
        {
            get { return _secondArmyNo; }

            set { SetProperty(ref _secondArmyNo, value); }
        }

        public int ThirdArmyNo
        {
            get { return _thirdArmyNo; }

            set { SetProperty(ref _thirdArmyNo, value); }
        }

        public int FourthArmyNo
        {
            get { return _fourthArmyNo; }

            set { SetProperty(ref _fourthArmyNo, value); }
        }

        public int FifthArmyNo
        {
            get { return _fifthArmyNo; }

            set { SetProperty(ref _fifthArmyNo, value); }
        }

        public int NeutralArmyNo { get => _neutralArmyNo; set => SetProperty(ref _neutralArmyNo, value); }

        public CameraDevice SelectedCamera
        {
            get { return _selectedCamera; }

            set { SetProperty(ref _selectedCamera, value); }
        }

        public List<CameraDevice> CameraList
        {
            get { return _cameraList; }

            set { SetProperty(ref _cameraList, value); }
        }

        public ImageSource ImageSource
        {
            get { return _imageSource; }

            set { SetProperty(ref _imageSource, value); }
        }

        public WriteableBitmap WriteableBitmap
        {
            get { return _writableBitmap; }

            set { SetProperty(ref _writableBitmap, value); }
        }

        public ConnectionStatus CamStatus { get => _camStatus; set => SetProperty(ref _camStatus, value); }

        #endregion

        #region Methods

        private async void StartStopStreamingInit()
        {
            if (CamStatus == ConnectionStatus.Connected)
            {
                await cameraStreaming.StopStreaming();
                cameraStreaming.OnFrameUpdate -= OnFrameUpdate;
                CamStatus = ConnectionStatus.Disconnected;
            }
            else
            {
                await cameraStreaming.StartStreaming(SelectedCamera.OpenCvId);
                cameraStreaming.OnFrameUpdate += OnFrameUpdate;
                CamStatus = ConnectionStatus.Connected;
            }
        }

        private void OnFrameUpdate(object sender,Mat newFrame)
        {
            //var bmp = newFrame.ToWriteableBitmap(); 
            //bmp.Freeze();
            //WriteableBitmap = bmp;         
        }

        private void OnProcessedFramesUpdate(object sender, ProcessedFrames processedFrames)
        {
            var bmp = processedFrames.GameResult.ToWriteableBitmap();
            bmp.Freeze();
            WriteableBitmap = bmp;

            ResultsArray[2] = this.ResultsArray[1];
            ResultsArray[1] = this.ResultsArray[0];
            ResultsArray[0] = results;

            if (ResultsArray[0] == ResultsArray[1] && ResultsArray[0] == ResultsArray[2])
            {
                FirstArmyNo = results.first.armyNo;
                FirstColor = results.first.color;

                SecondArmyNo = results.second.armyNo;
                SecondColor = results.second.color;

                ThirdArmyNo = results.third.armyNo;
                ThirdColor = results.third.color;

                FourthArmyNo = results.fourth.armyNo;
                FourthColor = results.fourth.color;

                FifthArmyNo = results.fifth.armyNo;
                FifthColor = results.fifth.color;

                NeutralArmyNo = results.neutral.armyNo;
            }
        }

        private void Exit()
        {
            Application.Current.MainWindow.Close();
        }

        #endregion
    }
}
