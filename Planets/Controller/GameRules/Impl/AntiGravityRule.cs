using Planets.Controller.GameRules.Abstract;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules.Impl
{
    class AntiGravityRule : AbstractGameRule
    {
        // changes the speed
        private readonly double _bartConstante = 50.0;

        protected override void ExecuteRule(Playfield pf, double ms)
        {
            foreach (var g in pf.GameObjects[typeof(Antigravity)])
            {
                pf.GameObjects.Iterate(g2 =>
                {
                    if (!g2.Is(Rule.AffectedByAg)) return;

                    if (g != g2)
                    {
                        Vector v = g.Location - g2.Location;
                        double l = v.Length() - g.Radius + 1.0;
                        double fg = _bartConstante * (g.Mass / (l * l));
                        // Speed of projectile gets updated
                        g2.Dv -= v.ScaleToLength(fg * (ms / 1000.0));
                    }
                });
            }
        }
    }
}
