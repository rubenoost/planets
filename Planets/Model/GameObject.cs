using System.Drawing;

namespace Planets.Model
{
    public class GameObject
    {

        public Vector Location;

        public Vector DV;

        public double mass;

        public double radius {
            get
            {
                return System.Math.Sqrt(mass);
            } 
        }

        public GameObject(double x, double y, double dx, double dy, double mass) : this(new Vector(x, y), new Vector(dx, dy), mass)
        {

        }

        public GameObject(Vector location, Vector velocity, double Mass)
        {
            Location = location;
            DV = velocity;
            this.mass = Mass;
        }

        public void InvertObjectX()
        {
            this.DV = new Vector(this.DV.X * -1, this.DV.Y);
        }

        public void InvertObjectY()
        {
            this.DV = new Vector(this.DV.X, this.DV.Y * -1);
        }

        public void UpdateLocation()
        {
            this.Location += this.DV / 4000;
        }

        public Vector CalcNewLocation()
        {
            return this.Location + this.DV / 4000;
        }
    }
}
