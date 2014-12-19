using Planets.Controller.GameRules.Abstract;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules.Impl
{
    class AntiGravityRule : AbstractGameRule
    {
        // changes the speed
        private double BartConstante = 50.0;

        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Update speed to black hole
            pf.BOT.Iterate(g =>
            {
                if (!(g is Antigravity)) return;
                pf.BOT.Iterate(g2 =>
                {
                    if (!g2.Is(Rule.AffectedByAg)) return;

                    if (g != g2)
                    {
                        Vector v = g.Location - g2.Location;
                        double l = v.Length() - g.Radius + 1.0;
                        double fg = BartConstante * (g.Mass / (l * l));
                        // Speed of projectile gets updated
                        g2.DV -= v.ScaleToLength(fg * (ms / 1000.0));
                    }
                });
            });
        }
    }
}
