using OpenCvSharp;
using ShogunVS.Models;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ShogunVS.Services
{
    public sealed class CameraStreaming : IDisposable
    {
        #region Fields

        private Task _streamTask;
        private CancellationTokenSource _cancellationTokenSource;
        public event EventHandler<Mat> OnFrameUpdate;

        #endregion

        #region Properties

        public CameraDevice CameraDevice { get; set; }
        public int CameraDeviceID { get; set; }

        #endregion

        #region Constructors

        public CameraStreaming()
        {

        }

        #endregion

        #region Methods

        public async Task StartStreaming(int deviceID)
        {
            if (_streamTask != null && !_streamTask.IsCompleted)
                return;

            _cancellationTokenSource = new CancellationTokenSource();
            _streamTask = Task.Run(async () =>
            {
                try
                {
                    var videoCapture = new VideoCapture();

                    if (!videoCapture.Open(deviceID))
                        throw new ApplicationException("Cannot connect to camera");

                    using (var frame = new Mat())
                    {
                        while(!_cancellationTokenSource.IsCancellationRequested)
                        {
                            //     videoCapture.Set(VideoCaptureProperties.Brightness,20);
                            //      videoCapture.Set(VideoCaptureProperties.Contrast,0);
                            //videoCapture.Set(VideoCaptureProperties.FrameHeight, 720);
                            //videoCapture.Set(VideoCaptureProperties.FrameWidth, 1280);
                            //videoCapture.Set(VideoCaptureProperties.Fps, 30);
                            //var a = videoCapture.Brightness;
                            //var c = videoCapture.Contrast;
                            //var he = videoCapture.FrameHeight;
                            //var wi = videoCapture.FrameWidth;
                            //var bu = videoCapture.BufferSize;
                            //var fps = videoCapture.Fps;
                            videoCapture.Read(frame);
                            //Cv2.Resize(frame, frame, new OpenCvSharp.Size(600, 400));
                            if (!frame.Empty())
                            {                              
                                OnFrameUpdate?.Invoke(this,frame);
                            }
                            await Task.Delay(33);
                        }
                    }
                    videoCapture?.Dispose();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }, _cancellationTokenSource.Token);

            if (_streamTask.IsFaulted)
                await _streamTask;
        }

        public async Task StopStreaming()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
                return;

            if(!_streamTask.IsCompleted)
            {
                _cancellationTokenSource.Cancel();
                await _streamTask;
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
        }

        #endregion
    }
}