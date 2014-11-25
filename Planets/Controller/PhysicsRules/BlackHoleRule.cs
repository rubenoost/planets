using System.Linq;
using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class BlackHoleRule : AbstractGameRule
    {
        private double JoelConstante = 1.0;

        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Update speed to black hole
            foreach(GameObject g in pf.GameObjects){
                if (g is BlackHole){
                    foreach (GameObject g2 in pf.GameObjects.Where(p => p.Traits.HasFlag(Rule.AFFECTED_BY_BH)))
                    {
                        if (g != g2 && !(g2 is Player))
                        {
                            Vector V = g.Location - g2.Location;
                            double Fg = JoelConstante*((g2.mass*g.mass)/(V.Length()*V.Length()));
                            g2.DV += V.ScaleToLength(Fg*(ms/1000.0));
                        }
                    }
                }
            }
        }
    }
}
