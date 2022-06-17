using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public int YellowArmyNo;
        public int BlueArmyNo;
        public int RedArmyNo;
        public int BlackArmyNo;
        public int PurpleArmyNo;
        public int GreenArmyNo;


        //public int armyNoo;

        //public Results()
        //{
        //    first.armyNo = 0;
        //    first.color = Brushes.Aqua;

        //    second.armyNo = 0;
        //    second.color = Brushes.Aqua;
        //}
    }

    public class Result
    {
        public int armyNo { get; set; }
        public SolidColorBrush color { get; set; }
    }
}
