using System.Collections.Generic;

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

        public override void Pull(List<GameObject> lg) {
            for(int i = 0; i < lg.Count; i++ ) {
                GameObject b = lg[i];
                if(b.GetType().Name == "BlackHole" || b.GetType().Name == "Player") {
                    continue;
                } else {
                    if(b.Location.X < Location.X) {
                        //DX omhoog
                        b.DV = new Vector(10, b.DV.Y);
                    } else if(b.Location.X > Location.X) {
                        b.DV = new Vector(-10, b.DV.Y);
                    } else {
                        b.DV = new Vector(0, b.DV.X);
                    }

                    if(b.Location.Y < Location.Y) {
                        b.DV = new Vector(b.DV.X, 10);
                    } else if(b.Location.Y > Location.Y) {
                        b.DV = new Vector(b.DV.X, -10);
                    } else {
                        b.DV = new Vector(b.DV.X, 0);
                    }

                    if(b.Location.X == Location.X && b.Location.Y == Location.Y){
                        
                    }
                }
            }
        }
    }
}
