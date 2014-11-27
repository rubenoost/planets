using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planets.Model;

namespace Planets.Controller.PhysicsRules
{

    class EatRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            var removed = new List<GameObject>();

            foreach (GameObject go in pf.GameObjects.Where(p => p.Traits.HasFlag(Rule.EATS)).Where(p => p is BlackHole))
            {
                foreach (GameObject go2 in pf.GameObjects.Where(p => p.Traits.HasFlag(Rule.EATABLE)))
                {
                    if (go != go2 && go.IntersectsWith(go2))
                    {
                        if (go2 is Player)
                        {
                            if (go.Traits.HasFlag(Rule.EAT_PLAYER))
                                removed.Add(go2);
                        }
                        else
                        {
                            removed.Add(go2);
                        }
                    }
                }
            }
            removed.ForEach(g => pf.GameObjects.Remove(g));
        }
    }
}

