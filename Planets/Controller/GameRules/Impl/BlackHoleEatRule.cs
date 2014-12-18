using Planets.Controller.GameRules.Abstract;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules.Impl
{

    class BlackHoleEatRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            pf.BOT.Iterate(go =>
            {
                if (!(go is BlackHole)) return;
                pf.BOT.Iterate(go2 =>
                {
                    if (!go2.Is(Rule.EATABLE)) return;
                    if (!go.Is(go2 is Player ? Rule.EAT_PLAYER : Rule.EATS)) return;

                    if (go != go2 && go.IntersectsWith(go2))
                    {
                        if (go2 is Player)
                        {
                            if (go.Is(Rule.EAT_PLAYER))
                                pf.BOT.Remove(go2);
                        }
                        else
                        {
                            if (go2 is AntiMatter) return;
                            pf.BOT.Remove(go2);
                        }
                        AnimatedGameObject explosion = new AnimatedGameObject(go2.Location, new Vector(0, 0), 500);
                        explosion.Radius = go2.Radius * 3.0;
                        pf.BOT.Add(explosion);
                    }
                });
            });
        }
    }
}

