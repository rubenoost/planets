using Planets.Model;

namespace Planets.Controller.PhysicsRules
{

    class BlackHoleEatRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            pf.BOT.Iterate(go =>
            {
                if (!(go is BlackHole) || !go.Traits.HasFlag(Rule.EATS)) return;
                pf.BOT.Iterate(go2 =>
                {
                    if (!go2.Traits.HasFlag(Rule.EATABLE)) return;
                    if (go != go2 && go.IntersectsWith(go2))
                    {
                        if (go2 is Player)
                        {
                            if (go.Traits.HasFlag(Rule.EAT_PLAYER))
                                pf.BOT.Remove(go2);
                        }
                        else
                        {
                            pf.BOT.Remove(go2);
                        }
                    }
                });
            });
        }
    }
}

