using System.Linq;

namespace Planets.Controller.GameRules.GameTargets
{
    /// <summary>
    /// GameTarget to get the largest.
    /// </summary>
    public class GameTargetGetLargest : IGameTarget
    {
        /// <summary>
        /// Checks whether player is largest now.
        /// </summary>
        /// <param name="ge">The GameEngine.</param>
        /// <returns>Boolean indicating whether the target is reached.</returns>
        public bool IsTargetReached(GameEngine ge)
        {
            // Get player mass, buffered for performance
            double playerMass = ge.field.CurrentPlayer.Mass;

            // Check if largest
            return ge.field.BOT.GameObjectList.All(g => g.Mass < playerMass);
        }
    }
}
