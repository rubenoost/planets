using System.Drawing;

namespace Planets.Model
{
    class Player : GameObject
    {

        public Player(double x, double y, double[] DV, double mass) : base(x, y, DV, mass)
        {
            
        }

        public override void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Blue), (float)location.X, (float)location.Y, Utils.CalcRadius(mass), Utils.CalcRadius(mass));
        }

    }
}
