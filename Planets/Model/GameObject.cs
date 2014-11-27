using System;

namespace Planets.Model
{
    [Flags]
    public enum Rule
    {
        MOVE = 1,
        EATABLE = 2,
        EATS = 4,
        EAT_PLAYER = 8,
        DYNAMIC_RADIUS = 16,
        AFFECTED_BY_BH = 32,
        COLLIDES = 64,
    }

    public class GameObject
    {
        // Properties

        public event Action<GameObject> Moved;

        private Vector _propLocation;

        public Vector Location
        {
            get { return _propLocation; }
            set
            {
                _propLocation = value;
                if(Moved != null) Moved(this);
            }
        }

        private Vector _propDV;
        public Vector DV
        {
            get { return _propDV; }
            set
            {
                _propDV = value;
                if(Double.IsNaN(DV.X) || Double.IsNaN(DV.Y))
                    throw new Exception("NAN!");
            }
        }

        public double mass;

        public Rule Traits { get; protected set; }

        private double? _propRadius;
        public double Radius
        {
            get
            {
                if (Traits.HasFlag(Rule.DYNAMIC_RADIUS))
                    return 5 * System.Math.Sqrt(mass);
                return _propRadius.HasValue ? _propRadius.Value : 0.0;
            }
            set
            {
                if (!Traits.HasFlag(Rule.DYNAMIC_RADIUS))
                    _propRadius = value;
            }
        }

        public GameObject(Vector location, Vector velocity, double Mass)
            : this(location, velocity, Mass,
            Rule.AFFECTED_BY_BH | Rule.COLLIDES | Rule.DYNAMIC_RADIUS | Rule.EATABLE | Rule.MOVE)
        { }

        protected GameObject(Vector location, Vector velocity, double Mass, Rule traits)
        {
            Location = location;
            DV = velocity;
            mass = Mass;
            Traits = traits;
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
            if (!DoLinesOverlap(Location.X, Radius * 2, go.Location.X, go.Radius * 2) &&
                !DoLinesOverlap(Location.Y, Radius * 2, go.Location.Y, go.Radius * 2))
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
