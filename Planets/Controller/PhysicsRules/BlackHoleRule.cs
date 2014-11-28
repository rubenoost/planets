using System.Linq;
using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class BlackHoleRule : AbstractGameRule
    {
        // changes the speed
        private double JoelConstante = 10.0;

        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Update speed to black hole
            pf.BOT.Iterate(g =>
            {
                if (g is BlackHole)
                {
                    pf.BOT.Iterate(g2 =>
                    {
                        if (!g2.Traits.HasFlag(Rule.AFFECTED_BY_BH)) return;

                        if (g != g2 && !(g2 is Player))
                        {
                            Vector V = g.Location - g2.Location;
                            double Fg = JoelConstante * (g.mass / (V.Length() * V.Length()));
                            // Speed of projectile gets updated
                            g2.DV += V.ScaleToLength(Fg * (ms / 1000.0));
                        }
                    });
                }
            });
        }
    }
}
