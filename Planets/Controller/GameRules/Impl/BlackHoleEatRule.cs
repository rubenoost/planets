using Planets.Controller.GameRules.Abstract;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules.Impl
{

    class BlackHoleEatRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            pf.GameObjects.Iterate(go =>
            {
                if (!(go is BlackHole)) return;
                pf.GameObjects.Iterate(go2 =>
                {
                    if (!go2.Is(Rule.Eatable)) return;
                    if (!go.Is(go2 is Player ? Rule.EatPlayer : Rule.Eats)) return;

                    if (go != go2 && go.IntersectsWith(go2))
                    {
                        if (go2 is Player)
                        {
                            if (go.Is(Rule.EatPlayer))
                                pf.GameObjects.Remove(go2);
                        }
                        else
                        {
                            if (go2 is AntiMatter) return;
                            pf.GameObjects.Remove(go2);
                        }
                        var explosion = new AnimatedGameObject(go2.Location, new Vector(0, 0), 500) { Radius = ((int) go2.Radius) / 5 * 5};
                        pf.GameObjects.Add(explosion);
                    }
                });
            });
        }
    }
}

