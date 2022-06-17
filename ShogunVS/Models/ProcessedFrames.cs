using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShogunVS.Models
{
    public class ProcessedFrames
    { 
        public Mat GameRegion=new Mat();
        public Mat GameResult = new Mat();
        public Mat YellowPlayer = new Mat();
        public Mat RedPlayer = new Mat();
        public Mat BluePlayer = new Mat();
        public Mat PurplePlayer = new Mat();
        public Mat BlackPlayer = new Mat();
        public Mat GreenNeutral = new Mat();
    }
}
