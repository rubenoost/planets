using System.Collections.Generic;
using System.ComponentModel;

namespace Planets.Model
{
    class BlackHole : GameObject
    {
        private double Strength;

        public BlackHole(double x, double y, double dx, double dy, double mass, double strength)
            : this(new Vector(x, y), new Vector(dx, dy), mass, strength)
        { }

        public BlackHole(Vector location, Vector velocity, double mass, double strength)
            : base(location, velocity, mass)
        {
            Strength = strength;
            IsAffectedByBlackHole = false;
            CanGrow = false;
            CanMove = false;
            HasDynamicRadius = false;
            Radius = 50;
        }

        public double Gravity()
        {
            double GravGrootte = Strength + mass;
            return GravGrootte;
        }
    }
}
