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
                    if (!g2.Is(Rule.AFFECTED_BY_AG)) return;

                    if (g != g2)
                    {
                        Vector V = g.Location - g2.Location;
                        double L = V.Length() - g.Radius + 1.0;
                        double Fg = BartConstante * (g.Mass / (L * L));
                        // Speed of projectile gets updated
                        g2.DV -= V.ScaleToLength(Fg * (ms / 1000.0));
                    }
                });
            });
        }
    }
}
