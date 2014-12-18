namespace Planets.Controller.GameRules.GameTargets
{
    /// <summary>
    /// Target of a game.
    /// </summary>
    public interface IGameTarget
    {
        /// <summary>
        /// Checks if this target is reached.
        /// </summary>
        /// <param name="ge">The GameEngine to check for if the target is reached.</param>
        /// <returns></returns>
        bool IsTargetReached(GameEngine ge);
    }
}
