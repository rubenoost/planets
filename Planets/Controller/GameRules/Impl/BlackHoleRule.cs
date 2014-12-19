using Planets.Controller.GameRules.Abstract;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules.Impl
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
                if (!(g is BlackHole)) return;
                pf.BOT.Iterate(g2 =>
                {
                    if (!g2.Is(Rule.AffectedByBh)) return;

                    if (g != g2 && !(g2 is Player))
                    {
                        Vector v = g.Location - g2.Location;
                        double fg = JoelConstante * (g.Mass / (v.Length() * v.Length()));
                        // Speed of projectile gets updated
                        g2.DV += v.ScaleToLength(fg * (ms / 1000.0));
                    }
                });
            });
        }
    }
}
