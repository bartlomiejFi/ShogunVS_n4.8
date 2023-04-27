﻿using OpenCvSharp;
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

        private YeelightControl yeelightControl { get; set; }
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
            yeelightControl = container.Resolve<YeelightControl>();
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
                var frameMasked = new Mat(newFrame.Size(),newFrame.Type());
                var frameBlured = new Mat(newFrame.Size(), newFrame.Type());
                var frameResult = newFrame;

                var frameYellow = processedFrames.YellowPlayer;
                var frameRed = processedFrames.RedPlayer;
                var frameBlue = processedFrames.BluePlayer;
                var frameBlack = processedFrames.BlackPlayer;
                var framePurple = processedFrames.PurplePlayer;
                var frameGreen = processedFrames.GreenNeutral;



                //GaussianBlur   
                Cv2.GaussianBlur(newFrame, frameBlured, new Size(FiltersSettings.GaussianBlurSize,FiltersSettings.GaussianBlurSize), 1);

                //var frameMask = new Mat(frameBlur.Size(), frameBlur.Type(), Scalar.Black);

                //var frameROI = new Mat(frameBlur, FiltersSettings.ROI);

           

                var frameMaskZeros = new Mat(frameBlured.Size(), MatType.CV_8U, Scalar.Black);//Mat.Zeros(frameBlur.Size(),MatType.CV_8U);
                Cv2.Rectangle(frameMaskZeros, FiltersSettings.ROI, Scalar.White,-1);

               

                Cv2.BitwiseAnd(frameBlured, frameBlured, frameMasked, frameMaskZeros);  
                
               
                //Cv2.MedianBlur(newFrame, frameBlur,1);
                //frameBlur = newFrame;
                //color filter

                ColorFilter(YellowActLimits,processedFrames.YellowPlayer, frameMasked);
                ColorFilter(RedActLimits, processedFrames.RedPlayer, frameMasked);
                ColorFilter(BlueActLimits, processedFrames.BluePlayer, frameMasked);
                ColorFilter(BlackActLimits, processedFrames.BlackPlayer, frameMasked);
                ColorFilter(PurpleActLimits, processedFrames.PurplePlayer, frameMasked);
                ColorFilter(GreenActLimits, processedFrames.GreenNeutral, frameMasked);


                //Mat frameGRAY = newFrame.CvtColor(ColorConversionCodes.RGB2GRAY);
                //processedFrames.BlackPlayer = frameGRAY;

                //   frameResult = frameMasked;

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
                processedFrames.RedMask = new Mat(frameResult.Size(), MatType.CV_8UC1, Scalar.White);
                processedFrames.YellowMask = new Mat(frameResult.Size(), MatType.CV_8UC1, Scalar.White);
                processedFrames.BlueMask = new Mat(frameResult.Size(), MatType.CV_8UC1, Scalar.White);
                processedFrames.BlackMask = new Mat(frameResult.Size(), MatType.CV_8UC1, Scalar.White);
                processedFrames.PurpleMask = new Mat(frameResult.Size(), MatType.CV_8UC1, Scalar.White);
                processedFrames.GreenMask = new Mat(frameResult.Size(), MatType.CV_8UC1, Scalar.White);
                processedFrames.MasksSummary = new Mat(frameResult.Size(), MatType.CV_8UC1, Scalar.White);

                int erodeGeneral = 6;
                ContourDetection(processedFrames.RedPlayer, frameResult,processedFrames.RedMask, Scalar.Red, Scalar.Black, erodeGeneral, out results.RedArmyNo);

                // SimpleBlobFilter(processedFrames.RedPlayer,frameResult,newFrame,Scalar.LavenderBlush);



                Cv2.BitwiseAnd(processedFrames.MasksSummary, processedFrames.RedMask, processedFrames.MasksSummary);
                Cv2.BitwiseAnd(processedFrames.YellowPlayer, processedFrames.MasksSummary, processedFrames.YellowPlayer);

                ContourDetection(processedFrames.YellowPlayer, frameResult, processedFrames.YellowMask, Scalar.Orange, Scalar.Black, erodeGeneral, out results.YellowArmyNo);

                Cv2.BitwiseAnd(processedFrames.MasksSummary, processedFrames.YellowMask, processedFrames.MasksSummary);
                Cv2.BitwiseAnd(processedFrames.BluePlayer, processedFrames.MasksSummary, processedFrames.BluePlayer);


                ContourDetection(frameBlue, frameResult, processedFrames.BlueMask, Scalar.CornflowerBlue, Scalar.Black, erodeGeneral, out results.BlueArmyNo);

                Cv2.BitwiseAnd(processedFrames.MasksSummary, processedFrames.BlueMask, processedFrames.MasksSummary);
                Cv2.BitwiseAnd(processedFrames.GreenNeutral, processedFrames.MasksSummary, processedFrames.GreenNeutral);

                ContourDetection(frameGreen, frameResult, processedFrames.GreenMask, Scalar.LightGreen, Scalar.Black, erodeGeneral, out results.GreenArmyNo);

                Cv2.BitwiseAnd(processedFrames.MasksSummary, processedFrames.GreenMask, processedFrames.MasksSummary);
                Cv2.BitwiseAnd(processedFrames.PurplePlayer, processedFrames.MasksSummary, processedFrames.PurplePlayer);

                ContourDetection(framePurple, frameResult, processedFrames.PurpleMask, Scalar.MediumPurple, Scalar.Black, erodeGeneral, out results.PurpleArmyNo);

                Cv2.BitwiseAnd(processedFrames.MasksSummary, processedFrames.PurpleMask, processedFrames.MasksSummary);
                Cv2.BitwiseAnd(processedFrames.BlackPlayer, processedFrames.MasksSummary, processedFrames.BlackPlayer);

                ContourDetection(frameBlack, frameResult, processedFrames.BlackMask, Scalar.DarkGray, Scalar.Purple, erodeGeneral, out results.BlackArmyNo);

                var frameMaskTopLine = new Mat(frameBlured.Size(), MatType.CV_8U, Scalar.Black);//Mat.Zeros(frameBlur.Size(),MatType.CV_8U);
                Cv2.Rectangle(frameMaskTopLine, new Rect(0, 0, frameMaskTopLine.Width, 2), Scalar.White, -1);

                var frameMaskBottomLine = new Mat(frameBlured.Size(), MatType.CV_8U, Scalar.Black);//Mat.Zeros(frameBlur.Size(),MatType.CV_8U);
                Cv2.Rectangle(frameMaskBottomLine, new Rect(0, 718, frameMaskTopLine.Width, 2), Scalar.White, -1);

                Cv2.Rectangle(frameResult, FiltersSettings.ROI, Scalar.White, 1,LineTypes.AntiAlias);

                var topLineMean = Cv2.Mean(processedFrames.BlackPlayer, frameMaskTopLine);

                if (topLineMean == new Scalar(255)
                    ||
                    Cv2.Mean(processedFrames.RedPlayer, frameMaskTopLine) == new Scalar(255)
                     ||
                    Cv2.Mean(processedFrames.BluePlayer, frameMaskTopLine) == new Scalar(255)
                     ||
                    Cv2.Mean(processedFrames.PurplePlayer, frameMaskTopLine) == new Scalar(255)
                     ||
                    Cv2.Mean(processedFrames.YellowPlayer, frameMaskTopLine) == new Scalar(255)
                     ||
                    Cv2.Mean(processedFrames.GreenNeutral, frameMaskTopLine) == new Scalar(255)
                     ||
                    Cv2.Mean(processedFrames.BlackPlayer, frameMaskBottomLine) == new Scalar(255)
                      ||
                    Cv2.Mean(processedFrames.RedPlayer, frameMaskBottomLine) == new Scalar(255)
                     ||
                    Cv2.Mean(processedFrames.BluePlayer, frameMaskBottomLine) == new Scalar(255)
                     ||
                    Cv2.Mean(processedFrames.PurplePlayer, frameMaskBottomLine) == new Scalar(255)
                     ||
                    Cv2.Mean(processedFrames.YellowPlayer, frameMaskBottomLine) == new Scalar(255)
                     ||
                    Cv2.Mean(processedFrames.GreenNeutral, frameMaskBottomLine) == new Scalar(255))
                    return;

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

                results.neutral.armyNo = results.GreenArmyNo;
                results.neutral.color = Brushes.DarkGreen;


                //processedFrames.YellowPlayer = frameMasked;   
                //processedFrames.RedPlayer = frameMaskZeros;
                //processedFrames.BluePlayer = frameMaskTopLine;
                //processedFrames.BlackPlayer = frameBlack ;
                //processedFrames.PurplePlayer = framePurple;
                //processedFrames.GreenNeutral = frameGreen;
                processedFrames.GameRegion = newFrame;
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

        private void ContourDetection(Mat inputFrame, Mat frameResult,Mat frameMask,Scalar rectColor,Scalar contColor,int erodeSize,out int result)
        {
            var element = new Mat(erodeSize, erodeSize, MatType.CV_8UC1);
            Cv2.Erode(inputFrame, inputFrame, element);
            Point[][] contours;
            HierarchyIndex[] hierarchyIndices;
            Cv2.FindContours(inputFrame, out contours, out hierarchyIndices,
                RetrievalModes.List,
                ContourApproximationModes.ApproxTC89KCOS);
            List<Point[]> maskList;
            maskList = new List<Point[]>();


            var armySizeMin = 8;//18
            var armySizeMax = 30;//65

            // Cv2.DrawContours(frameResult, contours, -1, contColor);
            int index = 0;
            int temp = 0;
            result = temp;
            foreach (Point[] contour in contours)
            {
                if (hierarchyIndices[index].Child != -1)
                    return;
                index++;
                Rect rect = Cv2.BoundingRect(contour);
                RotatedRect rotRect = Cv2.MinAreaRect(contour);
                //var approx = Cv2.ApproxPolyDP(contour, 0.01 * Cv2.ArcLength(contour, true), true);
                //if (rect.Height * rect.Width > 20 && rect.Height * rect.Width < 1200)
                if (rotRect.Size.Height > armySizeMin &&
                    rotRect.Size.Width > armySizeMin &&
                    rotRect.Size.Height < armySizeMax &&
                    rotRect.Size.Width < armySizeMax)
                {
                    Cv2.Ellipse(frameResult, rotRect, rectColor, 2, LineTypes.AntiAlias);
                    //Cv2.Rectangle(frameResult, rect, rectColor);
                    temp++;
                    var pts = rotRect.Points();
                    maskList.Add(contour);

                    Cv2.Ellipse(frameMask, rotRect, Scalar.Black, -1);
                    Cv2.DrawContours(frameMask, maskList, -1, Scalar.Black, -1);
                }
                else if
                    (rotRect.Size.Height < armySizeMax && rotRect.Size.Width > armySizeMax ||
                    rotRect.Size.Height > armySizeMax && rotRect.Size.Width < armySizeMax)
                {
                    Cv2.Ellipse(frameResult, rotRect, rectColor, 2, LineTypes.AntiAlias);
                    var biggerSize = rotRect.Size.Height;
                    if(rotRect.Size.Width>rotRect.Size.Height)
                        biggerSize=rotRect.Size.Width;
                    maskList.Add(contour);
                    Cv2.Ellipse(frameMask, rotRect, Scalar.Black, -1);
                    Cv2.DrawContours(frameMask, maskList, -1, Scalar.Black, -1);
                    var armyAmount = biggerSize / 20;//35
                    var iarmyAmount = (int)Math.Round(armyAmount, MidpointRounding.AwayFromZero);
                    if (iarmyAmount == 1)
                        iarmyAmount++;
                    temp = temp + iarmyAmount;
                }
                else if
                        (rotRect.Size.Height > armySizeMax ||
                        rotRect.Size.Width > armySizeMax)
                {

                    Cv2.Rectangle(frameResult, rect, rectColor, 2, LineTypes.AntiAlias);
                }
            }
            result = temp;
        }

        private void SimpleBlobFilter(Mat inputFrame, Mat frameResult,Mat newFrame, Scalar keypointsColor)
        {
            var detectorParams = new SimpleBlobDetector.Params
            {
                MinDistBetweenBlobs = 1, // 10 pixels between blobs
                MinRepeatability = 1,

                //MinThreshold = 100,
                //MaxThreshold = 255,
                //ThresholdStep = 5,

                //FilterByArea = false,
                FilterByArea = true,
                MinArea = 30, // 10 pixels squared
                MaxArea = 5000,

                FilterByCircularity = false,
                //FilterByCircularity = true,
                //MinCircularity = 0.001f,

               FilterByConvexity = true,
               // FilterByConvexity = ,
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
           Console.WriteLine(keyPoint.Size.ToString());
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
