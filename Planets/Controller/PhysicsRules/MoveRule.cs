using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class MoveRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            pf.BOT.Iterate(g =>
            {
                if (g.Traits.HasFlag(Rule.MOVE))
                {
                    g.UpdateLocation(ms);
                }
            });
        }
    }
}
