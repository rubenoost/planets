using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules
{
    class MoveRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            pf.BOT.Iterate(g =>
            {
                if (g.Is(Rule.MOVE))
                    g.UpdateLocation(ms);
            });
        }
    }
}
