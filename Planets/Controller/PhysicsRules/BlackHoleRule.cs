using System.Collections.Generic;
using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class BlackHoleRule : AbstractGameRule
    {
        private double JoelConstante = 0.1;

        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Check for collisions with black hole

            var removed = new List<GameObject>();

            foreach (GameObject go in pf.GameObjects)
            {
                if (go is BlackHole)
                {
                    //go.Pull(pf.GameObjects);
                    foreach (GameObject go2 in pf.GameObjects)
                    {

                        if (go != go2 && go.IntersectsWith(go2))
                        {
                            if (!(go2 is Player))
                                removed.Add(go2);
                        }
                    }
                }
            }
            removed.ForEach(g => pf.GameObjects.Remove(g));

            // Update speed to black hole
            foreach(GameObject g in pf.GameObjects){
                if (g is BlackHole){
                    foreach (GameObject g2 in pf.GameObjects){
                        Vector V = g2.Location - g.Location;
                        double Fg = JoelConstante * ((g2.mass * g.mass) / (V.Length() * V.Length()));
                        g2.DV += V.ScaleToLength(Fg * (ms / 1000.0));
                    }
                }
            }
        }
    }
}
