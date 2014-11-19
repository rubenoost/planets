using System.Drawing;

namespace Planets.Model
{
    class Player : GameObject
    {
        public Player(double x, double y, double dx, double dy, double mass) : base(x, y, dx, dy, mass)
        {
        }

        public Player(Vector location, Vector velocity, double mass) : base(location, velocity, mass)
        {
        }

        public override void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Blue), (float)Location.X, (float)Location.Y, Utils.CalcRadius(Mass), Utils.CalcRadius(Mass));
        }

    }
}
