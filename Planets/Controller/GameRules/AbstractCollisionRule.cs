using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules
{
    public abstract class AbstractCollisionRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            pf.BOT.DoCollisions(
                (go1, go2, dt) =>
                {
                    DoCollision(pf, go1, go2, dt);
                }, ms);
        }

        protected abstract void DoCollision(Playfield pf, GameObject go1, GameObject go2, double ms);
    }
}
