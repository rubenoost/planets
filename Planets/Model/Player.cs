using System;
using System.Collections.Generic;
using System.Drawing;
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

        public override void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Blue), (float)x, (float)y, Utils.CalcRadius(mass), Utils.CalcRadius(mass));
        }

    }
}
