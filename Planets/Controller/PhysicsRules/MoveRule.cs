using System.Linq;
using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class MoveRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            foreach (GameObject go in pf.GameObjects.Where(p => p.Traits.HasFlag(Rule.MOVE)))
            {
                go.UpdateLocation(ms);
            }
        }
    }
}
