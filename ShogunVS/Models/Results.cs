using System.Windows.Media;

namespace ShogunVS.Models
{
    public class Results
    {
        public Result first = new Result();
        public Result second = new Result();
        public Result third = new Result();
        public Result fourth = new Result();
        public Result fifth = new Result();

        public Result neutral = new Result();

        public int YellowArmyNo;
        public int BlueArmyNo;
        public int RedArmyNo;
        public int BlackArmyNo;
        public int PurpleArmyNo;
        public int GreenArmyNo;
    }

    public class Result
    {
        public int armyNo { get; set; }
        public SolidColorBrush color { get; set; }
    }
}
