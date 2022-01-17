using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingModel
{
    public class Point
    {
        public Point(double xIndex, double yIndex)
        {
            X = xIndex;
            Y = yIndex;
        }

        // X
        public double X
        {
            get;
            set;
        }

        // Y
        public double Y
        {
            get;
            set;
        }
    }
}
