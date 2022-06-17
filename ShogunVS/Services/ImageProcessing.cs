using OpenCvSharp;
using Prism.Ioc;
using ShogunVS.Models;
using ShogunVS.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ShogunVS.Services
{
    public class ImageProcessing
    {
        #region Fields

        public event EventHandler<ProcessedFrames> OnProcessedFramesUpdate;

        public event EventHandler OnResultsUpdate;

        #endregion

        #region Properties

        private CameraStreaming cameraStreaming { get; set; }

        private ProcessedFrames processedFrames { get; set; }

        private Results results { get; set; }

        public FiltersSettings FiltersSettings { get; set; }

        public ColorLimits YellowActLimits = new ColorLimits();

        public ColorLimits RedActLimits = new ColorLimits();

        public ColorLimits BlueActLimits = new ColorLimits();

        public ColorLimits BlackActLimits = new ColorLimits();

        public ColorLimits PurpleActLimits = new ColorLimits();

        public ColorLimits GreenActLimits = new ColorLimits();

        public int GaussianBlurSize = 5;
        #endregion

        #region Constructors

        public ImageProcessing(IContainerExtension container)
        {
            cameraStreaming = container.Resolve<CameraStreaming>();
            FiltersSettings = container.Resolve<FiltersSettings>();
            results = container.Resolve<Results>();
            cameraStreaming.OnFrameUpdate += OnFrameUpdate;
        }

        #endregion

        #region Methods

        private void OnFrameUpdate(object sender, Mat newFrame)
        {
            try
            {
                processedFrames = new ProcessedFrames();
                ////finding items
                var frameBlur = new Mat();
                var frameYellow = new Mat();
                var frameRed = new Mat();
                var frameBlue = new Mat();
                var frameBlack = new Mat();
                var framePurple = new Mat();
                var frameGreen = new Mat();

                var frameResult = new Mat();
                //GaussianBlur

                //Cv2.GaussianBlur(newFrame, frameBlur, new Size(GaussianBlurSize,GaussianBlurSize), 1);
                //Cv2.MedianBlur(newFrame, frameBlur,1);
                frameBlur = newFrame;
                //color filter

                ColorFilter(YellowActLimits, frameYellow, frameBlur);
                ColorFilter(RedActLimits, frameRed, frameBlur);
                ColorFilter(BlueActLimits, frameBlue, frameBlur);
                ColorFilter(BlackActLimits, frameBlack, frameBlur);
                ColorFilter(PurpleActLimits, framePurple, frameBlur);
                ColorFilter(GreenActLimits, frameGreen, frameBlur);

                //Mat frameGRAY = newFrame.CvtColor(ColorConversionCodes.RGB2GRAY);
                //processedFrames.BlackPlayer = frameGRAY;

                frameResult = newFrame;

                //Mat element = new Mat(3, 3, MatType.CV_8UC1);
                //Cv2.Erode(frameYellow, frameYellow, element);
                //Cv2.Erode(frameRed, frameRed, element);
                //Cv2.Erode(frameBlue, frameBlue, element);
                //Cv2.Erode(frameBlack, frameBlack, element);
                //Cv2.Erode(framePurple, framePurple, element);
                //Cv2.Erode(frameGreen, frameGreen, element);
                //simple blob detector
                //  SimpleBlobFilter(frameYellow, frameResult, frameResult, Scalar.Blue);
                //SimpleBlobFilter(frameRed, frameResult, frameResult, Scalar.Red);
                //SimpleBlobFilter(frameBlue, frameResult, frameResult, Scalar.Blue);
                //SimpleBlobFilter(frameBlack, frameResult, frameResult, Scalar.Black);
                //SimpleBlobFilter(framePurple, frameResult, frameResult, Scalar.Purple);
                //SimpleBlobFilter(frameGreen, frameResult, frameResult, Scalar.Green);

                // frameYellow =  frameYellow.Mul(100);
                //Cv2.Canny(frameYellow, frameYellow, 500, 1000);
                //Cv2.Canny(frameRed, frameRed, 200, 800);
                ContourDetection(frameYellow,frameResult,Scalar.Orange,Scalar.Black,9,out results.YellowArmyNo);
                ContourDetection(frameRed, frameResult, Scalar.DarkRed, Scalar.Black,9, out results.RedArmyNo);
                ContourDetection(frameBlue, frameResult, Scalar.CornflowerBlue, Scalar.Black,9, out results.BlueArmyNo);
                ContourDetection(frameBlack, frameResult, Scalar.DarkGray, Scalar.Purple,9, out results.BlackArmyNo);
                ContourDetection(framePurple, frameResult, Scalar.MediumPurple, Scalar.Black,5, out results.PurpleArmyNo);
                ContourDetection(frameGreen, frameResult, Scalar.LightGreen, Scalar.Black,9, out results.GreenArmyNo);
                
                Dictionary<SolidColorBrush,int> resultsDictionary = new Dictionary<SolidColorBrush, int>();
                resultsDictionary.Add(Brushes.Yellow,results.YellowArmyNo);
                resultsDictionary.Add(Brushes.Red,results.RedArmyNo );
                resultsDictionary.Add(Brushes.Blue,results.BlueArmyNo);
                resultsDictionary.Add(Brushes.Black,results.BlackArmyNo);
                resultsDictionary.Add(Brushes.Purple,results.PurpleArmyNo);
                var ordered = resultsDictionary.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                results.first.armyNo = ordered.ElementAt(0).Value;
                results.first.color = ordered.ElementAt(0).Key;

                results.second.armyNo = ordered.ElementAt(1).Value;
                results.second.color = ordered.ElementAt(1).Key;

                results.third.armyNo = ordered.ElementAt(2).Value;
                results.third.color = ordered.ElementAt(2).Key;

                results.fourth.armyNo = ordered.ElementAt(3).Value;
                results.fourth.color = ordered.ElementAt(3).Key;

                results.fifth.armyNo = ordered.ElementAt(4).Value;
                results.fifth.color = ordered.ElementAt(4).Key;


                processedFrames.YellowPlayer =  frameYellow;
                processedFrames.RedPlayer = frameRed;
                processedFrames.BluePlayer = frameBlue;
                processedFrames.BlackPlayer = frameBlack ;
                processedFrames.PurplePlayer = framePurple;
                processedFrames.GreenNeutral = frameGreen;
                processedFrames.GameRegion = frameResult;
                processedFrames.GameResult = frameResult;
                OnProcessedFramesUpdate?.Invoke(this, processedFrames);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void ColorFilter(ColorLimits colorLimits, Mat processedMat, Mat newFrame)
        {
            var frameHSV = newFrame.CvtColor(ColorConversionCodes.BGR2HSV);
            if(colorLimits.HueMax < colorLimits.HueMin)
            {
                var resultcol = new Mat();
                var resultcol2 = new Mat();
                Cv2.InRange(frameHSV,
                          new Scalar(
                              0, colorLimits.SatMin, colorLimits.ValMin),
                          new Scalar(
                              colorLimits.HueMax, colorLimits.SatMax, colorLimits.ValMax), resultcol);
                Cv2.InRange(frameHSV,
                          new Scalar(
                              colorLimits.HueMin, colorLimits.SatMin, colorLimits.ValMin),
                          new Scalar(
                              179, colorLimits.SatMax, colorLimits.ValMax), resultcol2);
                Cv2.BitwiseOr(resultcol, resultcol2, processedMat);
            }
            else
            Cv2.InRange(frameHSV,
                      new Scalar(
                          colorLimits.HueMin, colorLimits.SatMin, colorLimits.ValMin),
                      new Scalar(
                          colorLimits.HueMax, colorLimits.SatMax, colorLimits.ValMax),processedMat);
        }

        private void ContourDetection(Mat inputFrame, Mat frameResult,Scalar rectColor,Scalar contColor,int erodeSize,out int result)
        {
            var element = new Mat(erodeSize, erodeSize, MatType.CV_8UC1);
            Cv2.Erode(inputFrame, inputFrame, element);
            Point[][] contours;
            HierarchyIndex[] hierarchyIndices;
            Cv2.FindContours(inputFrame, out contours, out hierarchyIndices,
                RetrievalModes.External,
                ContourApproximationModes.ApproxTC89KCOS);
          //  Cv2.DrawContours(frameResult, contours, -1, contColor);
            int temp = 0;
            foreach (Point[] contour in contours)
            {
                Rect rect = Cv2.BoundingRect(contour);
                RotatedRect rotRect = Cv2.MinAreaRect(contour);
                //var approx = Cv2.ApproxPolyDP(contour, 0.01 * Cv2.ArcLength(contour, true), true);
                //if (rect.Height * rect.Width > 20 && rect.Height * rect.Width < 1200)
                if (rotRect.Size.Height > 25 && rotRect.Size.Width > 25 
                    && rotRect.Size.Height < 70 && rotRect.Size.Width < 70)
                {
                    Cv2.Ellipse(frameResult, rotRect, rectColor, 3, LineTypes.AntiAlias);
                    //Cv2.Rectangle(frameResult, rect, rectColor);
                    temp++;
                }
            }
            result = temp;
        }

        private void SimpleBlobFilter(Mat inputFrame, Mat frameResult,Mat newFrame, Scalar keypointsColor)
        {
            var detectorParams = new SimpleBlobDetector.Params
            {
                MinDistBetweenBlobs = 10, // 10 pixels between blobs
                MinRepeatability = 1,

                //MinThreshold = 100,
                //MaxThreshold = 255,
                //ThresholdStep = 5,

                //FilterByArea = false,
                FilterByArea = true,
                MinArea = 30, // 10 pixels squared
                MaxArea = 500,

                FilterByCircularity = false,
                //FilterByCircularity = true,
                //MinCircularity = 0.001f,

              // FilterByConvexity = false,
                FilterByConvexity = true,
                MinConvexity = 0.001f,
                MaxConvexity = 10,

                FilterByInertia = false,
                //FilterByInertia = true,
                //MinInertiaRatio = 0.001f,

                FilterByColor = false
                //FilterByColor = true,
                //BlobColor = 255 // to extract light blobs
            };
            var simpleBlobDetector = SimpleBlobDetector.Create(detectorParams);
            var keyPoints = simpleBlobDetector.Detect(inputFrame);
            foreach (var keyPoint in keyPoints)
            {
                Console.WriteLine("X: {0}, Y: {1}", keyPoint.Pt.X, keyPoint.Pt.Y);
            }

            Cv2.DrawKeypoints(
             image: newFrame,
             keypoints: keyPoints,
             outImage: frameResult,
             color: keypointsColor,
             flags: DrawMatchesFlags.DrawRichKeypoints);
        }

        #endregion
    }
}
