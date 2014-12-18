using System.Linq;

namespace Planets.Controller.GameRules.GameTargets
{
    public class GameTargetGetLargest : IGameTarget
    {
        public bool IsTargetReached(GameEngine ge)
        {
            // Get player mass, buffered for performance
            double playerMass = ge.field.CurrentPlayer.Mass;

            // Check if largest
            return ge.field.BOT.GameObjectList.All(g => g.Mass < playerMass);
        }
    }
}
