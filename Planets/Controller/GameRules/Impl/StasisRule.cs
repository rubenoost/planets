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
            pf.BOT.Iterate(g =>
            {
                if (!(g is Stasis)) return;
                pf.BOT.Iterate(g2 =>
                {
                    if (!g2.Is(Rule.SLOWABLE)) return;

                    if (g != g2)
                    {
                        if (g.IntersectsWith(g2))
                        {
                            // Speed of projectile gets updated
                            g2.DV *= Math.Pow(0.55, ms / 1000);
                            if (g2.DV.X <= 0 && g2.DV.X >= -15)
                            {
                                g2.DV = new Vector(-15, g2.DV.Y);
                            }
                            else if (g2.DV.X >= 0 && g2.DV.X <= 15)
                            {
                                g2.DV = new Vector(15, g2.DV.Y);
                            }

                            if (g2.DV.Y <= 0 && g2.DV.Y >= -15)
                            {
                                g2.DV = new Vector(g2.DV.X, -15);
                            }
                            else if (g2.DV.Y >= 0 && g2.DV.Y <= 15)
                            {
                                g2.DV = new Vector(g2.DV.X, 15);
                            }
                        }
                    }
                });
            });
        }
    }
}
