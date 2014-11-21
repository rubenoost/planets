using System.Collections.Generic;
using System.ComponentModel;

namespace Planets.Model
{
    class BlackHole : GameObject
    {
        private double Strength;

        public BlackHole(double x, double y, double dx, double dy, double mass, double strength) : base(x, y, dx, dy, mass, false)
        {
            Strength = strength;
        }

        public BlackHole(Vector location, Vector velocity,  double mass, double strength) : base(location, velocity, mass, false)
        {
            Strength = strength;
        }

        public double Gravity() {
            double GravGrootte = Strength + mass;
            return GravGrootte;
        }

        public override bool Pull(List<GameObject> lg) {
            for(int i = 0; i < lg.Count; i++ ) {
                GameObject b = lg[i];
                if(b is BlackHole || b is Player) {
                } else {
                    if(b.Location.X < Location.X) {
                        //DX omhoog
                        b.DV = new Vector(10, b.DV.Y);
                    } else if(b.Location.X > Location.X) {
                        b.DV = new Vector(-10, b.DV.Y);
                    } else {
                        b.DV = new Vector(0, b.DV.Y);
                    }

                    if(b.Location.Y < Location.Y) {
                        b.DV = new Vector(b.DV.X, 10);
                    } else if(b.Location.Y > Location.Y) {
                        b.DV = new Vector(b.DV.X, -10);
                    } else {
                        b.DV = new Vector(b.DV.X, 0);
                    }

                    if(IntersectsWith(b)){
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
