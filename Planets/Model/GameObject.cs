using System.Drawing;

namespace Planets.Model
{
    public abstract class GameObject
    {

        protected Vector Location;

        protected Vector DV;

        protected double Mass;

        public GameObject(double x, double y, double dx, double dy, double mass) : this(new Vector(x, y), new Vector(dx, dy), mass)
        {

        }

        public GameObject(Vector location, Vector velocity, double mass)
        {
            Location = location;
            DV = velocity;
            Mass = mass;
        }

        public abstract void Draw(Graphics g);

    }
}
