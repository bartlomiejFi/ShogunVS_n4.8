using OpenCvSharp;

namespace ShogunVS.Models
{
    public class ProcessedFrames
    {
        public Mat GameRegion = new Mat();
        public Mat GameResult = new Mat();
        public Mat YellowPlayer = new Mat();
        public Mat RedPlayer = new Mat();
        public Mat BluePlayer = new Mat();
        public Mat PurplePlayer = new Mat();
        public Mat BlackPlayer = new Mat();
        public Mat GreenNeutral = new Mat();

        public Mat YellowMask = new Mat();
        public Mat RedMask = new Mat();
        public Mat BlueMask = new Mat();
        public Mat PurpleMask = new Mat();
        public Mat BlackMask = new Mat();
        public Mat GreenMask = new Mat();

        public Mat MasksSummary = new Mat();

        public Mat CleanFrame = new Mat();
    }
}
