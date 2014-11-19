using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets
{
    class Utils
    {
        enum Direction
        {
            up,
            down,
            left,
            right
        }

        static class Utils
        {

            public static int StartMass = 300;

            public static float CalcRadius(double mass)
            {
                double R = Math.Sqrt(mass); // R = Wortel van M
                return (float)(R * 2);
            }
        }
    }
}
