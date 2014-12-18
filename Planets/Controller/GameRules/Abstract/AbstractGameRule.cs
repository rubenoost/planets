using Planets.Model;

namespace Planets.Controller.GameRules.Abstract
{
    /// <summary>
    /// GameRule used to give a useful interface for the Playfield.
    /// </summary>
    public abstract class AbstractGameRule : INativeGameRule
    {

        /// <summary>
        /// Execute this GameRule, the call is redirected to ExecuteRule.
        /// </summary>
        /// <param name="ge">The GameEngine this GameRule is running on.</param>
        /// <param name="ms">The amount of milliseconds that elapsed since last call.</param>
        public void Execute(GameEngine ge, double ms)
        {
            // Execute rule
            ExecuteRule(ge.field, ms);
        }

        /// <summary>
        /// Execute this GameRule.
        /// </summary>
        /// <param name="pf">The Playfield for this GameRule.</param>
        /// <param name="ms">The amount of milliseconds that elapsed since last call.</param>
        protected abstract void ExecuteRule(Playfield pf, double ms);
    }
}
