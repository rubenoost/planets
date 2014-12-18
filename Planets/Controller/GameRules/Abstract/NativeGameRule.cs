namespace Planets.Controller.GameRules.Abstract
{
    /// <summary>
    /// This class is used as the native gamerule, a GameEngine is given to this function to get access to every function.
    /// </summary>
    public interface INativeGameRule
    {
        /// <summary>
        /// Execute this GameRule.
        /// </summary>
        /// <param name="ge">The GameEngine this GameRule is running in</param>
        /// <param name="ms">The amount of milliseconds elapsed since the last call.</param>
        void Execute(GameEngine ge, double ms);
    }
}
