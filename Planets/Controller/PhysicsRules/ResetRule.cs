using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class ResetRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Reset if too low
            if (pf.CurrentPlayer.mass < 10)
            {
                pf.GameObjects.Clear();
                pf.CurrentPlayer = new Player(200, 200, 0, 0, Utils.StartMass);
            }
        }
    }
}
