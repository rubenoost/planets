using System;
using Planets.Controller.GameRules.Abstract;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules.Impl
{
    class StasisRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Update speed to black hole
            pf.GameObjects.Iterate(g =>
            {
                if (!(g is Stasis)) return;
                pf.GameObjects.Iterate(g2 =>
                {
                    if (!g2.Is(Rule.Slowable)) return;

                    if (g != g2)
                    {
                        if (g.IntersectsWith(g2))
                        {
                            // Speed of projectile gets updated
                            g2.Dv *= Math.Pow(0.55, ms / 1000);
                            if (g2.Dv.X <= 0 && g2.Dv.X >= -15)
                            {
                                g2.Dv = new Vector(-15, g2.Dv.Y);
                            }
                            else if (g2.Dv.X >= 0 && g2.Dv.X <= 15)
                            {
                                g2.Dv = new Vector(15, g2.Dv.Y);
                            }

                            if (g2.Dv.Y <= 0 && g2.Dv.Y >= -15)
                            {
                                g2.Dv = new Vector(g2.Dv.X, -15);
                            }
                            else if (g2.Dv.Y >= 0 && g2.Dv.Y <= 15)
                            {
                                g2.Dv = new Vector(g2.Dv.X, 15);
                            }
                        }
                    }
                });
            });
        }
    }
}
