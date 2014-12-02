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
        protected override void DoCollision(Playfield pf, GameObject go1, GameObject go2, double ms)
        {
            // Check distance
            double L = (go1.Location - go2.Location).Length();

            // Check for distance too large
            if (go1.Radius + go2.Radius <= L) return;

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
            if (!gS.Traits.HasFlag(Rule.EATABLE)) return;
            if (!gL.Traits.HasFlag(gS is Player ? Rule.EAT_PLAYER : Rule.EATS)) return;

            // Check for too close
            double T = gL.Mass + gS.Mass;
            if (Math.Sqrt(T / Math.PI) > L)
            {
                gL.Mass = T;
                gS.Mass = 0;
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
