using System.Drawing;

namespace Planets.Model
{
    public abstract class GameObject
    {

        protected Vector Location;

        protected Vector DV;

        protected double mass;

        public GameObject(double x, double y, double dx, double dy, double mass) : this(new Vector(x, y), new Vector(dx, dy), mass)
        {

        }

        public GameObject(Vector location, Vector velocity)
        {
            Location = location;
            DV = velocity;
        }

        public abstract void Draw(Graphics g);

    }
}
