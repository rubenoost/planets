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
                return 5 * System.Math.Sqrt(mass);
            } 
        }

        public bool IsAffectedByBlackHole {
            get;
            private set;
        }

        public GameObject(double x, double y, double dx, double dy, double mass, bool blackhole) : this(new Vector(x, y), new Vector(dx, dy), mass, false)
        {

        }

        public GameObject(Vector location, Vector velocity, double Mass, bool blackhole)
        {
            Location = location;
            DV = velocity;
            this.mass = Mass;
            IsAffectedByBlackHole = blackhole;
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
