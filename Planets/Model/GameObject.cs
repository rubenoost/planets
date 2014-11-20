using System;
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

        public void UpdateLocation(double ms)
        {
            Location = CalcNewLocation(ms);
        }

        public Vector CalcNewLocation(double ms)
        {
            return Location + DV * ms / 1000.0f;
        }

        public bool IntersectsWith(GameObject go)
        {
            if (!DoLinesOverlap(Location.X, radius*2, go.Location.X, go.radius*2) &&
                !DoLinesOverlap(Location.Y, radius*2, go.Location.Y, go.radius*2))
            {
                return false;
                
            }
            return (Location - go.Location).Length() < (radius + go.radius);
            
        }

        public static bool DoLinesOverlap(double x1, double width1, double x2, double width2)
        {
            if (x1 >= x2)
                return (x2 + width2) > x1;
            return (x1 + width1) > x2;
        }
    }
}
