using Planets.Controller.GameRules.Abstract;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules.Impl
{
    class MoveRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            pf.GameObjects.Iterate(g =>
            {
                if (g.Is(Rule.Move))
                    g.UpdateLocation(ms);
            });
        }
    }
}
