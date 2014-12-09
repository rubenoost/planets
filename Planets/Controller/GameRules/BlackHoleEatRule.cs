using Planets.Model;
using Planets.View.Imaging;

namespace Planets.Controller.PhysicsRules
{

    class BlackHoleEatRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            pf.BOT.Iterate(go =>
            {
                if (!(go is BlackHole) || !go.Is(Rule.EATS)) return;
                pf.BOT.Iterate(go2 =>
                {
                    if (!go2.Is(Rule.EATABLE)) return;
                    if (go != go2 && go.IntersectsWith(go2))
                    {
                        if (go2 is Player)
                        {
                            if (go.Is(Rule.EAT_PLAYER))
                                pf.BOT.Remove(go2);
                            //pf.BOT.Add(new Explosion(new Vector(10, 10), new Vector(50, 50), 50000));
                        }
                        else
                        {
                            if (go2 is AntiMatter) return;
                            pf.BOT.Remove(go2);
                        }
                    }
                });
            });
        }
    }
}

