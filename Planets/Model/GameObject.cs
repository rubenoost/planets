using System;
using System.Drawing;

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
        COLLIDES = 64
    }

    public class GameObject
    {
        // Properties

        public event Action<GameObject> Moved;

        public event Action<GameObject> Resized;

        private Vector _propLocation;
        public Vector Location
        {
            get { return _propLocation; }
            set
            {
                _propLocation = value;
                _propBoundingBox = null;
                if (Moved != null) Moved(this);
            }
        }

        public Vector DV { get; set; }

        private Rectangle? _propBoundingBox;
        public Rectangle BoundingBox
        {
            get
            {
                if (_propBoundingBox.HasValue)
                    return _propBoundingBox.Value;
                Rectangle r = new Rectangle((int)(Location.X - Radius), (int)(Location.Y - Radius), (int)Radius * 2, (int)Radius * 2);
                _propBoundingBox = r;
                return r;
            }
        }

        private double _propMass;
        public double Mass
        {
            get { return _propMass; }
            set
            {
                if (Traits.HasFlag(Rule.DYNAMIC_RADIUS))
                {
                    _propRadius = null;
                    _propBoundingBox = null;
                    if (Resized != null) Resized(this);
                }
                _propMass = Math.Max(0.0, value);
            }
        }

        public Rule Traits { get; protected set; }

        private double? _propRadius;
        public double Radius
        {
            get
            {
                if (!_propRadius.HasValue)
                {
                    if (Traits.HasFlag(Rule.DYNAMIC_RADIUS))
                    {
                        _propRadius = Math.Sqrt(Mass / Math.PI);
                    }
                    else
                    {
                        _propRadius = 50;
                        if (Resized != null) Resized(this);
                    }
                }
                return _propRadius.Value;
            }
            set
            {
                if (!Traits.HasFlag(Rule.DYNAMIC_RADIUS))
                {
                    _propRadius = value;
                    _propBoundingBox = null;
                }
            }
        }

        public GameObject(Vector location, Vector velocity, double Mass)
            : this(location, velocity, Mass,
            Rule.AFFECTED_BY_BH | Rule.COLLIDES | Rule.DYNAMIC_RADIUS | Rule.EATABLE | Rule.MOVE | Rule.EATS | Rule.EAT_PLAYER)
        { }

        protected GameObject(Vector location, Vector velocity, double Mass, Rule traits)
        {
            Location = location;
            DV = velocity;
            this.Mass = Mass;
            Traits = traits;
        }

        public void InvertObjectX()
        {
            DV = new Vector(DV.X * -1, DV.Y);
        }

        public void InvertObjectY()
        {
            DV = new Vector(DV.X, DV.Y * -1);
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
