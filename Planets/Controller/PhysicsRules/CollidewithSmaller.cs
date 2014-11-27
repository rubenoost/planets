using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class CollidewithSmaller : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            int collisionCheckCount = 0;
            for (int i = 0; i < pf.GameObjects.Count; i++)
            {
                var go1 = pf.GameObjects[i];
                if (!go1.Traits.HasFlag(Rule.COLLIDES)) continue;
                for (int j = i + 1; j < pf.GameObjects.Count; j++)
                {
                    var go2 = pf.GameObjects[j];
                    if (!go2.Traits.HasFlag(Rule.COLLIDES)) continue;
                    collisionCheckCount++;
                    CheckObjectCollission(go1,go2);
                }
            }
        }

        public void CheckObjectCollission(GameObject c1, GameObject c2)
        {

        }
    }
}
