using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class DynamicEatRule : AbstractCollisionRule
    {
        protected override void DoCollision(Playfield pf, ScoreBoard sb, GameObject go1, GameObject go2, double ms)
        {
            if (!go1.Is(Rule.EATABLE) || !go2.Is(Rule.EATABLE)) return;
            if (go1 is BlackHole || go2 is BlackHole) return;

            // Check distance
            double L = (go1.Location - go2.Location).Length();

            // Check for distance too large
            if (go1.Radius + go2.Radius <= L) return;

            if ((go1 is AntiMatter || go2 is AntiMatter) && !(go1 is AntiMatter && go2 is AntiMatter))
            {

                GameObject gL, gS;
                if(go1.Radius > go2.Radius)
                {
                    gL = go1;
                    gS = go2;
                }
                else
                {
                    gL = go2;
                    gS = go1;
                }

                // gL moet zoveel kleiner worden als dat gS is. Hierna moet er worden gecheckt of
                double LostMass = gS.Mass * 30;

                gL.Mass -= LostMass;

                gS.Mass -= gS.Mass;

                //pf.BOT.Remove(gS);

                if (gL.Mass <= 0)
                    pf.BOT.Remove(gL);

                if (gS.Mass <= 0)
                    pf.BOT.Remove(gS);

            }
            else
            {
                // Find largest and smallest
                GameObject gL, gS;
                if (go1.Mass > go2.Mass)
                {
                    gL = go1;
                    gS = go2;
                }
                else
                {
                    gL = go2;
                    gS = go1;
                }

                // Check for eat flags
                if (!gS.Is(Rule.EATABLE)) return;
                if (!gL.Is(gS is Player ? Rule.EAT_PLAYER : Rule.EATS)) return;

                // Check for too close
                double T = gL.Mass + gS.Mass;
                if (Math.Sqrt(T / Math.PI) > L)
                {
                    gL.Mass = T;
                    gS.Mass = 0;

                    // Bereken score? Animeer score!
                    if(!(gS is Player) && (gL is Player))
                    {
                        Player p = gL as Player;
                        sb.AddScore(new Score(50, DateTime.Now, gL.Location));
                    }

                    pf.BOT.Remove(gS);
                    return;
                }

                // Do magic
                double B = T;
                double temp1 = Math.PI * L * L - T;

                double C = temp1 * temp1;
                double D = Math.Sqrt(B * B - C);

                double glM = (B + D) / 2;

                // Set new velocity
                gL.DV = (gL.DV * gL.Mass + gS.DV * (glM - gL.Mass)) / glM;

                // Set new masses
                gL.Mass = glM;
                gS.Mass = (B - D) / 2;
            }
        }
    }
}
