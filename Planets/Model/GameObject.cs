using System.Collections.Generic;

namespace Planets.Model
{
    public class GameObject
    {
        // Flags

        public bool CanMove = true;

        public bool EatsOthers = false;

        public bool CanGrow = false;

        public bool EatsPlayer = false;

        public bool HasDynamicRadius = true;

        public bool IsAffectedByBlackHole = true;

        // Properties

        public Vector Location;

        public Vector DV;

        public double mass;

        private double? _propRadius;
        public double Radius {
            get
            {
                if(HasDynamicRadius)
                    return 5 * System.Math.Sqrt(mass);
                return _propRadius.HasValue ? _propRadius.Value : 0.0;
            }
            set
            {
                if (!HasDynamicRadius)
                    _propRadius = value;
            }
        }

        public GameObject(double x, double y, double dx, double dy, double mass) : this(new Vector(x, y), new Vector(dx, dy), mass)
        {

        }

        public GameObject(Vector location, Vector velocity, double Mass)
        {
            Location = location;
            DV = velocity;
            mass = Mass;
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
            if (!DoLinesOverlap(Location.X, Radius*2, go.Location.X, go.Radius*2) &&
                !DoLinesOverlap(Location.Y, Radius*2, go.Location.Y, go.Radius*2))
        {
                return false;
                
            }
            return (Location - go.Location).Length() <= (Radius + go.Radius);
            
        }

        public static bool DoLinesOverlap(double x1, double width1, double x2, double width2)
        {
            if (x1 >= x2)
                return (x2 + width2) > x1;
            return (x1 + width1) > x2;
        }
    }
}
