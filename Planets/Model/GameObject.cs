using System.Drawing;

namespace Planets.Model
{
    public class GameObject
    {

        public Vector Location;

        public Vector DV;

        public double mass;

        public GameObject(double x, double y, double dx, double dy, double mass) : this(new Vector(x, y), new Vector(dx, dy), mass)
        {

        }

        public GameObject(Vector location, Vector velocity, double Mass)
        {
            Location = location;
            DV = velocity;
            this.mass = Mass;
        }

        public void UpdateLocation()
        {
            this.Location += this.DV / 4000;
        }
    }
}
