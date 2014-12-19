using System;
using System.Drawing;
using System.Xml.Serialization;

namespace Planets.Model.GameObjects
{
    [Flags]
    public enum Rule
    {
        None = 0,
        Move = 1,
        Eatable = 2,
        Eats = 4,
        EatPlayer = 8,
        DynamicRadius = 16,
        AffectedByBh = 32,
        Collides = 64,
        Slowable = 128,
        Explodes = 256,
        AffectedByAg = 512,
        HasScore = 1024
    }

    [XmlInclude(typeof(BlackHole))]
    [XmlInclude(typeof(Antagonist))]
    [XmlInclude(typeof(Antigravity))]
    [XmlInclude(typeof(AntiMatter))]
    [XmlInclude(typeof(Mine))]
    [XmlInclude(typeof(Stasis))]
    [XmlInclude(typeof(Bonus))]
    public class GameObject
    {
        // Properties

        public event Action<GameObject> Moved;

        public event Action<GameObject> Resized;

        private Vector _propLocation;

        // So the Antagonist doesn't follow it's own projectiles;
        public bool Ai;
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
                if (Traits.HasFlag(Rule.DynamicRadius))
                {
                    if (value <= 0)
                        throw new SystemException("You can't set mass lower then zero, you freaking moron, you are the cause of the instability caused in this universe!!!!");
                    _propRadius = null;
                    _propBoundingBox = null;
                    if (Resized != null) Resized(this);
                }
                _propMass = Math.Max(0.0, value);
            }
        }

        protected Rule Traits;

        private double? _propRadius;
        public double Radius
        {
            get
            {
                if (!_propRadius.HasValue)
                {
                    if (Traits.HasFlag(Rule.DynamicRadius))
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
                if (!Traits.HasFlag(Rule.DynamicRadius))
                {
                    _propRadius = value;
                    _propBoundingBox = null;
                }
            }
        }

        // ReSharper disable once UnusedMember.Global
        public GameObject()
            : this(new Vector(), new Vector(), Utils.StartMass)
        { }

        public GameObject(Vector location, Vector velocity, double mass)
            : this(location, velocity, mass,
            Rule.AffectedByBh | Rule.Collides | Rule.DynamicRadius | Rule.Eatable | Rule.Move | Rule.Eats | Rule.Slowable | Rule.AffectedByAg)
        { }

        protected GameObject(Vector location, Vector velocity, double mass, Rule traits)
        {
            Location = location;
            DV = velocity;
            this.Mass = mass;
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
            if (BoundingBox.IntersectsWith(go.BoundingBox))
            {
                return true;

            }
            return (Location - go.Location).Length() <= (Radius + go.Radius);

        }

        public bool Is(Rule r)
        {
            return Traits.HasFlag(r);
        }
    }
}
