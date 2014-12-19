using System.Linq;
using Planets.Model.GameObjects;

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
            double playerMass = ge.Field.CurrentPlayer.Mass;

            // Check if largest
            bool reached = true;
            ge.Field.BOT.Iterate(g =>
            {
                if (g.GetType() == typeof(GameObject))
                    reached = reached && (g.Mass < playerMass);
            });
            return reached;
        }
    }
}
