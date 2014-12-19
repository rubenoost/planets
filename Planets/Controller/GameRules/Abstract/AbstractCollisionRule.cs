using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules.Abstract
{
    /// <summary>
    /// GameRule used to give a useful interface for collisionchecking.
    /// </summary>
    public abstract class AbstractCollisionRule : AbstractGameRule
    {
        /// <summary>
        /// Execute this GameRule.
        /// </summary>
        /// <param name="pf">The Playfield for this GameRule.</param>
        /// <param name="ms">The amount of milliseconds that elapsed since last call.</param>
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            pf.BOT.DoCollisions(
                (go1, go2, dt) =>
                {
                    DoCollision(pf, pf.sb, go1, go2);
                }, ms);
        }

        /// <summary>
        /// Do collision.
        /// </summary>
        /// <param name="pf">The playfield this collision occured on.</param>
        /// <param name="sb">The scoreboard scores can be added to on this collision.</param>
        /// <param name="go1">The first GameObject to collide.</param>
        /// <param name="go2">The second GameObjct to collide.</param>
        protected abstract void DoCollision(Playfield pf, ScoreBoard sb, GameObject go1, GameObject go2);
    }
}
