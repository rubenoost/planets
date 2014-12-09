using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planets.Controller.PhysicsRules;

namespace Planets.Controller.GameRules
{
    class ScoreRule : AbstractGameRule
    {

        protected override void ExecuteRule(Model.Playfield pf, double ms)
        {

            pf.BOT.Iterate(go =>
                {
                    // first GameObject



                });

        }

    }
}
