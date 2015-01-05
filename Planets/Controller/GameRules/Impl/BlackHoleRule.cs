using Planets.Controller.GameRules.Abstract;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules.Impl
{
    class BlackHoleRule : AbstractGameRule
    {
        // changes the speed
        private readonly double _joelConstante = 10.0;

        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Update speed to black hole
            pf.GameObjects.Iterate(g =>
            {
                if (!(g is BlackHole)) return;
                pf.GameObjects.Iterate(g2 =>
                {
                    if (!g2.Is(Rule.AffectedByBh)) return;

                    if (g != g2 && !(g2 is Player))
                    {
                        Vector v = g.Location - g2.Location;
                        double fg = _joelConstante * (g.Mass / (v.Length() * v.Length()));
                        // Speed of projectile gets updated
                        g2.Dv += v.ScaleToLength(fg * (ms / 1000.0));
                    }
                });
            });
        }
    }
}
