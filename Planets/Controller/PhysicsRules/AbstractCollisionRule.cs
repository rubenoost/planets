using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    public abstract class AbstractCollisionRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            pf.BOT.DoCollisions(
                (go1, go2, dt) =>
                {
                    if (go1.Traits.HasFlag(Rule.COLLIDES) && go2.Traits.HasFlag(Rule.COLLIDES))
                        DoCollision(pf, go1, go2, dt);
                }, ms);
        }

        protected abstract void DoCollision(Playfield pf, GameObject go1, GameObject go2, double ms);
    }
}
