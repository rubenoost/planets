using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.Model
{
    class Player : GameObject
    {

        public Player(double x, double y, double[] DV, double mass) : base(x, y, DV, mass)
        {
            
        }

    }
}
