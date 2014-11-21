using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class MoveRule : AbstractGameRule
    {
        internal override void Execute(Playfield pf, double ms)
        {
            foreach (GameObject go in pf.GameObjects)
            {
                go.UpdateLocation(ms);
            }
        }
    }
}
