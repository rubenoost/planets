using Planets.Model;
using System;

namespace Planets.Controller.PhysicsRules
{
    class StasisRule : AbstractGameRule
    {
        private bool IntersectCircle(GameObject StasisField, GameObject SpaceRock) {
            double RockX = SpaceRock.Radius + SpaceRock.Location.X;
            double RockY = SpaceRock.Radius + SpaceRock.Location.Y;
            Vector RockVector = new Vector(RockX, RockY);

            double StasisX = StasisField.Radius + StasisField.Location.X;
            double StasisY = StasisField.Radius + StasisField.Location.Y;
            Vector StasisVector = new Vector(StasisX, StasisY);

            if(StasisVector.Length() - RockVector.Length() < StasisField.Mass){
                return true;
            }
            return false;
        }

        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Update speed to black hole
            pf.BOT.Iterate(g =>
            {
                if (!(g is Stasis)) return;
                pf.BOT.Iterate(g2 =>
                {
                    if (!g2.Traits.HasFlag(Rule.SLOWS)) return;

                    if(g != g2 && !(g2 is Player)) {
                        Vector V = g.Location - g2.Location;
                        double Fg = (g.Mass / (V.Length() * V.Length()));
                        // Speed of projectile gets updated

                        if(IntersectCircle(g, g2)){
                            g2.DV *= Math.Pow(0.5, ms / 1000);
                            if(g2.DV.X <= 0 && g2.DV.X >= -15){
                                g2.DV = new Vector(-15, g2.DV.Y);
                            } else if(g2.DV.X >= 0 && g2.DV.X <= 15) {
                                g2.DV = new Vector(15, g2.DV.Y);
                            }

                            if(g2.DV.Y <= 0 && g2.DV.Y >= -15) {
                                g2.DV = new Vector(g2.DV.X, -15);
                            } else if(g2.DV.Y >= 0 && g2.DV.Y <= 15) {
                                g2.DV = new Vector(g2.DV.X, 15);
                            }
                        }
                    }
                });
            });
        }
    }
}
