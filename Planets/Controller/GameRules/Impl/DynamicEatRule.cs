using System;
using Planets.Controller.GameRules.Abstract;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules.Impl
{
    class DynamicEatRule : AbstractCollisionRule
    {
        protected override void DoCollision(Playfield pf, ScoreBoard sb, GameObject go1, GameObject go2)
        {
            if (!go1.Is(Rule.Eatable) && !go2.Is(Rule.Eatable)) return;
            if (!go1.Is(Rule.DynamicRadius) && !go2.Is(Rule.DynamicRadius)) return;
            if (go1 is BlackHole || go2 is BlackHole) return;

            // Check distance
            double l = (go1.Location - go2.Location).Length();

            // Check for distance too large
            if (go1.Radius + go2.Radius <= l) return;

            // Determine largest and smallest
            GameObject gL, gS;
            if (go1.Radius > go2.Radius)
            {
                gL = go1;
                gS = go2;
            }
            else
            {
                gL = go2;
                gS = go1;
            }

            if (gS is AntiMatter && !(gL is AntiMatter) && gL.Is(Rule.Eatable))
            {
                // gL moet zoveel kleiner worden als dat gS is. Hierna moet er worden gecheckt of
                double lostMass = gS.Mass * 30;

                // Check for mass of large gameobject
                if (lostMass >= gL.Mass)
                {
                    if (gL != pf.CurrentPlayer)
                        pf.GameObjects.Remove(gL);
                }
                else
                    gL.Mass -= lostMass;

                if (gL is Player)
                {
                    pf.ScoreBoard.AddScore(new Score(-50, DateTime.Now, gS.Location, true));
                }


                // Remove antimatter
                pf.GameObjects.Remove(gS);
            }
            else
            {
                // Check for eat flags
                if (!gS.Is(Rule.Eatable)) return;
                if (!gL.Is(gS is Player ? Rule.EatPlayer : Rule.Eats)) return;

                // Check for too close
                double T = gL.Mass + gS.Mass;
                if (Math.Sqrt(T / Math.PI) > l)
                {
                    gL.Mass = T;
                    gS.Mass = 1.0;

                    // Bereken score? Animeer score!
                    if (!(gS is Player) && (gL is Player))
                    {
                        sb.AddScore(new Score(50, DateTime.Now, gS.Location, (gL == pf.CurrentPlayer)));
                    }

                    pf.GameObjects.Remove(gS);
                    return;
                }

                // Do magic
                double b = T;
                double temp1 = Math.PI * l * l - T;

                double c = temp1 * temp1;
                double d = Math.Sqrt(b * b - c);

                double glM = (b + d) / 2;

                // Set new velocity
                gL.Dv = (gL.Dv * gL.Mass + gS.Dv * (glM - gL.Mass)) / glM;

                // Set new masses
                gL.Mass = glM;
                gS.Mass = (b - d) / 2;
            }
        }
    }
}
