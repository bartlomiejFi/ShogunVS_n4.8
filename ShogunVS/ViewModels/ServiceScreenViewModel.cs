using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using ShogunVS.Models;
using ShogunVS.Services;
using ShogunVS.Settings;
using ShogunVS.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ShogunVS.ViewModels
{
  public  class ServiceScreenViewModel :CommonViewModel
    {
        #region Fields

        private WriteableBitmap _writableBitmapRESULT;

        private WriteableBitmap _writableBitmapCOLOR;

        private int _hueMin;

        private int _hueMax;

        private int _satMin;

        private int _satMax;

        private int _valMin;

        private int _valMax;

        private int _yellowArmyNo;

        private int _redArmyNo;

        private int _blueArmyNo;

        private int _blackArmyNo;

        private int _purpleArmyNo;

        private int _greenArmyNo;

        private int _gaussianBlurSize;

        private List<string> _framesList;

        private string _selectedFrame;

        private FiltersSettings _filtersSettings;

        #endregion

        #region Constructors

        public ServiceScreenViewModel(IEventAggregator eventAggregator, IContainerExtension container)
            : base(eventAggregator, container)
        {
            imageProcessing = container.Resolve<ImageProcessing>();
            results = container.Resolve<Results>();
            FramesList = typeof(ProcessedFrames).GetFields().Select(x => x.Name).Where(i => i.Contains("Game") == false).ToList();

            SelectedFrame = FramesList[0];
            SelectedFrame = FramesList[1];
            SelectedFrame = FramesList[2];
            SelectedFrame = FramesList[3];
            SelectedFrame = FramesList[4];
            SelectedFrame = FramesList[5];
            SelectedFrame = FramesList.FirstOrDefault();



            imageProcessing.OnProcessedFramesUpdate += OnProcessedFramesUpdate;


            // Commands.
            SaveSettingsCommand = new DelegateCommand(SaveSettings);
            LoadSettingsCommand = new DelegateCommand(LoadSettings);
        }

        #endregion

        #region Properties

        public DelegateCommand SaveSettingsCommand { get; private set; }
        public DelegateCommand LoadSettingsCommand { get; private set; }

        public FiltersSettings Settings
        {
            get { return _filtersSettings; }
            set { SetProperty(ref _filtersSettings, value); }
        }

        public List<string> FramesList
        {
            get { return _framesList; }

            set { SetProperty(ref _framesList, value); }
        }

       public string SelectedFrame
        {
            get { return _selectedFrame; }

            set { SetProperty(ref _selectedFrame, value, LoadSettings);
            }
        }


        public int HueMin
        {
            get { return _hueMin; }

            set { SetProperty(ref _hueMin, value, SendSettings);
            }
        }

        public int HueMax
        {
            get { return _hueMax; }

            set { SetProperty(ref _hueMax, value, SendSettings);
            }
        }

        public int SatMin
        {
            get { return _satMin; }

            set { SetProperty(ref _satMin, value, SendSettings);  }
        }

        public int SatMax
        {
            get { return _satMax; }

            set { SetProperty(ref _satMax, value, SendSettings);
            }
        }

        public int ValMin
        {
            get { return _valMin; }

            set { SetProperty(ref _valMin, value, SendSettings);
            }
        }

        public int ValMax
        {
            get { return _valMax; }

            set { SetProperty(ref _valMax, value, SendSettings);
            }
        }

        public int YellowArmyNo
        {
            get { return _yellowArmyNo; }

            set { SetProperty(ref _yellowArmyNo, value); }
        }

        public int RedArmyNo
        {
            get { return _redArmyNo; }

            set { SetProperty(ref _redArmyNo, value); }
        }

        public int BlueArmyNo
        {
            get { return _blueArmyNo; }

            set { SetProperty(ref _blueArmyNo, value); }
        }

        public int BlackArmyNo
        {
            get { return _blackArmyNo; }

            set { SetProperty(ref _blackArmyNo, value); }
        }

        public int PurpleArmyNo
        {
            get { return _purpleArmyNo; }

            set { SetProperty(ref _purpleArmyNo, value); }
        }

        public int GreenArmyNo
        {
            get { return _greenArmyNo; }

            set { SetProperty(ref _greenArmyNo, value); }
        }

        public int GaussianBlurSize
        {
            get { return _gaussianBlurSize; }

            set
            {
                SetProperty(ref _gaussianBlurSize, value, SendSettings);
            }
        }


        private ImageProcessing imageProcessing { get; set; }

        private Results results { get; set; }

        public WriteableBitmap WriteableBitmapRESULT
        {
            get { return _writableBitmapRESULT; }

            set { SetProperty(ref _writableBitmapRESULT, value); }
        }

        public WriteableBitmap WriteableBitmapCOLOR
        {
            get { return _writableBitmapCOLOR; }

            set { SetProperty(ref _writableBitmapCOLOR, value); }
        }

        #endregion

        #region Methods

        private void OnProcessedFramesUpdate(object sender, ProcessedFrames processedFrames)
        {
            try
            {
                var bmp = processedFrames.BlackPlayer.ToWriteableBitmap();
                switch (SelectedFrame)
                {
                    case "YellowPlayer":
                        bmp = processedFrames.YellowPlayer.ToWriteableBitmap();
                        break;
                    case "BlackPlayer":
                        bmp = processedFrames.BlackPlayer.ToWriteableBitmap();
                        break;
                    case "BluePlayer":
                        bmp = processedFrames.BluePlayer.ToWriteableBitmap();
                        break;
                    case "PurplePlayer":
                        bmp = processedFrames.PurplePlayer.ToWriteableBitmap();
                        break;
                    case "RedPlayer":
                        bmp = processedFrames.RedPlayer.ToWriteableBitmap();
                        break;
                    case "GreenNeutral":
                        bmp = processedFrames.GreenNeutral.ToWriteableBitmap();
                        break;
                }
                bmp.Freeze();
                WriteableBitmapCOLOR = bmp;


                bmp = processedFrames.GameResult.ToWriteableBitmap();
                bmp.Freeze();
                WriteableBitmapRESULT = bmp;

                YellowArmyNo = results.YellowArmyNo;
                BlueArmyNo = results.BlueArmyNo;
                BlackArmyNo = results.BlackArmyNo;
                RedArmyNo = results.RedArmyNo;
                PurpleArmyNo = results.PurpleArmyNo;
                GreenArmyNo = results.GreenArmyNo;
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private void SendSettings()
        {
            try
            { 
            switch (SelectedFrame)
            {
                case "YellowPlayer":
                    imageProcessing.YellowActLimits.ValMin = ValMin;
                    imageProcessing.YellowActLimits.ValMax = ValMax;
                    imageProcessing.YellowActLimits.HueMin = HueMin;
                    imageProcessing.YellowActLimits.HueMax = HueMax;
                    imageProcessing.YellowActLimits.SatMin = SatMin;
                    imageProcessing.YellowActLimits.SatMax = SatMax;
                    break;
                case "BlackPlayer":
                    imageProcessing.BlackActLimits.ValMin = ValMin;
                    imageProcessing.BlackActLimits.ValMax = ValMax;
                    imageProcessing.BlackActLimits.HueMin = HueMin;
                    imageProcessing.BlackActLimits.HueMax = HueMax;
                    imageProcessing.BlackActLimits.SatMin = SatMin;
                    imageProcessing.BlackActLimits.SatMax = SatMax;
                    break;
                case "BluePlayer":
                    imageProcessing.BlueActLimits.ValMin = ValMin;
                    imageProcessing.BlueActLimits.ValMax = ValMax;
                    imageProcessing.BlueActLimits.HueMin = HueMin;
                    imageProcessing.BlueActLimits.HueMax = HueMax;
                    imageProcessing.BlueActLimits.SatMin = SatMin;
                    imageProcessing.BlueActLimits.SatMax = SatMax;
                    break;
                case "PurplePlayer":
                    imageProcessing.PurpleActLimits.ValMin = ValMin;
                    imageProcessing.PurpleActLimits.ValMax = ValMax;
                    imageProcessing.PurpleActLimits.HueMin = HueMin;
                    imageProcessing.PurpleActLimits.HueMax = HueMax;
                    imageProcessing.PurpleActLimits.SatMin = SatMin;
                    imageProcessing.PurpleActLimits.SatMax = SatMax;
                    break;
                case "RedPlayer":
                    imageProcessing.RedActLimits.ValMin = ValMin;
                    imageProcessing.RedActLimits.ValMax = ValMax;
                    imageProcessing.RedActLimits.HueMax = HueMin;
                    imageProcessing.RedActLimits.HueMin = HueMax;
                    imageProcessing.RedActLimits.SatMin = SatMin;
                    imageProcessing.RedActLimits.SatMax = SatMax;
                    break;
                case "GreenNeutral":
                    imageProcessing.GreenActLimits.ValMin = ValMin;
                    imageProcessing.GreenActLimits.ValMax = ValMax;
                    imageProcessing.GreenActLimits.HueMin = HueMin;
                    imageProcessing.GreenActLimits.HueMax = HueMax;
                    imageProcessing.GreenActLimits.SatMin = SatMin;
                    imageProcessing.GreenActLimits.SatMax = SatMax;
                    break;
            }

            imageProcessing.GaussianBlurSize = GaussianBlurSize;
            FiltersSettings = Settings;
            imageProcessing.FiltersSettings = Settings;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private void SaveSettings()
        {
            try
            {
                switch (SelectedFrame)
                {
                    case "YellowPlayer":
                        Settings.Yellow.ValMin = ValMin;
                        Settings.Yellow.ValMax = ValMax;
                        Settings.Yellow.HueMin = HueMin;
                        Settings.Yellow.HueMax = HueMax;
                        Settings.Yellow.SatMin = SatMin;
                        Settings.Yellow.SatMax = SatMax;
                        break;
                    case "BlackPlayer":
                        Settings.Black.ValMin = ValMin;
                        Settings.Black.ValMax = ValMax;
                        Settings.Black.HueMin = HueMin;
                        Settings.Black.HueMax = HueMax;
                        Settings.Black.SatMin = SatMin;
                        Settings.Black.SatMax = SatMax;
                        break;
                    case "BluePlayer":
                        Settings.Blue.ValMin = ValMin;
                        Settings.Blue.ValMax = ValMax;
                        Settings.Blue.HueMin = HueMin;
                        Settings.Blue.HueMax = HueMax;
                        Settings.Blue.SatMin = SatMin;
                        Settings.Blue.SatMax = SatMax;
                        break;
                    case "PurplePlayer":
                        Settings.Purple.ValMin = ValMin;
                        Settings.Purple.ValMax = ValMax;
                        Settings.Purple.HueMin = HueMin;
                        Settings.Purple.HueMax = HueMax;
                        Settings.Purple.SatMin = SatMin;
                        Settings.Purple.SatMax = SatMax;
                        break;
                    case "RedPlayer":
                        Settings.Red.ValMin = ValMin;
                        Settings.Red.ValMax = ValMax;
                        Settings.Red.HueMax = HueMin;
                        Settings.Red.HueMin = HueMax;
                        Settings.Red.SatMin = SatMin;
                        Settings.Red.SatMax = SatMax;
                        break;
                    case "GreenNeutral":
                        Settings.Green.ValMin = ValMin;
                        Settings.Green.ValMax = ValMax;
                        Settings.Green.HueMin = HueMin;
                        Settings.Green.HueMax = HueMax;
                        Settings.Green.SatMin = SatMin;
                        Settings.Green.SatMax = SatMax;
                        break;
                }
                Settings.GaussianBlurSize = GaussianBlurSize;
                FiltersSettings.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private void LoadSettings()
        {
            try
            {
                FiltersSettings.Load();
                Settings = FiltersSettings;
                imageProcessing.FiltersSettings = FiltersSettings;
                GaussianBlurSize = Settings.GaussianBlurSize;
                switch (SelectedFrame)
                {
                    case "YellowPlayer":
                        ValMin = Settings.Yellow.ValMin;
                        ValMax=Settings.Yellow.ValMax ;
                        HueMin=Settings.Yellow.HueMin ;
                        HueMax=Settings.Yellow.HueMax  ;
                        SatMin=Settings.Yellow.SatMin  ;
                        SatMax=Settings.Yellow.SatMax ;
                        break;
                    case "BlackPlayer":
                        ValMin = Settings.Black.ValMin;
                        ValMax = Settings.Black.ValMax;
                        HueMin = Settings.Black.HueMin;
                        HueMax = Settings.Black.HueMax;
                        SatMin = Settings.Black.SatMin;
                        SatMax = Settings.Black.SatMax;
                        break;
                    case "BluePlayer":
                        ValMin = Settings.Blue.ValMin;
                        ValMax = Settings.Blue.ValMax;
                        HueMin = Settings.Blue.HueMin;
                        HueMax = Settings.Blue.HueMax;
                        SatMin = Settings.Blue.SatMin;
                        SatMax = Settings.Blue.SatMax;
                        break;
                    case "PurplePlayer":
                        ValMin = Settings.Purple.ValMin;
                        ValMax = Settings.Purple.ValMax;
                        HueMin = Settings.Purple.HueMin;
                        HueMax = Settings.Purple.HueMax;
                        SatMin = Settings.Purple.SatMin;
                        SatMax = Settings.Purple.SatMax;
                        break;
                    case "RedPlayer":
                        ValMin = Settings.Red.ValMin;
                        ValMax = Settings.Red.ValMax;
                        HueMax = Settings.Red.HueMin;
                        HueMin = Settings.Red.HueMax;
                        SatMin = Settings.Red.SatMin;
                        SatMax = Settings.Red.SatMax;
                        break;
                    case "GreenNeutral":
                        ValMin = Settings.Green.ValMin;
                        ValMax = Settings.Green.ValMax;
                        HueMin = Settings.Green.HueMin;
                        HueMax = Settings.Green.HueMax;
                        SatMin = Settings.Green.SatMin;
                        SatMax = Settings.Green.SatMax;
                        break;
                }
                SendSettings();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        #endregion
    }
}
